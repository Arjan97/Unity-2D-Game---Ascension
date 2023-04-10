using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        // Load the first level of the game
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        // Quit the game
        Application.Quit();
    }
}
