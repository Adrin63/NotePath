using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifManager : MonoBehaviour
{
    public Transform Objective;

    Sprite[] frames;
    int fps;
    int spriteLength;
    bool start;

    void Update()
    {
        if (start)
        {
            int index = (int)(Time.time * fps) % frames.Length;
            Objective.GetComponent<SpriteRenderer>().sprite = frames[index];
        }
    }

    public void SetParameters(Sprite[] sprites, int fps)
    {
        spriteLength = sprites.Length;
        frames = new Sprite[spriteLength];
        frames = sprites;
        this.fps = fps;
        start = true;
    }

}
