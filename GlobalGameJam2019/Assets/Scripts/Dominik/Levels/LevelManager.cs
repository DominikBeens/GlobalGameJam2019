using System.Collections.Generic;
using UnityEngine;
using DB.MenuPack;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    private static int currentLevel;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject levelCompleteCanvas;

    [Space]

    [SerializeField] private List<LevelData> levelData;
    [SerializeField] private GameObject selectedLevelPanel;
    [SerializeField] private TextMeshProUGUI selectedLevelText;
    [SerializeField] private Transform levelSelectButtonHolder;
    [SerializeField] private GameObject levelSelectionIndicator;

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
        levelSelectionIndicator.SetActive(false);
        levelCompleteCanvas.SetActive(false);
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
        selectedLevelPanel.SetActive(false);
        levelSelectionIndicator.SetActive(false);
        menuCanvas.SetActive(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0 ? true : false);
    }

    public void CollectPickup()
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            if (levelData[i].level == currentLevel)
            {
                levelData[i].pickupsCollected++;
                return;
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.OnLevelLoaded -= SceneManager_OnLevelLoaded;
    }
}
