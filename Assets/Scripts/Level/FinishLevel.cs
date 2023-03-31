using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Stop the timer and save the high score
            FindObjectOfType<ScoreTimer>().StopTimer();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
