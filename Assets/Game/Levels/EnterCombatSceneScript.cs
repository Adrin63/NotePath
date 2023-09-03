using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCombatSceneScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("SUUU");
        if (collision.gameObject.tag == "Player")
            SceneManager.LoadScene("Combat", LoadSceneMode.Single);

    }
}
