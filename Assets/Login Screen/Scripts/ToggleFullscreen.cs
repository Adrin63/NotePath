using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    Toggle myToggle;

    private void Start()
    {
        myToggle = transform.gameObject.GetComponent<Toggle>();
        if (PlayerPrefs.GetInt("Fullscreen") == 0)//false
            myToggle.isOn = false;
        else if (PlayerPrefs.GetInt("Fullscreen") == 1)//true
            myToggle.isOn = true;
    }

    private void Update()
    {
        if (myToggle.isOn)
            PlayerPrefs.SetInt("Fullscreen", 1);
        else
            PlayerPrefs.SetInt("Fullscreen", 0);
    }

    public void Fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
