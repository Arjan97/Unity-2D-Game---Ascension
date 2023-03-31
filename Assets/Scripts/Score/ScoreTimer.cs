using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component that displays the timer
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component that displays the high score
    private int time = 0;

    public string highScoreKey; // Key to save the high score in PlayerPrefs

    private float startTime; // Time when the level started
    private float elapsedTime; // Time elapsed since the level started
    private bool isTimerRunning; // Flag to check if the timer is currently running
    private string highScore; // Local high score for the level

    private void Start()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScoreText.text = PlayerPrefs.GetInt(highScoreKey).ToString();
        } else
        {
            highScoreText.text = " ";
        }

        // Start the timer
        StartTimer();
    }

    private void Update()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey).ToString();
        Debug.Log("Elapsed time: " + time + ", High score: " + highScore);
    }

    public void StartTimer()
    {
        time = 0;
        InvokeRepeating("IncrimentTime", 1, 1);
    }
    void IncrimentTime()
    {
        time += 1;
        timerText.text = "Time: " + time;
    }
    public void SetHighscore()
    {
        PlayerPrefs.SetInt(highScoreKey, time);
        highScoreText.text = PlayerPrefs.GetInt(highScoreKey).ToString();
    }

    public void StopTimer()
    {
        CancelInvoke();
        if (PlayerPrefs.GetInt(highScoreKey) <= 0)
        {
            SetHighscore();
        }
        if (PlayerPrefs.GetInt(highScoreKey) > time)
        {
            SetHighscore();
        } 
    }
    /*
    void OnGUI()
    {
        //Delete all of the PlayerPrefs settings by pressing this button.
        if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
        {
            PlayerPrefs.DeleteAll();
        }
    } */
}
