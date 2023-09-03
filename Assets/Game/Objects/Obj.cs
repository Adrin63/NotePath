using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Obj : MonoBehaviour {
    [Header("Main")]
    public Transform playerObj;
    public Transform interfaceObj;
    public TextMeshProUGUI pickedUpTxt;
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
    [Header("Slot checker")] // Checks if a slot in inventory is used or not
    public bool slot0;
    public bool slot1;
    public bool slot2;
    public bool slot3;
    public bool slot4;
    public bool slot5;
    //[Space]
    //public List<int> objectsList;

    PlayerBehaviour player;
    InterfaceManager im;
    bool pickedUpObj = false;
    bool fullInventory = false;
    float timer = 0f;

    void Awake() {
        player = playerObj.GetComponent<PlayerBehaviour>();
        im = interfaceObj.GetComponent<InterfaceManager>();
	}

	void Start() {
        pickedUpTxt.gameObject.SetActive(false);
        slot0 = slot1 = slot2 = slot3 = slot4 = slot5 = false; // There is no object in the inventory
        //objectsList = new List<int>();
    }

	void Update() {
        // Display text for 3 secs
        if (pickedUpObj) {
            pickedUpTxt.gameObject.SetActive(true);
            pickedUpTxt.text = "You picked up: " + objId.ToString();

            // Display text
            if (timer >= 3f) {
                timer = 0f;
                pickedUpObj = false;
                pickedUpTxt.gameObject.SetActive(false);
            } else { timer += Time.deltaTime; }
        } else if (fullInventory) {
            Color color = pickedUpTxt.color;
            color.r = 255f;
            color.g = color.b = 0f;
            pickedUpTxt.color = color;

            pickedUpTxt.gameObject.SetActive(true);
            pickedUpTxt.text = "You haven't available slots!";
        }
    }

    #region SETTERS_GETTERS

    public bool GetFullInventory() { return fullInventory; }

    #endregion

    #region OBJECTS_INTERFACE

    /// <summary>
    /// The character picks up an object.
    /// </summary>
    /// <param name="id">The id of the object.</param>
    public void PickUpObject(CurrentObject.ObjectId id) {
        switch (id) {
            case CurrentObject.ObjectId.GUITAR:
                objId = ObjectId.GUITAR;
                break;
            case CurrentObject.ObjectId.ELECTRIC_BASS:
                objId = ObjectId.ELECTRIC_BASS;
                break;
            case CurrentObject.ObjectId.PIANO:
                objId = ObjectId.PIANO;
                break;
            case CurrentObject.ObjectId.DRUM:
                objId = ObjectId.DRUM;
                break;
            case CurrentObject.ObjectId.VIRUOUS_VIOLIN:
                objId = ObjectId.VIRUOUS_VIOLIN;
                break;
            case CurrentObject.ObjectId.FRENCH_HORN:
                objId = ObjectId.FRENCH_HORN;
                break;
            case CurrentObject.ObjectId.DRUM_KIT:
                objId = ObjectId.DRUM_KIT;
                break;
            case CurrentObject.ObjectId.SAXOPHONE:
                objId = ObjectId.SAXOPHONE;
                break;
            case CurrentObject.ObjectId.HAMELINS_FLUTE:
                objId = ObjectId.HAMELINS_FLUTE;
                break;
        }

        pickedUpObj = true;

        // Add the object to the list
        player.objectsList.Add((int)objId);

        // Add it to the inventory, as long as there's a free slot
        if (!slot0) {
            im.Slots(0);
            slot0 = true;
		} else if (!slot1) {
            im.Slots(1);
            slot1 = true;
        } else if (!slot2) {
            im.Slots(2);
            slot2 = true;
        } else if (!slot3) {
            im.Slots(3);
            slot3 = true;
        } else if (!slot4) {
            im.Slots(4);
            slot4 = true;
        } else if (!slot5) {
            im.Slots(5);
            slot5 = true;
            fullInventory = true; // Now it's full
        } else {
            // No slots available
            Debug.LogWarning("There are no slots available!");
		}
    }

    #endregion
}