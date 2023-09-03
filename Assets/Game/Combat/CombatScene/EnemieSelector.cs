using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieSelector : MonoBehaviour
{
    public Transform[] enemieSelected;

    public void ChangeEnemie(int newEnemie)
    {
        for(int i = 0; i < enemieSelected.Length; i++) { enemieSelected[i].gameObject.SetActive(false); }

        enemieSelected[newEnemie].gameObject.SetActive(true);
    }
}
