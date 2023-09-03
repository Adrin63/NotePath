using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackMainMenu : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "DemoEnd" || SceneManager.GetActiveScene().name == "Defeat")
            Cursor.visible = true;
    }

    public void EnterMainMenu()
    {
        if(SavedDataManager.singleton != null)
            SavedDataManager.singleton.firstTime = true;

        SceneManager.LoadScene("LoginScreen");
    }
}
