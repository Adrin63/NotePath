using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentObject : MonoBehaviour {
    public Transform playerObj;
    public enum ObjectId {
        // COMBAT OBJECTS
        GUITAR,
        ELECTRIC_BASS, // Bajo eléctrico
        PIANO,
        DRUM, // Tambor
        VIRUOUS_VIOLIN,
        FRENCH_HORN, // Trompa
        DRUM_KIT, // Batería
        SAXOPHONE,
        // QUEST OBJECTS
        HAMELINS_FLUTE,
        //...
    }

    public ObjectId objId;
    
    private void Start()
    {
        if (SavedDataManager.singleton.objects.Contains((int)objId))
        {
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, playerObj.position);
        Gizmos.DrawWireSphere(transform.position, transform.GetComponent<CircleCollider2D>().radius * 5f);
	}
}