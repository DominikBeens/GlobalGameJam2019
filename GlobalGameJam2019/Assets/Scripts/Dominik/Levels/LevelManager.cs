using System.Collections.Generic;
using UnityEngine;
using DB.MenuPack;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    private int selectedLevel;

    [SerializeField] private List<LevelData> levelData;
    [SerializeField] private GameObject selectedLevelPanel;
    [SerializeField] private TextMeshProUGUI selectedLevelText;

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
        }

        selectedLevelPanel.SetActive(false);
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
            }
        }
    }

    public void SelectLevelToPlay(int level)
    {
        selectedLevel = level;
        selectedLevelText.text = $"Selected level: {selectedLevel}";
        selectedLevelPanel.SetActive(true);
    }

    public void LoadLevel()
    {
        selectedLevelPanel.SetActive(false);
        SceneManager.instance.LoadScene(selectedLevel, false);
    }
}
