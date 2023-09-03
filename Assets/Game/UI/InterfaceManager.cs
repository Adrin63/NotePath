using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceManager : MonoBehaviour {
    public enum InterfaceType { QUESTS, OBJECTS, ATTACKS, ENEMIES };
    public InterfaceType type;
    [Header("Main")]
    public LakeGameManager gameManager;
    public LakeMissionManager missionManager;
    public Obj objectManager;
    public PlayerBehaviour player;
    [Header("Cameras")]
    public Camera questsCam;
    public Camera attacksCam;
    public Camera objectsCam;
    public Camera enemiesCam;
    [Header("Controls")]
    public TextMeshProUGUI controlsTxt;
    [Header("Quests")] // ~Al ser la demo, solamente habrá una misión principal
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI descriptionText;
    [Header("Attacks")] // ~Sólo tendrá cuatro ataques (DEMO)
    public TextMeshProUGUI attackTitle;
    public TextMeshProUGUI attackDescription;
    public Button attack01;
    public Button attack02;
    public Button attack03;
    public Button attack04;
    [Header("Objects")] // ~Sólo se mostrarán unos pocos (DEMO)
    public TextMeshProUGUI objectTitle;
    public TextMeshProUGUI objectDescription;
    public Transform[] objPlaceholders;
    public RawImage[] imagePlaceholders;
    public Texture[] objects;
    [Header("Enemies")] // ~En este nivel sólo hay tres (DEMO)
    public RawImage enemyImg;
    public TextMeshProUGUI enemyDescription;
    public TextMeshProUGUI enemyStats;
    public Button enemy01;
    public Button enemy02;
    public Button enemy03;
    [Space]
    public Texture lakeDragon;
    public Texture lakeBull;
    public Texture lakeBoss;

    enum Attacks { MASTER_TOUCH, CARMEN, PAUSE, REQUIEM, NONE };
    enum Enemies { DRAGON, BULL, LAKE_BOSS, NONE };
    Attacks currentAttack;
    Enemies currentEnemy;
    int drumSlot = 0;
    float controlsTimer = 5f; // Show the controls

	void Start() {
        // All cams deactivated
        questsCam.gameObject.SetActive(false);
        attacksCam.gameObject.SetActive(false);
        objectsCam.gameObject.SetActive(false);
        enemiesCam.gameObject.SetActive(false);

        // Empty strings
        attackTitle.text = attackDescription.text = "";        
        objectTitle.text = objectDescription.text = "";
        enemyDescription.text = enemyStats.text = "";

        // Objects image transparent
        for (int i = 0; i < imagePlaceholders.Length; i++) {
            Color colors = imagePlaceholders[i].color;
            colors.a = 0f;
            imagePlaceholders[i].color = colors;
        }

        // There's no attack or enemy selected in the interface
        currentAttack = Attacks.NONE;
        currentEnemy = Enemies.NONE;
    }

    void Update() {
        if (controlsTimer <= 0f) { controlsTxt.gameObject.SetActive(false); }
        else { controlsTimer -= Time.deltaTime; }
    }

	/// <summary>
	/// Checks the current interface type.
	/// </summary>
	public void CheckInterface() {
        switch (type) {
            case InterfaceType.QUESTS:
                InQuests();
                break;
            case InterfaceType.ATTACKS:
                InAttacks();
                break;
            case InterfaceType.OBJECTS:
                InObjects();
                break;
            case InterfaceType.ENEMIES:
                InEnemies();
                break;
            default: break;
        }
    }

    /// <summary>
    /// Deactivate the selected camera.
    /// </summary>
    ///<param name="cam">The selected camera to deactive.</param>
    public void DeactivateInterface(Camera cam) {
        cam.gameObject.SetActive(false);
    }

    #region BUTTONS

    // QUESTS INTERFACE

    /// <summary>
    /// Quests -> Attacks
    /// </summary>
    public void NextTo() {
        type = InterfaceType.ATTACKS;
        gameManager.interfaceType = LakeGameManager.InterfaceType.ATTACKS;
    }

    // ATTACKS INTERFACE

    /// <summary>
    /// Attacks -> Objects
    /// </summary>
    public void NextToObjects() {
        type = InterfaceType.OBJECTS;
        gameManager.interfaceType = LakeGameManager.InterfaceType.OBJECTS;
    }

    /// <summary>
    /// Attacks -> Quests
    /// </summary>
    public void BackToQuests() {
        type = InterfaceType.QUESTS;
        gameManager.interfaceType = LakeGameManager.InterfaceType.QUESTS;
    }

    public void MasterTouch() {
        currentAttack = Attacks.MASTER_TOUCH;
    }

    public void Carmen() {
        currentAttack = Attacks.CARMEN;
    }

    public void Pause() {
        currentAttack = Attacks.PAUSE;
    }

    public void Requiem() {
        currentAttack = Attacks.REQUIEM;
    }

    // OBJECTS INTERFACE

    /// <summary>
    /// Objects -> Enemies
    /// </summary>
    public void NextToEnemies() {
        type = InterfaceType.ENEMIES;
        gameManager.interfaceType = LakeGameManager.InterfaceType.ENEMIES;
    }

    /// <summary>
    /// Objects -> Attacks
    /// </summary>
    public void BackToAttacks() {
        type = InterfaceType.ATTACKS;
        gameManager.interfaceType = LakeGameManager.InterfaceType.ATTACKS;
    }

    /// <summary>
    /// Put the object's image when picked up.
    /// </summary>
    /// <param name="_id">Slot id.</param>
    public void Slots(int _id) {
        if ((int)objectManager.objId == _id) { drumSlot = _id; }

        // Set the image
        imagePlaceholders[_id].texture = objects[(int)objectManager.objId];

        // Set alpha to maximum
        Color colors = imagePlaceholders[_id].color;
        colors.a = 255f;
        imagePlaceholders[_id].color = colors;
    }

    /// <summary>
    /// Shows the title and the description of an object.
    /// </summary>
    /// <param name="i">The index of the object stored in the list.</param>
    public void ShowInfo(int i) {
        if (objectManager.slot0 || objectManager.slot1 || objectManager.slot2 || objectManager.slot3 || objectManager.slot4 || objectManager.slot5) {
            // Get every value from the list
            int[] values = player.objectsList.ToArray();

            if (i < player.objectsList.Count)
                switch (values[i]) {
                    case 0: // Guitar
                        objectTitle.text = "Guitar";
                        objectDescription.text = "Relax the character, restoring 10 points of health.";
                        break;
                    case 1: // Electric Bass
                        objectTitle.text = "Electric Bass";
                        objectDescription.text = "For the next three turns, the character restores 5 points of health.";
                        break;
                    case 2: // Piano
                        objectTitle.text = "Piano";
                        objectDescription.text = "Makes a musical accompaniment to the character, making a sfere of attack and defense autocomplete.";
                        break;
                    case 3: // Drum
                        objectTitle.text = "Drum";
                        objectDescription.text = "Sets the musical rythm, making spheres going at 25% speed for the next three turns of defense. " +
                        "Also, it can be used to destroy some obstacles.";
                        break;
                    case 4: // Virtuous Violin
                        objectTitle.text = "Virtuous Violin";
                        objectDescription.text = "The violin increases in a 50% the total damage in the next turn.";
                        break;
                    case 5: // French Horn
                        objectTitle.text = "French Horn";
                        objectDescription.text = "Scares the enemy, who will recive a 40% more damage in the next attack.";
                        break;
                    case 6: // Drum Kit
                        objectTitle.text = "Drum Kit";
                        objectDescription.text = "Increase the sferes' speed during the attack, in exchange of causing the double of damage.";
                        break;
                    case 7: // Saxophone
                        objectTitle.text = "Saxophone";
                        objectDescription.text = "With a pure jazz style, the enemy loses 50 points of health instantly.";
                        break;
                    case 8: // Hamelin's Flute
                        objectTitle.text = "Hamelin's Flute";
                        objectDescription.text = "A legendary instrument. It has the power to put the rats off.";
                        break;
                    default:
                        objectTitle.text = "";
                        objectDescription.text = "";
                        break;
                }
		}
    }

    /// <summary>
    /// Deletes an object from the inventory.
    /// </summary>
    /// <param name="objectId">The object to delete.</param>
    public void DeleteObject(int objectId) {
        int[] values = player.objectsList.ToArray();

        for (int i = 0; i < values.Length; i++) {
            // Check the object
            if (values[i] == objectId) {
                if (objectManager.slot0 || objectManager.slot1 || objectManager.slot2 || objectManager.slot3 || objectManager.slot4 || objectManager.slot5) {
                    // Set alpha to zero in order to hide the texture
                    Color colors = imagePlaceholders[drumSlot].color;
                    colors.a = 0f;
                    imagePlaceholders[drumSlot].color = colors;

                    // Remove the object from the list
                    player.objectsList.Remove(objectId);

                    // Let the current slot free to save an object
                    if (drumSlot == 0) objectManager.slot0 = false;
                    else if (drumSlot == 1) objectManager.slot1 = false;
                    else if (drumSlot == 2) objectManager.slot2 = false;
                    else if (drumSlot == 3) objectManager.slot3 = false;
                    else if (drumSlot == 4) objectManager.slot4 = false;
                    else if (drumSlot == 5) objectManager.slot5 = false;
                }
            }
        }
    }

    // ENEMIES INTERFACE

    /// <summary>
    /// Enemies -> Objects
    /// </summary>
    public void BackToObjects() {
        type = InterfaceType.OBJECTS;
        gameManager.interfaceType = LakeGameManager.InterfaceType.OBJECTS;
    }

    public void SelectLakeDragon() {
        currentEnemy = Enemies.DRAGON;
    }

    public void SelectLakeBull() {
        currentEnemy = Enemies.BULL;
    }

    public void SelectLakeBoss() {
        currentEnemy = Enemies.LAKE_BOSS;
    }

    #endregion

    #region INTERFACES
    
    void InQuests() {
        // Deactivate cams & button
        DeactivateInterface(attacksCam);
        DeactivateInterface(objectsCam);
        DeactivateInterface(enemiesCam);

        // Activate current cam
        questsCam.gameObject.SetActive(true);

        // Properties
        missionManager.Quest(0, missionText, descriptionText);
    }

    void InAttacks() { /// *IMAGES
        // Deactivate cams
        DeactivateInterface(questsCam);
        DeactivateInterface(objectsCam);
        DeactivateInterface(enemiesCam);

        // Activate current cam
        attacksCam.gameObject.SetActive(true);

        // Properties
        attackTitle.gameObject.SetActive(true);
        attackDescription.gameObject.SetActive(true);

            // Change the title and the description depending on the selected attack
        if (currentAttack == Attacks.MASTER_TOUCH) {
            attackTitle.text = "MASTER TOUCH"; // Set the title
            attackDescription.text = "Throw a note that causes the 100% of damage."; // Set the description text
        } else if (currentAttack == Attacks.CARMEN) {
            attackTitle.text = "CARMEN";
            attackDescription.text = "With the power of Bizet, throw two notes that " +
            "reduce the enemy's damage to the half.";
        } else if (currentAttack == Attacks.PAUSE) {
            attackTitle.text = "PAUSE";
            attackDescription.text = "Execute a short song that restores 15 points " +
            "of health.";
        } else if (currentAttack == Attacks.REQUIEM) {
            attackTitle.text = "REQUIEM";
            attackDescription.text = "With the power of Mozart, cause the 110% of " +
            "damage in exchange of losing 25 points of health";
        } else if (currentAttack == Attacks.NONE) {
            attackTitle.text = attackDescription.text = "";
		}
    }

    void InObjects() {
        // Deactivate cameras
        DeactivateInterface(questsCam);
        DeactivateInterface(attacksCam);
        DeactivateInterface(enemiesCam);

        // Activate current cam
        objectsCam.gameObject.SetActive(true);

        // Properties
            // Objects
    }

    void InEnemies() {
        // Deactivate cameras
        DeactivateInterface(questsCam);
        DeactivateInterface(attacksCam);
        DeactivateInterface(objectsCam);

        // Activate current cam
        enemiesCam.gameObject.SetActive(true);

        // Properties
        if (currentEnemy == Enemies.DRAGON) { // DRAGON
            Color colors = enemyImg.color;
            colors.a = 255f;
            enemyImg.color = colors;

            enemyImg.texture = lakeDragon; // Change the texture
            enemyDescription.text = // Set the description text
            "The harmony made them peaceful, but now they have been corrupted by " +
            "heavy metal and attack anybody who cross their territory. It seems " +
            "they prefer damage.";
            enemyStats.text = "DP: 5\tHP: 7"; // Set the values
        } else if (currentEnemy == Enemies.BULL) { // BULL
            Color colors = enemyImg.color;
            colors.a = 255f;
            enemyImg.color = colors;

            enemyImg.texture = lakeBull;
            enemyDescription.text =
            "Bulls were alredy dangerous, but now they are more nervous than " +
            "normal. Despite their endurance, they are not immortals";
            enemyStats.text = "DP: 3\tHP: 10";
        } else if (currentEnemy == Enemies.LAKE_BOSS) { // LAKE BOSS
            Color colors = enemyImg.color;
            colors.a = 255f;
            enemyImg.color = colors;

            enemyImg.texture = lakeBoss;
            enemyDescription.text =
            "This one used to be benevolent who faced Doom's Gate, but despite " +
            "its great voice, he lost and was cursed to serve the band for the eternity";
            enemyStats.text = "DP: 10\tHP: 50";
        } else if (currentEnemy == Enemies.NONE) {
            Color colors = enemyImg.color;
            colors.a = 0f;
            enemyImg.color = colors;

            enemyDescription.text = "";
        }
    }

    #endregion

    /// <summary>
    /// Check if Doja has the drum in the inventory.
    /// </summary>
    /// <returns>True or false, depending on the drum in the inventory or not.</returns>
    public bool CheckDrum() {
        bool drum = false;

        if (objectManager.slot0 || objectManager.slot1 || objectManager.slot2 || objectManager.slot3 || objectManager.slot4 || objectManager.slot5) {
            int[] values = player.objectsList.ToArray();

            for (int i = 0; i < values.Length; i++)
                if (values[i] == 3) {
                    drum = true;
                    Debug.Log("I have the drum");
                    //DeleteObject(3);
                } else {
                    drum = false;
                    Debug.LogWarning("I don't have the drum");
                }
        } else {
            Debug.LogWarning("INVENTORY EMPTY");
        }

        return drum;
    }

    /*public void DeleteObject(int id) {
        int[] values = objectManager.objectsList.ToArray();

        for (int i = 0; i < values.Length; i++)
            if (values[i] == id) {
                Color colors = imagePlaceholders[i].color;
                colors.a = 0f;
                imagePlaceholders[i].color = colors;

                objectManager.objectsList.Remove(i);

                Debug.Log(values[i]);
            }
    }*/

    /// <summary>
    /// Clean texts and textures from interfaces.
    /// </summary>
    public void CleanTexts() {
        // Clean texts
        attackTitle.text = attackDescription.text = "";
        objectTitle.text = objectDescription.text = "";
        enemyDescription.text = enemyStats.text = "";

        // "Clear" images
        Color colors = enemyImg.color;
        colors.a = 0f;
        enemyImg.color = colors;

        // Other
        currentAttack = Attacks.NONE;
        currentEnemy = Enemies.NONE;
    }
}