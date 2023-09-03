using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform[] attackNotesObject;
    public Transform[] pressedNotesObject;
    public Transform[] startPositionAttackNotes;
    public Transform[] startPositionPressedNotes;
    public Transform noteBackgroundObject;
    public Transform combatManagerObject;

    [Header("Sprites")]
    public Sprite[] musicalNoteSprites;
    public Sprite[] AttackNoteSprites;

    [Header("Parameters")]
    public float attackNoteSpeedMin = 1f;
    public float attackNoteSpeedMax = 4f;

    [Header("Music")]
    public AudioSource atackAudioSource;

    AttackNote[] attackNotes;
    MusicalNoteSpriteSelector[] pressedNoteSpriteSelectors;
    CombatManager combatManager;

    enum State
    {
        Moving, Limit, CornerDamage, HalfDamage, FullDamage
    }

    State state, nextState;

    bool halfHitting, fullHitting;

    int actualNote, totalNotes;

    bool onAttacking;

    int totalDamage;

    void Start()
    {
        pressedNoteSpriteSelectors = new MusicalNoteSpriteSelector[pressedNotesObject.Length];
        attackNotes = new AttackNote[attackNotesObject.Length];

        for (int i = 0; i < attackNotesObject.Length; i++) { attackNotes[i] = attackNotesObject[i].gameObject.GetComponent<AttackNote>(); }

        for (int i = 0; i < pressedNotesObject.Length; i++) { pressedNoteSpriteSelectors[i] = pressedNotesObject[i].gameObject.GetComponentInChildren<MusicalNoteSpriteSelector>(); }

        combatManager = combatManagerObject.GetComponent<CombatManager>();
    }

    void Update()
    {
        if (onAttacking) OnAttack();
    }

    public void StartAttack()
    {
        Cursor.visible = false;

        ActivateAllObjects();

        for (int i = 0; i < attackNotesObject.Length; i++) { attackNotesObject[i].position = startPositionAttackNotes[i].position; }
        for (int i = 0; i < pressedNotesObject.Length; i++) { pressedNotesObject[i].position = startPositionPressedNotes[i].position; }

        for (int i = 0; i < attackNotesObject.Length; i++)
        {
            int attackNoteSpriteNum = Random.Range(0, AttackNoteSprites.Length);
            int musicalNoteSpriteNum = Random.Range(0, musicalNoteSprites.Length);
            attackNotes[i].StartAttackNote(AttackNoteSprites[attackNoteSpriteNum], musicalNoteSprites[musicalNoteSpriteNum], attackNoteSpriteNum);
            pressedNoteSpriteSelectors[i].ChangeMusicalNoteSprite(musicalNoteSprites[musicalNoteSpriteNum]);
        }

        for (int i = 0; i < attackNotes.Length; i++)
        {
            attackNotes[i].movingParticle.gameObject.SetActive(false);
            attackNotes[i].cornerDamageParticle.gameObject.SetActive(false);
            attackNotes[i].halfDamageParticle.gameObject.SetActive(false);
            attackNotes[i].fullDamageParticle.gameObject.SetActive(false);
            attackNotes[i].failHitParticle.gameObject.SetActive(false);
        }

        nextState = State.Moving;
        state = nextState;
        
        totalDamage = 0;

        actualNote = 0;
        totalNotes = attackNotesObject.Length - 1;

        onAttacking = true;

        switch (combatManager.difficulty)
        {
            case 0:
                attackNoteSpeedMin = 2f;
                attackNoteSpeedMax = 4f;
                break;
            case 1:
                attackNoteSpeedMin = 2f;
                attackNoteSpeedMax = 6f;
                break;
            case 2:
                attackNoteSpeedMin = 3f;
                attackNoteSpeedMax = 8f;
                break;
            case 3:
                attackNoteSpeedMin = 5f;
                attackNoteSpeedMax = 9f;
                break;
        }
    }

    void OnAttack()
    {
        if (state == State.Moving)
        {
            Vector2 pos = attackNotesObject[actualNote].position;
            pos -= new Vector2(0.0f, attackNotes[actualNote].attackNoteSpeed) * Time.deltaTime;
            attackNotesObject[actualNote].position = pos;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (halfHitting && !fullHitting) { nextState = State.CornerDamage; }
                else if (halfHitting && fullHitting) { nextState = State.HalfDamage; }
                else if (!halfHitting && fullHitting) { nextState = State.FullDamage; }
                else if (actualNote != totalNotes) 
                { 
                    attackNotes[actualNote].failHitParticle.gameObject.SetActive(true);
                    attackNotes[actualNote].movingParticle.gameObject.SetActive(false);
                    actualNote++; 

                }
                else
                {
                    EndAttack();
                }
                atackAudioSource.PlayOneShot(atackAudioSource.clip);
            }
            
        }
        else if (state == State.FullDamage)
        {
            Debug.Log("100!");

            totalDamage += 100;

            if (actualNote != totalNotes) { actualNote++; nextState = State.Moving; }
            else
            {
                EndAttack();
            }
        }
        else if (state == State.HalfDamage)
        {
            Debug.Log("50!");

            totalDamage += 50;

            if (actualNote != totalNotes) { actualNote++; nextState = State.Moving; }
            else
            {
                EndAttack();
            }
        }
        else if (state == State.CornerDamage)
        {
            Debug.Log("25!");

            totalDamage += 25;

            if (actualNote != totalNotes) { actualNote++; nextState = State.Moving; }
            else
            {
                EndAttack();
            }
        }
        else if (state == State.Limit)
        {
            Debug.Log("Went off");

            if (actualNote != totalNotes) { actualNote++; nextState = State.Moving; }
            else
            {
                EndAttack();
            }
        }


        if(nextState == State.Moving)
        {
            attackNotes[actualNote].movingParticle.gameObject.SetActive(true);
        }
        else if(nextState == State.CornerDamage)
        {
            attackNotes[actualNote].cornerDamageParticle.gameObject.SetActive(true);
        }
        else if (nextState == State.HalfDamage)
        {
            attackNotes[actualNote].halfDamageParticle.gameObject.SetActive(true);
        }
        else if (nextState == State.FullDamage)
        {
            attackNotes[actualNote].fullDamageParticle.gameObject.SetActive(true);
        }

        if(nextState != State.Moving)
        {
            attackNotes[actualNote].movingParticle.gameObject.SetActive(false);
            halfHitting = false;
            fullHitting = false;
        }

        if (state != nextState)
        {
            state = nextState;
        }
    }

    void EndAttack()
    {
        onAttacking = false;
        combatManager.MakeDamage(totalDamage, 1);
    }

    public void OnHalfDamageTriggering(bool enters) { halfHitting = enters; }
    public void OnFullDamageTriggering(bool enters) { fullHitting = enters; }
    public void OnLimitReached() { nextState = State.Limit; }

    public void DisableAllObjects()
    {
        for (int i = 0; i < attackNotesObject.Length; i++) { attackNotesObject[i].gameObject.SetActive(false); }
        for (int i = 0; i < pressedNotesObject.Length; i++) { pressedNotesObject[i].gameObject.SetActive(false); }
        noteBackgroundObject.gameObject.SetActive(false);
    }

    public void ActivateAllObjects()
    {
        for (int i = 0; i < attackNotesObject.Length; i++) { attackNotesObject[i].gameObject.SetActive(true); }
        for (int i = 0; i < pressedNotesObject.Length; i++) { pressedNotesObject[i].gameObject.SetActive(true); }
        noteBackgroundObject.gameObject.SetActive(true);
    }
}