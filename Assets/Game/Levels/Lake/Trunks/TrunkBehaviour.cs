using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set the Debug function from Unity Engine to avoid future issues
using Debug = UnityEngine.Debug;

public class TrunkBehaviour : MonoBehaviour {
    public PlayerBehaviour player;
    public float firstPos;
    public float finalPos;
    [Space]
    public float posX;
    [Space]
    public float offset = 1.5f;
    public float speed;

    void Update() {
        if (player.moveTrunk && !player.onTheOtherSide) { MoveFordward(); } // Current side
        else if (player.moveTrunk && player.onTheOtherSide) { MoveBackward(); } // Other side
    }


    void MoveFordward() {
        if (transform.position.y < finalPos) { // Player goes to the island
            player.gameObject.transform.position = transform.position;
            transform.position += new Vector3(0f, speed, 0f) * Time.deltaTime;
        } else if (transform.position.y >= finalPos) { // Player has arrived to the island
            player.moveTrunk = false;
            player.gameObject.transform.position = new Vector3(gameObject.transform.position.x, finalPos + offset, 0f);
            player.onTheOtherSide = true;
        }
    }

    void MoveBackward() {
        if (transform.position.y > firstPos) { // Player goes to the lake path
            player.gameObject.transform.position = transform.position;
            transform.position -= new Vector3(0f, speed, 0f) * Time.deltaTime;
        } else if (transform.position.y <= firstPos) { // Player has arrived to the lake path
            player.moveTrunk = false;
            player.gameObject.transform.position = new Vector3(transform.position.x, firstPos - offset, 0f);
            player.onTheOtherSide = false;
        }
    }
}