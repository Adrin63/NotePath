using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JukeBoxManager : MonoBehaviour
{
    public Transform vinylObject;
    public Transform songNameObject;
    public AudioSource audioSource;
    public Sprite[] posibleVinyl;
    public Song[] SongList;

    TextMeshProUGUI songName;
    int currentSong;
    SpriteRenderer vinyl;

    void Start()
    {
        songName = songNameObject.GetComponent<TextMeshProUGUI>();
        vinyl = vinylObject.GetComponent<SpriteRenderer>();
        currentSong = 0;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSource.Stop();
            currentSong++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSource.Stop();
            currentSong--;
        }

        if (currentSong <= 0)
            currentSong = 0;
        if (currentSong >= SongList.Length - 1)
            currentSong = SongList.Length - 1;

        if (SongList[currentSong].ID == 0)
            vinyl.sprite = posibleVinyl[0];
        else
            vinyl.sprite = posibleVinyl[1];

        songName.text = SongList[currentSong].Name;
        audioSource.clip = SongList[currentSong].AudioClip;

        if (!audioSource.isPlaying)
            audioSource.Play();
        else
        {

        }
    }
}
