using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public Sprite[] backGroundSprites;
    public Transform gifManagerObject;
    public int Frames;

    GifManager gifManager;

    void Start()
    {
        gifManager = gifManagerObject.GetComponent<GifManager>();
        gifManager.SetParameters(backGroundSprites, Frames);
    }
}
