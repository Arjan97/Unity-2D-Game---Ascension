using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load

    public int keysNeeded = 1; // number of keys needed to unlock the finish level
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.keysPickedUp >= keysNeeded)
            {
                // load the next scene
                //FindObjectOfType<FinishLevel>().LoadNextScene();

                FindObjectOfType<ScoreTimer>().StopTimer();
                SceneManager.LoadScene(nextSceneName);
            }
            // Stop the timer and save the high score
        }
    }
}
