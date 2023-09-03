using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public float secondsToWait1, secondsToWait2;

    public Image black, logoSA, colaborationLogo;

    public Transform logo;

    bool start1 = true;

    void Start()
    {
        if (!SavedIntroManager.singleton.introDone)
        {
            logo.gameObject.SetActive(false);
            StartCoroutine(FadeInAndOut());
        }
        else
        {
            start1 = false;
        }
    }

    IEnumerator FadeInAndOut()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            logoSA.color = new Color(1, 1, 1, i);
            yield return null;
        }
        
        yield return new WaitForSeconds(secondsToWait1);
        
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            logoSA.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            colaborationLogo.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(secondsToWait2);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            colaborationLogo.color = new Color(1, 1, 1, i);
            yield return null;
        }

        logo.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);

        SavedIntroManager.singleton.introDone = true;
        start1 = false;
    }

    private void Update()
    {
        if (!start1)
            transform.gameObject.SetActive(false);
    }
}
