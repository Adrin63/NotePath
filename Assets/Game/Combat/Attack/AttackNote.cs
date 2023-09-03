using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNote : MonoBehaviour
{
    public SpriteRenderer spriteRendererAttackNote;
    public Transform musicNoteGameObject;

    MusicalNoteSpriteSelector mNote;

    public Transform attackManagerObject;
    AttackManager attackManager;

    [Header("Particles")]
    public ParticleSystem movingParticle;
    public ParticleSystem cornerDamageParticle;
    public ParticleSystem halfDamageParticle;
    public ParticleSystem fullDamageParticle;
    public ParticleSystem failHitParticle;
    public Color[] particleColors;
    public float attackNoteSpeed;

    void Start()
    {
        mNote = musicNoteGameObject.gameObject.GetComponent<MusicalNoteSpriteSelector>();
        attackManager = attackManagerObject.GetComponent<AttackManager>();
    }

    public void StartAttackNote(Sprite spriteAttackNote, Sprite spriteMusicalNote, int color)
    {
        spriteRendererAttackNote.sprite = spriteAttackNote;
        mNote.ChangeMusicalNoteSprite(spriteMusicalNote);
        attackNoteSpeed = Random.Range(attackManager.attackNoteSpeedMin, attackManager.attackNoteSpeedMax);

        ParticleSystem.MainModule main = movingParticle.main;
        main.startColor = particleColors[color];
        main = cornerDamageParticle.main;
        main.startColor = particleColors[color];
        main = halfDamageParticle.main;
        main.startColor = particleColors[color];
        main = fullDamageParticle.main;
        main.startColor = particleColors[color];
        main = failHitParticle.main;
        main.startColor = particleColors[color];

        //for (int i = 0; i < cornerDamageParticle.gameObject.transform.childCount; i++) { cornerDamageParticle.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = particleColors[color]; }
        //for (int i = 0; i < halfDamageParticle.gameObject.transform.childCount; i++) { halfDamageParticle.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = particleColors[color]; }
        //for (int i = 0; i < fullDamageParticle.gameObject.transform.childCount; i++) { fullDamageParticle.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = particleColors[color]; }
        //for (int i = 0; i < failHitParticle.gameObject.transform.childCount; i++) { failHitParticle.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = particleColors[color]; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collider50") { attackManager.OnHalfDamageTriggering(true); }
        else if (collision.tag == "Collider100") { attackManager.OnFullDamageTriggering(true); }
        if (collision.tag == "Limit") { attackManager.OnLimitReached(); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Collider50") { attackManager.OnHalfDamageTriggering(false); }
        else if (collision.tag == "Collider100") { attackManager.OnFullDamageTriggering(false); }
    }

    public void ChangeAttackSpeed(float speed)
    {
        attackNoteSpeed = speed;
    }
}