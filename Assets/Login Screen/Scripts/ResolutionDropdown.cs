using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    Dropdown myDropdown;
    public Toggle fullscreenBool;

    void Start()
    {
        myDropdown = GetComponent<Dropdown>();
        if (PlayerPrefs.GetInt("Resolution") == 0)
            myDropdown.value = 0;
        else if (PlayerPrefs.GetInt("Resolution") == 1)
            myDropdown.value = 1;
        else if (PlayerPrefs.GetInt("Resolution") == 2)
            myDropdown.value = 2;
        else if (PlayerPrefs.GetInt("Resolution") == 3)
            myDropdown.value = 3;
    }

    void Update()
    {
        /*switch (myDropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, fullscreenBool);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, fullscreenBool);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullscreenBool);
                break;
            case 3:
                Screen.SetResolution(600, 400, fullscreenBool);
                break;

        }

        PlayerPrefs.SetInt("Resolution", myDropdown.value);*/
    }

    public void DropdownSwitch(int n)
    {
        switch (n)
        {
            case 0:
                Screen.SetResolution(1920, 1080, fullscreenBool.isOn);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, fullscreenBool.isOn);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullscreenBool.isOn);
                break;
            case 3:
                Screen.SetResolution(600, 400, fullscreenBool.isOn);
                break;

        }

        PlayerPrefs.SetInt("Resolution", myDropdown.value);
    }
}
