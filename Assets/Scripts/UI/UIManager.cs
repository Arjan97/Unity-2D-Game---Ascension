using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
    public Image[] hearts;

    public Sprite emptyKey;
    public Sprite fullKey;
    public Image[] keys;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the heart sprites
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = fullHeartSprite;
        }
        for (int i = 0; i < GameManager.Instance.keysNeeded; i++)
        {
            keys[i].sprite = fullKey;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the heart sprites
        for (int i = 0; i < hearts.Length; i++)
        {
            if (playerHealth.currentHealth >= i + 1)
            {
                // Full heart
                hearts[i].sprite = fullHeartSprite;
            }
            else if (playerHealth.currentHealth >= i + 0.5f)
            {
                // Half heart
                hearts[i].sprite = halfHeartSprite;
            }
            else
            {
                // Empty heart
                hearts[i].sprite = emptyHeartSprite;
            }
        }
        for (int i = 0; i < GameManager.Instance.keysNeeded; i++)
        {
            keys[i].sprite = emptyKey;
            if (GameManager.Instance.keysPickedUp >= i + 1)
            {
                keys[i].sprite = fullKey;
                //keys[i].gameObject.SetActive(true);

            }
            else
            {
                keys[i].sprite = emptyKey;
               // keys[i].gameObject.SetActive(false);

            }
        }

    }
}
