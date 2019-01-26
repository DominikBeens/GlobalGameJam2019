using DB.MenuPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    private static int currentLevel;

    private List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();

    [SerializeField] private GameObject menuCanvas;

    [Space]

    [SerializeField] private List<LevelData> levelData;

    [SerializeField] private GameObject selectedLevelPanel;
    [SerializeField] private TextMeshProUGUI selectedLevelText;
    [SerializeField] private Transform levelSelectButtonHolder;
    [SerializeField] private GameObject levelSelectionIndicator;

    [Header("Level Popup")]
    [SerializeField] private GameObject levelPopupCanvas;
    [SerializeField] private TextMeshProUGUI levelPopupHeaderText;
    [SerializeField] private TextMeshProUGUI levelPopupScoreText;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        selectedLevelPanel.SetActive(false);
        foreach (Transform child in levelSelectButtonHolder)
        {
            levelSelectButtons.Add(child.GetComponent<LevelSelectButton>());
        }
        levelSelectionIndicator.SetActive(false);

        HidePopupPanel();

        SceneManager.OnLevelLoaded += SceneManager_OnLevelLoaded;
        SceneManager_OnLevelLoaded();
    }

    public LevelData GetLevelData(int level)
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].level == level)
            {
                return levelData[i];
            }
        }

        Debug.LogError($"Couldn't retreive level data for level {level}");
        return null;
    }

    public void ToggleLevelLock(int level, bool lockState)
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].level == level)
            {
                levelData[i].locked = lockState;
                return;
            }
        }
    }

    public void SelectLevelToPlay(int level)
    {
        currentLevel = level;

        selectedLevelPanel.SetActive(false);
        selectedLevelText.text = $"Selected level: {currentLevel}";
        selectedLevelPanel.SetActive(true);

        levelSelectionIndicator.SetActive(false);
        levelSelectionIndicator.transform.position = levelSelectButtonHolder.GetChild(level - 1).transform.position;
        levelSelectionIndicator.SetActive(true);
    }

    public void LoadLevel()
    {
        selectedLevelPanel.SetActive(false);
        SceneManager.instance.LoadScene(currentLevel, false);
    }

    private void SceneManager_OnLevelLoaded()
    {
        Time.timeScale = 1;
        selectedLevelPanel.SetActive(false);
        levelSelectionIndicator.SetActive(false);
        menuCanvas.SetActive(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0 ? true : false);
        HidePopupPanel();
        SetLevelAccesibility();
    }

    public void SetLevelAccesibility()
    {
        if (!HighscoreManager.instance)
        {
            return;
        }

        int highestLevel = HighscoreManager.instance.LastLevel();
        if (highestLevel < 0)
        {
            return;
        }

        for (int i = 0; i < levelData.Count; i++)
        {
            levelData[i].locked = levelData[i].level <= highestLevel ? false : true;
            levelSelectButtons[i].UpdateLock(levelData[i]);
        }
    }

    public void CollectPickup()
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].level == currentLevel)
            {
                levelData[i].AddPickup();
                return;
            }
        }
    }

    public void ResetPickups()
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].level == currentLevel)
            {
                levelData[i].ResetPickups();
                return;
            }
        }
    }

    public void RestartLevel()
    {
        ResetPickups();
        SceneManager.instance.LoadScene(currentLevel, false);
    }

    public void ReturnToMainMenu()
    {
        ResetPickups();
        SceneManager.instance.LoadScene(0, false);
    }

    public void ShowLevelCompletePopup(int score, bool newHighscore)
    {
        Time.timeScale = 0;
        levelPopupHeaderText.text = "Level Complete!";
        levelPopupScoreText.text = newHighscore ? $"Score: {score} \nNew highscore!" : $"Score: {score}";
        levelPopupCanvas.SetActive(true);
    }

    public void ShowTimeUpPopup()
    {
        Time.timeScale = 0;
        levelPopupHeaderText.text = "Time is up!";
        levelPopupScoreText.text = "";
        levelPopupCanvas.SetActive(true);
    }

    private void HidePopupPanel()
    {
        levelPopupCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        SceneManager.OnLevelLoaded -= SceneManager_OnLevelLoaded;
    }
}
