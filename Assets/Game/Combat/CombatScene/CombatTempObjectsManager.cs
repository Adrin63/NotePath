using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatTempObjectsManager : MonoBehaviour
{
    public TextMeshProUGUI[] HitTexts;

    [SerializeField]
    float timeTillDespawn = 2f;
    float timeTillDespawnCD;

    private void Start()
    {
        timeTillDespawnCD = timeTillDespawn;
    }

    public void GenerateDamage(int damageToDeal, int identifier)//0-Player, 1-Enemie
    {
        StartCoroutine(MakeDamageNum(damageToDeal, identifier));
    }

    IEnumerator MakeDamageNum(int damageToDeal, int identifier)
    {
        timeTillDespawnCD = timeTillDespawn;
        HitTexts[identifier].text = damageToDeal.ToString();
        HitTexts[identifier].alpha = 255;

        float t = 0;

        Color32 startColor = new Color(255, 255, 255, 255);
        Color32 endColor = new Color(255, 255, 255, 0);

        while (timeTillDespawnCD > 0)
        {
            HitTexts[identifier].color = Color32.Lerp(startColor, endColor, t);
            t += Time.deltaTime;

            timeTillDespawnCD -= Time.deltaTime;
            yield return null;
        }

        HitTexts[identifier].text = "";
    }
}
