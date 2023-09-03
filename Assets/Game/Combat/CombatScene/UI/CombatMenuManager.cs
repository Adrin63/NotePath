using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenuManager : MonoBehaviour
{
    public Transform sceneManagerObject;
    public Transform defaultObject;
    public Transform itemSelectorObject;
    public Transform attackSelectorObject;
    public AudioSource buttonSound;

    CombatSceneManager sceneManager;

    public enum Menu//Desde Default se puede atacar, elegir ataque y elegir item
    {
        Default, AttackSelector, ItemSelector, Hide
    }

    Menu menu, nextMenu;

    private void Start()
    {
        sceneManager = sceneManagerObject.GetComponent<CombatSceneManager>();
        nextMenu = Menu.Default;
        menu = nextMenu;
        StartMenu();
    }

    private void Update()
    {
        if (menu != nextMenu)
        {
            ChangeUI(menu, nextMenu);
            menu = nextMenu;
        }
    }
    
    void ChangeUI(Menu enterState, Menu endState)
    {
        switch (enterState)
        {
            case Menu.Default:
                defaultObject.gameObject.SetActive(false);
                break;

            case Menu.ItemSelector:
                itemSelectorObject.gameObject.SetActive(false);
                break;

            case Menu.AttackSelector:
                attackSelectorObject.gameObject.SetActive(false);
                break;
        }

        switch (endState)
        {
            case Menu.Default:
                defaultObject.gameObject.SetActive(true);
                break;

            case Menu.ItemSelector:
                itemSelectorObject.gameObject.SetActive(true);
                break;

            case Menu.AttackSelector:
                attackSelectorObject.gameObject.SetActive(true);
                break;
            case Menu.Hide:
                defaultObject.gameObject.SetActive(false);
                itemSelectorObject.gameObject.SetActive(false);
                attackSelectorObject.gameObject.SetActive(false);
                break;
        }
    }

    void StartMenu()
    {
        attackSelectorObject.gameObject.SetActive(false);
        itemSelectorObject.gameObject.SetActive(false);
    }

    public void MenuToHide() { nextMenu = Menu.Hide; }
    public void MenuToAttackSelector() { nextMenu = Menu.AttackSelector; }
    public void MenuToItemSelector() { nextMenu = Menu.ItemSelector; }
    public void MenuToDefault() { nextMenu = Menu.Default; }

    public void StartAttack()
    {
        buttonSound.Play();
        sceneManager.OnStartAttack();
    }
}
