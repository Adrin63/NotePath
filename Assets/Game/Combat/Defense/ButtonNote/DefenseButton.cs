using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseButton : MonoBehaviour
{
    public Transform defenseManagerObject;
    DefenseManager defenseManager;

    public Sprite pressedSprite;

    public int buttonNum;

    public KeyCode keyButton;

    public ParticleSystem destroyNoteParticle;

    float timeBetweenHits = 0.1f;
    float timeBetweenHitsCD;

    SpriteRenderer defenseSpriteRenderer;
    Sprite defaultSprite;

    bool pressed, canDestroyNote = false;

    public void Start()
    {
        defenseSpriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = defenseSpriteRenderer.sprite;
        defenseManager = defenseManagerObject.GetComponent<DefenseManager>();
        timeBetweenHitsCD = timeBetweenHits;
        canDestroyNote = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyButton))
        {
            
            pressed = true;
            canDestroyNote = true;
        }

        if (Input.GetKeyUp(keyButton))
        {
            pressed = false; 
            timeBetweenHitsCD = timeBetweenHits;
        }

        if (pressed)
        {
            timeBetweenHitsCD -= Time.deltaTime;

            if (timeBetweenHitsCD <= 0)
                canDestroyNote = false;
        }

        OnButtonPressed(pressed);
    }

    public void OnButtonPressed(bool pressed)
    {
        if (pressed)
        {
            defenseSpriteRenderer.sprite = pressedSprite;
        }
        else { defenseSpriteRenderer.sprite = defaultSprite; }
    }

    public void OnTriggerStay2D(Collider2D collision)
        {
        if (collision.tag == "DefenseNote" && canDestroyNote)
        {
            defenseManager.OnDestroyNote(collision.GetComponent<DefenseNote>().identifier, collision.GetComponent<DefenseNote>().row, false);
        }
    }

    public void DisableButton()
    {
        if (pressed)
            pressed = false;
    }
}
