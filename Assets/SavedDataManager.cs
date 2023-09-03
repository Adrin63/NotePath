using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavedDataManager : MonoBehaviour
{
    public static SavedDataManager singleton;

    PlayerBehaviour player;
    CombatSceneManager combatSceneManager;

    public Vector3 actualPlayerPos;
    public int difficulty;
    public List<int> objects;
    public float health;
    public bool[] triggerPassed;
    public bool logEventDone;

    bool playerExists, combatSceneManagerExists;

    public bool firstTime = true;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        triggerPassed = new bool[4];
    }

    private void Start()
    {

        LoadPlayer();
        SceneManager.sceneLoaded += OnEnterScene;
    }

    public void OnEnterScene(Scene nameScene, LoadSceneMode sceneMode)
    {
        playerExists = false;
        combatSceneManagerExists = false;

        switch(nameScene.name)
        {
            case "Lake":
                LoadPlayer();
                break;
            case "Combat":
                LoadSceneCombatManager();
                break;
        }
    }

    void RestartGame()
    {
        actualPlayerPos = new Vector3(62f, 6.5f, 0);
        health = 500;
        for (int i = 0; i < triggerPassed.Length; i++) { triggerPassed[i] = false; }
        logEventDone = false;
        difficulty = 0;
        playerExists = false;
        combatSceneManagerExists = false;
        firstTime = true;
    }

    void LoadPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

            if (firstTime)
            {
                RestartGame();
                firstTime = false;
            }
            else
            {
                player.SetHealth(health);
                player.UpdateHealthBar(health / player.GetMaxHealth());
                player.objectsList = objects;
                player.transform.position = actualPlayerPos;
            }

            playerExists = true;
        }
    }

    void LoadSceneCombatManager()
    {
        if (GameObject.FindGameObjectWithTag("CombatSceneManager") != null)
        {
            combatSceneManager = GameObject.FindGameObjectWithTag("CombatSceneManager").GetComponent<CombatSceneManager>();
            combatSceneManagerExists = true;
            combatSceneManager.difficultySelected = difficulty;
            combatSceneManager.currentHealth = health;
        }
    }

    public void OnLeaveScene()
    {
        if (playerExists)
        {
            actualPlayerPos = player.transform.position;
            health = player.GetHealth();
            objects = player.objectsList;
        }
        else if(combatSceneManagerExists)
        {
            health = combatSceneManager.currentHealth;
        }

        playerExists = false;
        combatSceneManagerExists = false;
    }

    public void DisableLogEvent() { logEventDone = true; }
}