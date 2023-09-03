using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LakeMissionManager : MonoBehaviour {
    public InterfaceManager interfaceManager;

    /// <summary>
    /// Shows in the quests interface the missions.
    /// </summary>
    /// <param name="id">The mission id.</param>
    /// <param name="missionTxt">The mission's objective (TMP Object)</param>
    /// <param name="descriptionTxt">The mission's description (TMP Object)</param>
    public void Quest(int id, TextMeshProUGUI missionTxt, TextMeshProUGUI descriptionTxt) {
        if (id == 0) {
            missionTxt.text = "Recover Hamelin's Flute";
            descriptionTxt.text = "There is a plague of rats in Zimmer, " +
            "I must find the flute to solve this. All we know is that a " +
            "member of Doom's Gate took it years ago.";
        }
    }
}