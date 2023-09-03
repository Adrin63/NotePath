using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform[] buttonObjects;
    public Transform[] startPositionButtons;
    public Transform[] startPositionNotes;
    public Transform defenseNotesParent;
    public Transform combatManagerObject;

    [Header("Sprites & Sounds")]
    public Sprite[] defenseNoteSprites;
    public GameObject defenseNotePrefab;
    public AudioSource hitSound;

    List<GameObject> noteObjects;
    CombatManager combatManager;
    DefenseButton[] defenseButton;

    [Header("Parameters")]
    public float noteSpeed = 5f;
    public float defenseTime = 40f;
    public float cdBetweenNotes = 0.4f;
    public float twoNotesPercentage = 30f;
    public float threeNotesPercentage = 10f;
    [SerializeField] float damageReceivedMultiplier = 1f;
    float actualTime;
    float actualCDBetweenNotes;
    int notesSpawned = 0;
    bool onDefending = false;

    bool[] isButtonPressed;

    int totalDamage;
    

    void Start()
    {
        combatManager = combatManagerObject.GetComponent<CombatManager>();
        noteObjects = new List<GameObject>();
        defenseButton = new DefenseButton[buttonObjects.Length];
        isButtonPressed = new bool[buttonObjects.Length];
        

        for (int i = 0; i < defenseButton.Length; i++) { defenseButton[i] = buttonObjects[i].GetComponent<DefenseButton>(); }
    }

    void Update()
    {
        if (onDefending) OnDefense();
    }

    public void StartDefense()
    {
        ActivateAllObjects(); 

        for (int i = 0; i < buttonObjects.Length; i++) { buttonObjects[i].position = startPositionButtons[i].position; }
        for(int i = 0; i < defenseButton.Length; i++) { defenseButton[i].destroyNoteParticle.gameObject.SetActive(false); defenseButton[i].DisableButton(); }

        //Inicializar variables cantidad notas...
        //La idea es hacer 2-3 estandares musicales para 2/4 3/4 (velocidad musical), marcar una canción con un compás
        //para que las notas vayan a una cierta velocidad y tenga sentido con la canción, hasta entonces
        //las notas se generan aleatoriamente siguiendo los parametros

        //notesToSpawn
        //defenseTime
        //noteSpeed
        //cdBetweenNotes

        switch(combatManager.difficulty)
        {
            case 0:
                defenseTime = 10f;
                noteSpeed = 7f;
                cdBetweenNotes = 0.8f;
                twoNotesPercentage = 10f;
                threeNotesPercentage = 0f;
                damageReceivedMultiplier = 1f;
                break;
            case 1:
                defenseTime = 15f;
                noteSpeed = 10f;
                cdBetweenNotes = 0.5f;
                twoNotesPercentage = 15f;
                threeNotesPercentage = 10f;
                damageReceivedMultiplier = 1.25f;
                break;
            case 2:
                defenseTime = 17f;
                noteSpeed = 12f;
                cdBetweenNotes = 0.3f;
                twoNotesPercentage = 20f;
                threeNotesPercentage = 10f;
                damageReceivedMultiplier = 1.5f;
                break;
            case 3:
                defenseTime = 25f;
                noteSpeed = 15f;
                cdBetweenNotes = 0.4f;
                twoNotesPercentage = 25f;
                threeNotesPercentage = 15f;
                damageReceivedMultiplier = 2f;
                break;
        }

        actualCDBetweenNotes = 0;
        actualTime = 0f;
        notesSpawned = 0;

        totalDamage = 0;
        onDefending = true;
    }

    void OnDefense()
    {
        actualTime += Time.deltaTime;
        actualCDBetweenNotes -= Time.deltaTime;

        for (int i = 0; i < noteObjects.Count; i++)
        {
            Vector3 pos = noteObjects[i].gameObject.transform.position;
            pos.x -= noteSpeed * Time.deltaTime;
            noteObjects[i].gameObject.transform.position = pos;
        }

        if(actualCDBetweenNotes <= 0)
        {
            int generatedPercentage = Random.Range(0, 100);

            if (generatedPercentage < twoNotesPercentage)//2 NOTAS
            {
                generatedPercentage = 1;
            }
            else if(generatedPercentage < (threeNotesPercentage + twoNotesPercentage))//3 NOTAS
            {
                generatedPercentage = 2;
            }
            else//1 NOTA
            {
                generatedPercentage = 0;
            }

            int[] noRepeatRow = new int[generatedPercentage + 1];
            for(int i = 0; i < generatedPercentage + 1; i++)
            {
                int row = -1;

                for (int a = 0; a < noRepeatRow.Length; a++)
                {
                    row = Random.Range(0, 4);

                    if (row != noRepeatRow[a])
                        noRepeatRow[a] = row;
                    else
                    {
                        while (row == noRepeatRow[a])
                        {
                            row = Random.Range(0, 4);
                        }
                    }
                }

                GameObject newNote = GameObject.Instantiate(defenseNotePrefab, startPositionNotes[row]);
                SpriteRenderer newNoteSpriteRenderer = newNote.GetComponent<SpriteRenderer>();
                DefenseNote newNoteDefenseNoteScript = newNote.GetComponent<DefenseNote>();
                newNoteDefenseNoteScript.defenseManagerObject = transform;
                newNoteDefenseNoteScript.identifier = notesSpawned;
                newNoteDefenseNoteScript.row = row;
                //newNote.tag = defenseNotePrefab.tag;
                newNote.transform.parent = defenseNotesParent;
                newNote.name = "DefenseNote Identifier:" + newNoteDefenseNoteScript.identifier;
                newNoteSpriteRenderer.sprite = defenseNoteSprites[row];

                noteObjects.Add(newNote);

                notesSpawned++;
            }

            actualCDBetweenNotes = cdBetweenNotes;
        }

        if (actualTime >= defenseTime) { EndDefense(); }
    }

    void EndDefense()
    {
        Cursor.visible = true;
        onDefending = false;
        combatManager.MakeDamage(Mathf.RoundToInt((float)totalDamage * damageReceivedMultiplier), 0);
    }
    
    public void DisableAllObjects()
    {
        if (noteObjects != null)
        {
            for (int i = defenseNotesParent.childCount - 1; i >= 0; i--)
            {
                Destroy(defenseNotesParent.GetChild(i).gameObject);
            }

            noteObjects.Clear();
        }

        for (int i = 0; i < buttonObjects.Length; i++) { buttonObjects[i].gameObject.SetActive(false); }
    }

    public void ActivateAllObjects()
    {
        for (int i = 0; i < buttonObjects.Length; i++) { buttonObjects[i].gameObject.SetActive(true); }
    }

    public void ButtonPressed(int buttonNum, bool pressed)
    {
        isButtonPressed[buttonNum - 1] = pressed;
    }

    public void OnDestroyNote(int identifier, int row, bool failedNote)
    {
        hitSound.Play();

        if (failedNote)
            totalDamage += 5;

        if (row != -1)
            defenseButton[row].destroyNoteParticle.gameObject.SetActive(true);

        StartCoroutine(DestroyNote(identifier));
    }

    IEnumerator DestroyNote(int identifier)
    {
        yield return 0;

        for (int i = 0; i < defenseNotesParent.childCount; i++)
        {
            if (defenseNotesParent.GetChild(i).GetComponent<DefenseNote>().identifier == identifier)
            {
                noteObjects.Remove(defenseNotesParent.GetChild(i).gameObject);
                Destroy(defenseNotesParent.GetChild(i).gameObject);
                break;
            }
        }
    }
}