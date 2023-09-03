using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehaviour : MonoBehaviour {
    [Header("Global parameters")]
    public Transform gmObj;
    [Space]
    public float speed;
    public float range;
    public LayerMask signsLayer;
    public LayerMask npcLayer;
    public LayerMask obstacleLayer;
    public LayerMask bossLayer;
    [Header("UI")]
    public TextMeshProUGUI interactionTxt;
    [Header("Objects")]
    public Transform[] objectsObj;
    public Transform omObj;
    public Image healthBar;
    public const int MAX_OBJECTS = 6; // Won't change
    [Header("Booleans")]
    public bool moveTrunk = false;
    public bool onTheOtherSide = false;
    [Header("Other params")]
    public int dialogueId;
    public Animator anim;
    public List<int> objectsList;
    [Header("AudioSources")]
    public AudioSource footSteps;
    public AudioSource pickUpItem;

    LakeGameManager gameManager;
    Obj objectManager;
    float health = 500f;
    readonly float maxHealth = 500f;
    Vector2 movement;
    Rigidbody2D rb;
    CurrentObject[] objects;
    bool interactDialogue;

    void Awake() {
        gameManager = gmObj.GetComponent<LakeGameManager>();
        objectManager = omObj.GetComponent<Obj>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        objects = new CurrentObject[objectsObj.Length];

        for (int i = 0; i < objectsObj.Length; i++) {
            objects[i] = objectsObj[i].GetComponent<CurrentObject>();
		}
    }

    void Start() {
        // Object list
        objectsList = new List<int>();

        // UI
        interactionTxt.gameObject.SetActive(false);
    }

    void Update() {
        interactDialogue = Input.GetKeyDown(KeyCode.E) && !gameManager.inDialogue;
    }

    #region SETTERS_GETTERS

    public void SetHealth(float h) { health = h; }
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// Player inputs.
    /// </summary>
    public void Inputs() {
        // Get axis X and Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animation parameters
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            if (!footSteps.isPlaying)
                footSteps.Play();
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            footSteps.Stop();
        }
    }

	/// <summary>
	/// Makes the player move through the map with the RigidBody2D. It also makes the animation happen.
	/// </summary>
	public void Movement() {
        // Movement with Rigidbody2D
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Detects if the player is able to interact with the entities in game.
    /// </summary>
    public void Detections() {
        // NPCs detection
        Collider2D npc = Physics2D.OverlapCircle(transform.position, range, npcLayer);

        if (npc != null)
            if (interactDialogue) {
                int id = npc.GetComponent<NPC>().id;

                if (id == 1) dialogueId = 5;
                else if (id == 2) dialogueId = 6;

                gameManager.inPause = gameManager.inDialogue = true;
            }

        // Signs detecion
        Collider2D sign = Physics2D.OverlapCircle(transform.position, range, signsLayer);

        if (sign != null)
            if (interactDialogue)
                if (sign.name == "Sign_0") {
                    dialogueId = 1;
                    gameManager.inPause = gameManager.inDialogue = true;
                } else if (sign.name == "Sign_1") {
                    dialogueId = 2;
                    gameManager.inPause = gameManager.inDialogue = true;
                }

        // Trunk obstacle detection
        Collider2D trunkObstacle = Physics2D.OverlapCircle(transform.position, range, obstacleLayer);

        if (trunkObstacle != null)
            if (interactDialogue)
                if (!gameManager.interfaceManager.CheckDrum()) {
                    dialogueId = 0;
                    gameManager.inPause = gameManager.inDialogue = true;
                } else {
                    dialogueId = 4;
                    gameManager.inPause = gameManager.inDialogue = true;
                }

        // Boss detection
        Collider2D boss = Physics2D.OverlapCircle(transform.position, range, bossLayer);

        if (boss != null)
            if (interactDialogue) {
                dialogueId = 3;
                gameManager.inPause = gameManager.inDialogue = true;
            }
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.fillAmount = value;
    }

    #endregion

    #region UNITY_FUNCTIONS

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Trunk")) interactionTxt.gameObject.SetActive(true);
        //else if (collider.CompareTag("CombatTrigger")) Debug.LogWarning("COMBAT TRIGGERED!");
        //else if (collider.CompareTag("BossTrigger")) Debug.LogWarning("BOSS TRIGGERED!");
        else if (collider.CompareTag("Object"))
            // The inventory is not full
            if (!objectManager.GetFullInventory()) {
                pickUpItem.Play();
                CurrentObject.ObjectId id = collider.gameObject.GetComponent<CurrentObject>().objId; // Get its id
                objectManager.PickUpObject(id); // Pick up the object through its id
                collider.gameObject.SetActive(false); // Deactivate the object
            }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag == "Trunk" && Input.GetKey(KeyCode.E)) moveTrunk = true;
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Trunk")) interactionTxt.gameObject.SetActive(false);
    }

    #endregion

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 0.11f, 0f), range);
    }
}