using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform combatSceneManagerObject;
    public Transform damageTextsManagerObject;//CombatTempObjectsManager

    [Header("HealthBars")]
    public Image[] healthBars;
    public Image[] lifeLost;

    public float timeInterpolatingLife = 1f;
    float timeInterpolatingLifeCD;

    [HideInInspector] public int difficulty;

    CombatSceneManager combatSceneManager;

    CombatTempObjectsManager damageTextsManager;

    bool interpolatingLife;

    float currentLife, startLifeForInterpolate, maxHealthToUpdate;

    int indexToUpdate;

    void Start()
    {
        combatSceneManager = combatSceneManagerObject.GetComponent<CombatSceneManager>();
        damageTextsManager = damageTextsManagerObject.GetComponent<CombatTempObjectsManager>();
        timeInterpolatingLifeCD = 0;
        interpolatingLife = false;

        difficulty = combatSceneManager.difficultySelected;
    }

    private void Update()
    {
        if (interpolatingLife)
        {
            float newHealth = Mathf.Lerp(startLifeForInterpolate, currentLife, timeInterpolatingLifeCD / timeInterpolatingLife);
            lifeLost[indexToUpdate].fillAmount = newHealth / maxHealthToUpdate;
            timeInterpolatingLifeCD += Time.deltaTime;

            if(timeInterpolatingLifeCD > timeInterpolatingLife)
            {
                interpolatingLife = false;
                lifeLost[indexToUpdate].fillAmount = 0f;
                timeInterpolatingLifeCD = 0;
            }
        }
    }

    public void MakeDamage(int damage, int index)//0-player 1-enemie
    {
        
        damageTextsManager.GenerateDamage(damage, index);

        indexToUpdate = index;
        interpolatingLife = true;

        if(index == 0)
        {
            startLifeForInterpolate = combatSceneManager.currentHealth;
            combatSceneManager.currentHealth -= damage;
            currentLife = combatSceneManager.currentHealth;
            maxHealthToUpdate = combatSceneManager.maxPlayerHealth;

            SetHealthBarLife(combatSceneManager.currentHealth / combatSceneManager.maxPlayerHealth, 0);
            
            if (combatSceneManager.currentHealth <= 0)
                combatSceneManager.OnPlayerDie();
            else
                combatSceneManager.OnEndDefense();
        }
        else
        {
            startLifeForInterpolate = combatSceneManager.currentEnemieHealth;
            combatSceneManager.currentEnemieHealth -= damage;
            currentLife = combatSceneManager.currentEnemieHealth;
            maxHealthToUpdate = combatSceneManager.maxEnemieHealth;

            SetHealthBarLife(combatSceneManager.currentEnemieHealth / combatSceneManager.maxEnemieHealth, 1);

            if (combatSceneManager.currentEnemieHealth <= 0)
                combatSceneManager.OnEnemieDie();
            else
                combatSceneManager.OnEndAttack();
        }
    }

    public void SetHealthBarLife(float a, int index)
    {
        healthBars[index].fillAmount = a;
        lifeLost[index].fillAmount = a;
    }
}
