using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHighscore : MonoBehaviour
{
    [SerializeField] private float timeForLevel;
    [SerializeField] private float maxScore;
    [SerializeField] private float decrement;


    [SerializeField] private float timeLeft;
    [SerializeField] private float time;
    [SerializeField] private int seconds;
    [SerializeField] private int minutes;


    [KeyCommand(KeyCode.V, PressType.KeyPressType.Down)]
    private void SS()
    {
        StartScore(300, 5000);
    }

    private void StartScore(float _timeForLevel, float _maxScore)
    {
        timeForLevel = _timeForLevel;
        maxScore = _maxScore;
        decrement = maxScore / timeForLevel;
        timeLeft = timeForLevel;
    }

    private void Update()
    {
        if(timeLeft > 0)
        {
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
        if(time > 1)
        {
            timeLeft--;
            time--;
        }
        SecondMinute();
    }

    private void SecondMinute()
    {
        minutes =(int) Mathf.Floor(timeLeft / 60);
        seconds = (int)timeLeft % 60;
    }


    private int GetScore()
    {
        int score = Mathf.RoundToInt(timeLeft * decrement);
        return score;
    }

    [KeyCommand(KeyCode.S, PressType.KeyPressType.Down)]
    private void Finished()
    {
        Highscore newHighscore = new Highscore();
        newHighscore.score = GetScore();
        HighscoreManager.instance.Save(newHighscore);
    }
}
