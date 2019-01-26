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
    [SerializeField] private TMP_Text secondText;
    [SerializeField] private TMP_Text minuteText;
    [SerializeField] private TMP_InputField nameInput;
    private int pickupScore;

    [SerializeField] private int level;

    private void Start()
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

    public void Finished()
    {
        if(nameInput.text != "")
        {
            Highscore newHighscore = new Highscore();
            newHighscore.score = GetScore();
            HighscoreManager.instance.AddScore(newHighscore, nameInput.text,level -1);
        }
    }
}
