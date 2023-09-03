using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalNoteSpriteSelector : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeMusicalNoteSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
