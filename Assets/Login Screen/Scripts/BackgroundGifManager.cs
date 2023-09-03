using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundGifManager : MonoBehaviour
{
    public Transform Objective;

    public Sprite[] frames;
    public int fps;
    int spriteLength;

    private void Start()
    {
        Objective = transform;
    }

    void Update()
    {
        SetParameters(frames, fps);

        int index = (int)(Time.time * fps) % frames.Length;
        Objective.GetComponent<Image>().sprite = frames[index];
    }

    public void SetParameters(Sprite[] sprites, int fps)
    {
        spriteLength = sprites.Length;
        frames = new Sprite[spriteLength];
        frames = sprites;
        this.fps = fps;
    }
}
