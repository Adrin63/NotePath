using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public int difficulty;
    bool firstTime;

    void Start()
    {
        firstTime = SavedDataManager.singleton.triggerPassed[difficulty];

        if (firstTime)
            GetComponentInChildren<SpriteRenderer>().sprite = null;

    }

    // ENTERING COMBAT SAMPLE CODE
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !firstTime) {

            SavedDataManager.singleton.triggerPassed[difficulty] = true;

            SavedDataManager.singleton.difficulty = difficulty;
            SavedDataManager.singleton.OnLeaveScene();
            SceneManager.LoadScene("Combat", LoadSceneMode.Single);
        }
    }
}