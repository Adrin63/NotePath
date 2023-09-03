using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public void BackToMenu()
    {
        SavedDataManager.singleton.firstTime = true; 
        SceneManager.LoadScene("LoginScreen");
    }
}