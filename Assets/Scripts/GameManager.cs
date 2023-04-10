using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int keysNeeded = 0; // number of keys needed to unlock the finish level
    public int keysPickedUp = 0; // number of keys picked up by the player
    private int lastKeysPickedUp;
    private AudioSource ass;
    private void Start()
    {
        lastKeysPickedUp = keysPickedUp;
        ass = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (lastKeysPickedUp != keysPickedUp)
        {
            ass.Play();
            lastKeysPickedUp = keysPickedUp;
        }
    }
}
