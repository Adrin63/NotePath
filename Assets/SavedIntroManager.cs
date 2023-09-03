using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedIntroManager : MonoBehaviour
{
    public static SavedIntroManager singleton;

    public bool introDone;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
