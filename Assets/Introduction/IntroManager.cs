using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {
    public Image frontalPanel;

    public float timer = 15f;
    float s_timer = 5f;

    void Start() {
        frontalPanel.CrossFadeAlpha(0f, 3f, false);
    }

    void Update() {
        if (timer <= 0f) {
            frontalPanel.CrossFadeAlpha(1f, 1f, false);

            if (s_timer <= 0f) {
                SceneManager.LoadScene("Lake");
            } else { s_timer -= Time.deltaTime; }

            Debug.Log("DONE");
        } else { timer -= Time.deltaTime; }
    }
}