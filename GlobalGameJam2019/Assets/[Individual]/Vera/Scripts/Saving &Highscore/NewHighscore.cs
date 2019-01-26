using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewHighscore : MonoBehaviour
{
    [SerializeField] private float timeForLevel;
    [SerializeField] private float maxScore;
    [SerializeField] private float decrement;


    [SerializeField] private float timeLeft;
    [SerializeField] private float time;
    [SerializeField] private int seconds;
    [SerializeField] private int minutes;

    [Header("TextComponents")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    private int pickupScore;

    [Header("Timer")]
    [SerializeField] private Image circle;
    [SerializeField] private GameObject feather;

    [SerializeField] private int level;

    private void Start()
    {
        StartScore(70, 5000);
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
        LevelTimer();
        scoreText.text = "Score: " + GetScore().ToString();
    }

    private void SecondMinute()
    {
        minutes =(int) Mathf.Floor(timeLeft / 60);
        seconds = (int)timeLeft % 60;
        if(minutes == 0)
        {
            timeText.text = "Seconds: " + seconds.ToString();
        }
        else if (seconds == 0)
        {
            timeText.text = "Minutes: " + minutes.ToString();
        }
        else
        {
            timeText.text = "Minutes: " + minutes.ToString() + " Seconds: " + seconds.ToString();
        }
        
    }

    private int GetScore()
    {
        int score = Mathf.RoundToInt(timeLeft * decrement);
        score += pickupScore;
        return score;
    }

    private void Pickup(int score)
    {
        pickupScore += score;
    }

    [KeyCommand(KeyCode.C, PressType.KeyPressType.Down)]
    private void Coin()
    {
        Pickup(50);
    }

    private void LevelTimer()
    {
        float timePercentage = timeLeft / timeForLevel;
        circle.fillAmount = timePercentage;
        float rot = 360 * timePercentage;
        feather.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, rot);
    }

    [KeyCommand(KeyCode.M, PressType.KeyPressType.Down)]
    public void Finished()
    {
            Highscore newHighscore = new Highscore();
            newHighscore.score = GetScore();
            HighscoreManager.instance.AddScore(newHighscore, HighscoreManager.instance.currentName,level -1);
        
    }
}
