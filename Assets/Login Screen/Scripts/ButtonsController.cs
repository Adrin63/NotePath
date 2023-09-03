using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform Logo, NameTitle;
    public Transform mainMenuObject;
    public AudioClip select, click;
    public float secondsToWait;

    MainMenu mainMenuScript;
    AudioSource myAudioSource;
    Vector3 cachedScale;

    void Start()
    {
        mainMenuScript = mainMenuObject.GetComponent<MainMenu>();
        myAudioSource = transform.GetComponent<AudioSource>();

        myAudioSource.clip = select;

        cachedScale = transform.localScale;
    }

    public void Click(int option)
    {
        myAudioSource.clip = click;

        if(option == 1) // Play game
        {
            StartCoroutine(Wait(secondsToWait));
            mainMenuScript.PlayGame();
        }
        else if (option == 2) // Options
        {
            Logo.gameObject.SetActive(false);
            NameTitle.gameObject.SetActive(false);

            StartCoroutine(Wait(secondsToWait));
            mainMenuObject.gameObject.SetActive(false);
        }
        else if (option == 3) // Quit
        {
            StartCoroutine(Wait(secondsToWait));
            mainMenuScript.QuitGame();
        }

        myAudioSource.clip = select;
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.localScale = cachedScale;
    }

    public void EnterJukeBox()
    {
        SceneManager.LoadScene("JukeBox");
    }
}
