using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DB.MenuPack;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private enum CurrentMenu { main,levelSelect, leaderboard}
    [Header("Current menu")]
    [SerializeField] private CurrentMenu currentMenu;

    [Header("Menu's")]
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject leaderBoard;

    [Header("Tect Components")]
    [SerializeField] private TMP_InputField nameInput;


    private void Start()
    {
        nameInput.text = HighscoreManager.instance?.LoadLastName();
    }

    public void ResetSaves()
    {
        HighscoreManager.instance.ResetSaves();
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
                print(HighscoreManager.instance.LastLevel());
                LevelManager.instance?.SetLevelAccesibility();
            }
        }
    }


    private void SetMenu(CurrentMenu menu)
    {
        currentMenu = menu;
    }

    public void GoLeader()
    {
        SetMenu(CurrentMenu.leaderboard);
        leaderBoard.GetComponent<LeaderBoard>().SetLeaderBoards();
    }

    public void GoHome()
    {
        SetMenu(CurrentMenu.main);
    }

    public void ExitGame()
    {
        SceneManager.instance.QuitGame();        
    }

    private void Update()
    {
        switch (currentMenu)
        {
            case CurrentMenu.main:
                mainMenu.enabled = true;
                main.SetActive(true);
                leaderBoard.SetActive(false);
                break;
            case CurrentMenu.levelSelect:
                mainMenu.enabled = false;
                break;
            case CurrentMenu.leaderboard:
                mainMenu.enabled = true;
                main.SetActive(false);
                leaderBoard.SetActive(true);
                break;

        }
    }
}
