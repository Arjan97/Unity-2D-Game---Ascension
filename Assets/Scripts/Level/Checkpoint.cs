using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActivated = false; // is this checkpoint activated?
    public Sprite activatedSprite; // sprite for activated checkpoint
    public Sprite deactivatedSprite;
    private SpriteRenderer spriteRenderer; // reference to the sprite renderer component

    private AudioSource ass;

    private void Start()
    {
        ass = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // get the sprite renderer component
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetCheckpoint(); // set the checkpoint
            ass.Play();
            //activated = true;
           // PlayerHealth.SetCheckpoint(transform);
        }
    }

    private void SetCheckpoint()
    {
        // deactivate all other checkpoints
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint != this)
            {
                checkpoint.isActivated = false;
                checkpoint.spriteRenderer.sprite = checkpoint.spriteRenderer.sprite = checkpoint.deactivatedSprite;
            }
        }

        // set this checkpoint as activated
        isActivated = true;
        spriteRenderer.sprite = activatedSprite;
        PlayerHealth.SetCheckpoint(transform);
        //SpawnPoint.currentSpawnPoint = transform; // set the current spawn point to this checkpoint
    }
}
