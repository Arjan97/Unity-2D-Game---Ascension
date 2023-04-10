using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    private AudioSource ass;
    public Sprite unlockedSprite; // Sprite to use when the finish level is unlocked
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    private void Start()
    {
        ass = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.keysPickedUp >= GameManager.Instance.keysNeeded)
            {
                // Change the sprite to the unlocked sprite
                spriteRenderer.sprite = unlockedSprite;
                //ass.Play();
                // Stop the timer and save the high score
                FindObjectOfType<ScoreTimer>().StopTimer();
                //load next scene
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
