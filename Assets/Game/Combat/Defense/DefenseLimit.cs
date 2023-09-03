using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseLimit : MonoBehaviour
{
    public Transform defenseManagerObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DefenseNote")
        {
            defenseManagerObject.GetComponent<DefenseManager>().OnDestroyNote(collision.gameObject.GetComponent<DefenseNote>().identifier, -1, true);
        }
    }
}
