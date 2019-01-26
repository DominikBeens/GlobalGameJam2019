using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<LevelLeaderBoard> levels = new List<LevelLeaderBoard>();
    [SerializeField] private List<TMP_Text> namesText = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> scoreText = new List<TMP_Text>();
    [SerializeField] private TMP_Text currentText;

    [SerializeField] private int current;

    public void SetLeaderBoards()
    {
        for (int i = 0; i < HighscoreManager.instance.amountLevel; i++)
        {
            if (i >= levels.Count)
            {
                levels.Add(new LevelLeaderBoard());
            }
            levels[i].Sort(HighscoreManager.instance.allHighscore[i].scores, HighscoreManager.instance.allHighscore[i].names);
        }
        current = 0;
        NewList();
    }  


    private void NewList()
    {
        currentText.text = "Level: " + (current + 1).ToString();
        if(current < levels.Count)
        {
            for (int i = 0; i < levels[current].score.Count; i++)
            {
                namesText[i].text = i.ToString()+ ": " + levels[current].playerName[i];
                scoreText[i].text = levels[current].score[i].ToString();
            }
        }
    }

    public void Last()
    {
        current--;
        if (current < 0)
        {
            current = levels.Count - 1;
        }
        NewList();
    }

    public void Next()
    {
        current++;
        if(current == levels.Count)
        {
            current = 0;
        }
        NewList();
    }
}
