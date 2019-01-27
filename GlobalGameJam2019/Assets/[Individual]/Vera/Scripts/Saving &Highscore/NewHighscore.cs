using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewHighscore : MonoBehaviour
{
    public static NewHighscore instance;

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

    public int level;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartScore();
    }

    public void StartScore()
    {
        decrement = maxScore / timeForLevel;
        timeLeft = timeForLevel;
    }

    private void Update()
    {
        if(timeLeft >= 0)
        {
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        if(timeLeft == 0)
        {
            DeathPlacement.instance?.AddDeath(GameManager.instance.bird.transform.position);
            LevelManager.instance.ShowTimeUpPopup();
            return;
        }

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

    public int GetScore()
    {
        int score = Mathf.RoundToInt(timeLeft * decrement);
        score += pickupScore;
        return score;
    }

    public void Pickup(int score)
    {
        pickupScore += score;
    }

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
    public bool Finished()
    {
            Highscore newHighscore = new Highscore();
            newHighscore.score = GetScore();
            return HighscoreManager.instance.AddScore(newHighscore, HighscoreManager.instance.currentName, level - 1);
        
    }
}
