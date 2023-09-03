using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public float speed = -3;
    public Transform secretButton;

    AudioSource myAudioSource;
    AudioClip myAudioClip;
    int contador = 0;

    void Start()
    {
        myAudioSource = transform.GetComponent<AudioSource>();
        myAudioClip = myAudioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
        //StartCoroutine("StartPulsing");
       
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.name == "Logo-Juego")
                {
                    contador++;
                    Debug.Log(contador);
                }
            }
        }

        if (contador >= 10 && secretButton.gameObject.activeSelf == false)
        {
            secretButton.gameObject.SetActive(true);
            myAudioSource.PlayOneShot(myAudioClip);
        }
    }

    private IEnumerator StartPulsing()
    {

        for(float i = 0f; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector3(
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x + 0.05f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y + 0.05f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.z, transform.localScale.z + 0.05f, Mathf.SmoothStep(0f, 1f, i)))
                );                                                              
            yield return new WaitForSeconds(0.015f);                            
        }                                                                       
                                                                                
        for (float i = 0f; i <= 1f; i += 0.1f)                                  
        {                                                                       
            transform.localScale = new Vector3(                                 
                (Mathf.Lerp(transform.localScale.x, transform.localScale.x - 0.05f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.y, transform.localScale.y - 0.05f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(transform.localScale.z, transform.localScale.z - 0.05f, Mathf.SmoothStep(0f, 1f, i)))
                );
            yield return new WaitForSeconds(0.015f);
        }
    }
}
