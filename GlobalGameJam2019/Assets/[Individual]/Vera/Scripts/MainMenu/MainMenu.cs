using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private enum CurrentMenu { main,levelSelect}
    [Header("Current menu")]
    [SerializeField] private CurrentMenu currentMenu;

    [Header("Menu's")]
    [SerializeField] private Canvas mainMenu;

    [Header("Tect Components")]
    [SerializeField] private TMP_InputField nameInput;


    private void Start()
    {
        nameInput.text = HighscoreManager.instance?.LoadLastName();
    }

    public void MainPlayButton()
    {
        if(nameInput.text != "")
        {
            
            SetMenu(CurrentMenu.levelSelect);
            if(HighscoreManager.instance != null)
            {
                HighscoreManager.instance.currentName = nameInput.text;
                HighscoreManager.instance?.SaveLastName(nameInput.text);
            }
        }
    }


    private void SetMenu(CurrentMenu menu)
    {
        currentMenu = menu;
    }

    private void Update()
    {
        switch (currentMenu)
        {
            case CurrentMenu.main:
                mainMenu.enabled = true;
                break;
            case CurrentMenu.levelSelect:
                mainMenu.enabled = false;
                break;
        }
    }
}
