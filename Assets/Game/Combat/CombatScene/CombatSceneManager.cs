using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSceneManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform attackManagerObject;
    public Transform defenseManagerObject;
    public Transform cameraObject;
    public Transform UIManagerObject;
    public Transform combatTempObjectsManagerObject;
    public Transform enemieSpriteSelectorObject;
    public Transform combatManagerObject;

    [Header("Parameters")]
    public float maxPlayerHealth = 500;
    public float currentHealth;
    public float maxEnemieHealth = 500;
    [HideInInspector] public float currentEnemieHealth;
    public float timeBetweenAttackAndDefense = 3.0f;
    float timeBetweenAttackAndDefenseCD;

    public float timeBetweenDefenseAndMenu = 3.0f;
    float timeBetweenDefenseAndMenuCD;

    [Space][Space]
    public int difficultySelected = 0;

    AttackManager attackManager;
    DefenseManager defenseManager;
    FollowCamera followingCamera;
    CombatMenuManager UIManager;
    EnemieSelector enemieSelector;
    CombatManager combatManager;

    enum State
    {
        Menu, Attack, Defense, BetweenAttackAndDefense, BetweenDefenseAndMenu
    }

    State state, nextState;

    private void Start()
    {
        attackManager = attackManagerObject.GetComponent<AttackManager>();
        defenseManager = defenseManagerObject.GetComponent<DefenseManager>();
        UIManager = UIManagerObject.GetComponent<CombatMenuManager>();
        followingCamera = cameraObject.GetComponent<FollowCamera>();
        enemieSelector = enemieSpriteSelectorObject.GetComponent<EnemieSelector>();
        combatManager = combatManagerObject.GetComponent<CombatManager>();
        StartCombatScene();
    }

    void Update()
    {
        //Estado
        if (state == State.BetweenAttackAndDefense)
        {
            if(timeBetweenAttackAndDefenseCD <= 0)
            {
                nextState = State.Defense;
                timeBetweenAttackAndDefenseCD = timeBetweenAttackAndDefense;
            }
            else { timeBetweenAttackAndDefenseCD -= Time.deltaTime; }
        }
        else if (state == State.BetweenDefenseAndMenu)
        {
            if (timeBetweenDefenseAndMenuCD <= 0)
            {
                nextState = State.Menu;
                timeBetweenDefenseAndMenuCD = timeBetweenDefenseAndMenu;
            }
            else { timeBetweenDefenseAndMenuCD -= Time.deltaTime; }
        }

        //Transiciones
        if (state == State.Menu && nextState == State.Attack)
        {
            followingCamera.MoveDown();

            attackManager.ActivateAllObjects();
            attackManager.StartAttack();

            UIManager.MenuToHide();
        }
        else if (state == State.Attack && nextState == State.BetweenAttackAndDefense)
        {
        }
        else if (state == State.BetweenAttackAndDefense && nextState == State.Defense)
        {
            followingCamera.MoveDown();
            attackManager.DisableAllObjects();
            defenseManager.ActivateAllObjects();
            defenseManager.StartDefense();
        }
        else if (state == State.Defense && nextState == State.BetweenDefenseAndMenu)
        {
        }
        else if (state == State.BetweenDefenseAndMenu && nextState == State.Menu)
        {
            defenseManager.DisableAllObjects();
            followingCamera.MoveCenter();
            UIManager.MenuToDefault();
        }

        if (state != nextState)
        {
            state = nextState;
        }
    }

    public void StartCombatScene()
    {
        enemieSelector.ChangeEnemie(difficultySelected);

        Cursor.visible = true;
        timeBetweenAttackAndDefenseCD = timeBetweenAttackAndDefense;
        timeBetweenDefenseAndMenuCD = timeBetweenDefenseAndMenu;
        currentEnemieHealth = maxEnemieHealth;
        
        combatManager.SetHealthBarLife(currentHealth/maxPlayerHealth, 0);
        UIManager.MenuToDefault();
        attackManager.DisableAllObjects();
        defenseManager.DisableAllObjects();
    }

    public void OnStartAttack() { nextState = State.Attack; }
    public void OnEndAttack() { nextState = State.BetweenAttackAndDefense; }    

    public void OnStartDefense() { nextState = State.Defense; }
    public void OnEndDefense() { nextState = State.BetweenDefenseAndMenu; }

    public void OnEnemieDie()
    {
        if (SavedDataManager.singleton.difficulty == 3)
            SceneManager.LoadScene("DemoEnd");
        else
        { 
            SavedDataManager.singleton.OnLeaveScene();
            SceneManager.LoadScene("Lake");
        }
    }

    public void OnPlayerDie() {
        SceneManager.LoadScene("Defeat");
    }
}