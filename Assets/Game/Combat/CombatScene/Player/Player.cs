using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] playerSprites;
    public Transform gifManagerObject;
    
    GifManager gifManager;

    void Start()
    {
        gifManager = gifManagerObject.GetComponent<GifManager>();
        gifManager.SetParameters(playerSprites, 7);
    }
}
