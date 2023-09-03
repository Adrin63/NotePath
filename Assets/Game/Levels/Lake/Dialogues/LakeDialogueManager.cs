using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LakeDialogueManager : MonoBehaviour {
    public LakeGameManager manager;
    public PlayerBehaviour player;
    [Space]
    public TextMeshProUGUI speakerNameTxt;

    /// <summary>
    /// Checks the current dialogue with its id.
    /// </summary>
    /// <param name="txt">The dialogue text TMPGUI component.</param>
    public void TextManager(TextMeshProUGUI txt) {
        if (manager.inDialogue)
            switch (player.dialogueId) {
                case 0:
                    speakerNameTxt.text = "Doja";
                    speakerNameTxt.color = Color.green;
                    Sign(txt, "I cannot break these trunks. If only I had a DRUM...");
                    break;
                case 1:
                    speakerNameTxt.text = "";
                    Sign(txt, "You can use the trunk to go from one place to another through the water.");
                    break;
                case 2:
                    speakerNameTxt.text = "";
                    Sign(txt, "This is Doom's Gate territory. Go away!");
                    break;
                case 3:
                    speakerNameTxt.text = "Libra";
                    speakerNameTxt.color = Color.red;
                    DialogueWithBOSS(txt);
                    break;
                case 4:
                    speakerNameTxt.text = "";
                    Sign(txt, "Doja plays the drum. The trunks have been destroyed.");
                    manager.SetDestroyObstacle(true);
                    break;
                case 5:
                    speakerNameTxt.text = "Bach";
                    speakerNameTxt.color = Color.cyan;
                    DialogueWithNPC_01(txt);
                    break;
                case 6:
                    speakerNameTxt.text = "Wagner";
                    speakerNameTxt.color = Color.cyan;
                    DialogueWitchNPC_02(txt);
                    break;
                default: break;
            }
    }

    // Reading a sign (before the hall to the boss zone).
    void Sign(TextMeshProUGUI txt, string message) {
        if (manager.times == 0) {
            txt.text = message;
        } else if (manager.times == 1) { CloseDialogue(); }
    }

    // Dialogue with Boss (boss zone).
    void DialogueWithBOSS(TextMeshProUGUI txt) {
        if (manager.times == 0)
            txt.text = "Well, well. Looks like someone has reached the end.";
        else if (manager.times == 1)
            txt.text = "I know what you are looking for. This... little flute, am I wrong?";
        else if (manager.times == 2)
            txt.text = "...";
        else if (manager.times == 3)
            txt.text = "Wait, am I talking to Ludwig van Beethoven? Are you deaf?";
        else if (manager.times == 4)
            txt.text = "Don't worry, I will heal you with MY voice, with Doom's Gate's power.";
        else if (manager.times == 5)
            txt.text = "Submit or fall before me!";
        else {
            CloseDialogue();
            manager.allowCombatWithBoss = true;
        }
    }

    // Dialogue with NPC_01
    void DialogueWithNPC_01(TextMeshProUGUI txt) {
        if (manager.times == 0)
            txt.text = "My entire house is full of those... rats. Ugh, disgusting!";
        else if (manager.times == 1)
            txt.text = "Our reports say the thief is at the deep zone of the lake. Good luck, Doja!";
        else { CloseDialogue(); }
    }

    // Dialogue with NPC_02
    void DialogueWitchNPC_02(TextMeshProUGUI txt) {
        if (manager.times == 0)
            txt.text = "Doja, be careful. This area is dangerous!";
        else if (manager.times == 1)
            txt.text = "Since Doom's Gate took the harmony from this region, everything has changed!";
        else if (manager.times == 2)
            txt.text = "For Mozart's sake, even the animals noticed!";
        else if (manager.times == 3)
            txt.text = "They have become savage and they are attacking everyone. I don't know what to do...";
        else { CloseDialogue(); }
    }

    /// <summary>
    /// Ends the dialogue and resets the counter used for change between lines of text.
    /// </summary>
    void CloseDialogue() {
        manager.inDialogue = manager.inPause = false;
        manager.times = 0;
    }
}