using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LakeGameManager : MonoBehaviour {
    [Header("Main")]
    public PlayerBehaviour player; // Player
    public InterfaceManager interfaceManager; // Interface management
    public LakeDialogueManager dialogue; // Dialogue management
    [Header("Cams")]
    public Camera pauseCam; // Pause interface
    public Camera textCam; // Dialogue interface
    public Camera optionsCam;
    [Header("Text")]
    public TextMeshProUGUI dialogueText;
    [Header("Buttons")] // For pause interface
    public Button resumeButton;
    public Button exitButton;
    [Header("Obstacle")]
    public Transform trunksObstacle;
    [Header("Boss trigger")]
    public Transform bossTrigger;
    [Header("Other parameters")]
    public Transform confirmationWindow;
    public bool inDialogue;
    public bool inPause;
    public bool inOptions;
    public bool isFighting;
    public bool inInterface;
    public enum InterfaceType { QUESTS, ATTACKS, OBJECTS, ENEMIES };
    public InterfaceType interfaceType;
    public int times; // An integer to change the current text during dialogues
    public bool dialogueFirstTime; // A boolean that avoids a minor bug during dialogues (some dialogues were omitted)
    public bool allowCombatWithBoss = false;

    // States
    enum GameStates { PLAYING, PAUSE, FIGHTING, DIALOGUE, INTERFACE, OPTIONS };
    GameStates state, nexState;

    // Pause
    bool allowMovement = true;

    // Obstacle
    bool destroyObstacle = false;

    void Start() {
        // Cameras
        pauseCam.gameObject.SetActive(false);
        textCam.gameObject.SetActive(false);
        optionsCam.gameObject.SetActive(false);

        // Global parameters
        inDialogue = inPause = isFighting = inInterface = inOptions = false;
        confirmationWindow.gameObject.SetActive(false);

        // Dialogues
        times = 0;
        dialogueFirstTime = true;

        // States
        state = nexState = GameStates.PLAYING;

        destroyObstacle = SavedDataManager.singleton.logEventDone;
    }

    void Update() {
        Inputs();

        bossTrigger.gameObject.SetActive(allowCombatWithBoss);

        if (state == GameStates.PLAYING) { // The player is playing
            Cursor.visible = false; // The cursor won't be shown
            pauseCam.gameObject.SetActive(false); // No pause
            textCam.gameObject.SetActive(false); // No dialogue

            // Interfaces are deactivated
            interfaceManager.DeactivateInterface(interfaceManager.questsCam);
            interfaceManager.DeactivateInterface(interfaceManager.attacksCam);
            interfaceManager.DeactivateInterface(interfaceManager.objectsCam);
            interfaceManager.DeactivateInterface(interfaceManager.enemiesCam);
            allowMovement = true;

            player.Detections();

            // Check obstacle
            if (destroyObstacle) { trunksObstacle.gameObject.SetActive(false); }
        } else if (state == GameStates.PAUSE) { // The player is in the pause menu
            Cursor.visible = true; // Shows the cursor

            // Interfaces are deactivated here too
            interfaceManager.DeactivateInterface(interfaceManager.questsCam);
            interfaceManager.DeactivateInterface(interfaceManager.attacksCam);
            interfaceManager.DeactivateInterface(interfaceManager.objectsCam);
            interfaceManager.DeactivateInterface(interfaceManager.enemiesCam);

            if (!inOptions) {
                pauseCam.gameObject.SetActive(true); // Actives only the pause interface
                optionsCam.gameObject.SetActive(false);
            } else {
                pauseCam.gameObject.SetActive(false);
                optionsCam.gameObject.SetActive(true);
            }


            allowMovement = false;
        } else if (state == GameStates.OPTIONS) {
            pauseCam.gameObject.SetActive(false);
            optionsCam.gameObject.SetActive(true);
        } else if (state == GameStates.FIGHTING) { // The player is in a fight
            Cursor.visible = true;
            allowMovement = false;

            /// *Combat code...
        } else if (state == GameStates.DIALOGUE) { // The player is in a dialogue
            textCam.gameObject.SetActive(true); // Shows the dialogue interface
            dialogue.TextManager(dialogueText); // Controls the text in the textbox
            allowMovement = false;
        } else if (state == GameStates.INTERFACE) { // The player is in the interface, but which one?
            Cursor.visible = true;
            allowMovement = false;

            // Check the selected interface
            switch (interfaceType) {
                case InterfaceType.QUESTS: // Shows the main quests
                    interfaceManager.type = InterfaceManager.InterfaceType.QUESTS;
                    break;
                case InterfaceType.ATTACKS: // Shows the current attacks
                    interfaceManager.type = InterfaceManager.InterfaceType.ATTACKS;
                    break;
                case InterfaceType.OBJECTS: // Shows the current combat objects
                    interfaceManager.type = InterfaceManager.InterfaceType.OBJECTS;
                    break;
                case InterfaceType.ENEMIES: // Shows the enemies
                    interfaceManager.type = InterfaceManager.InterfaceType.ENEMIES;
                    break;
                default: break;
            }

            interfaceManager.CheckInterface();
        }

        CheckState();

        if (state != nexState) {
            if (nexState != GameStates.INTERFACE) { interfaceManager.CleanTexts(); }

            state = nexState;
        }
    }

    void FixedUpdate() {
        if (allowMovement) { player.Movement(); }
    }

    public void SetDestroyObstacle(bool _destroyed) { destroyObstacle = _destroyed; SavedDataManager.singleton.DisableLogEvent(); }

#region FUNCTIONS

    /// <summary>
    /// Checks the next game state depending on the situation.
    /// </summary>
    void CheckState() {
        if (!inPause && !isFighting && !inInterface) { nexState = GameStates.PLAYING; }
        else if (inPause && !inDialogue) { nexState = GameStates.PAUSE; }
        else if (inPause && inDialogue) { nexState = GameStates.DIALOGUE; }
        else if (inInterface && !inPause && !inDialogue && !isFighting) { nexState = GameStates.INTERFACE; }
    }

    /// <summary>
    /// Inputs for menus (animations too).
    /// </summary>
    void Inputs() {
        if (Input.GetKeyDown(KeyCode.Escape)) { // Pause menu
            inInterface = false;
            inPause = !inPause;
            confirmationWindow.gameObject.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.Z)) { // Quests window
            inInterface = !inInterface;
            interfaceType = InterfaceType.QUESTS;
        } else if (Input.GetKeyDown(KeyCode.X)) { // Attacks window
            inInterface = !inInterface;
            interfaceType = InterfaceType.ATTACKS;
        } else if (Input.GetKeyDown(KeyCode.C)) { // Objects window
            inInterface = !inInterface;
            interfaceType = InterfaceType.OBJECTS;
        } else if (Input.GetKeyDown(KeyCode.V)) { // Enemies window
            inInterface = !inInterface;
            interfaceType = InterfaceType.ENEMIES;
        } else if (Input.GetKeyDown(KeyCode.E) && inDialogue) {
            // Change to the next line of dialogue.
            times++;
        }

        if (allowMovement) { player.Inputs(); }
    }

#endregion

    #region BUTTON_FUNCTIONS

    public void ResumeGame() { inPause = false; }

    public void ExitGame() {
        // Display: 'Are you sure you want to leave?'
        confirmationWindow.gameObject.SetActive(true);
    }

    public void OptionsMenu() { inOptions = true; }

    public void ReturnToPause() { inOptions = false; }

    public void Deny() {
        confirmationWindow.gameObject.SetActive(false);
    }

    public void Confirm() { SceneManager.LoadScene(0); }

    #endregion
}