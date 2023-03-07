using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reFueler : MonoBehaviour
{
    public float fuelAmount = 100f; // Amount of fuel to add to the jetpack when picked up
    //public AudioClip pickupSound; // Sound to play when the fuel item is picked up

    // Function called when the player collides with the fuel item
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is the player
        playerJetpack jetpack = other.GetComponent<playerJetpack>();
        if (jetpack != null && jetpack.canFly)
        {
            // Add fuel to the jetpack and play the pickup sound
            jetpack.AddFuel(fuelAmount);
           // AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Destroy the fuel item
            Destroy(gameObject);
        }
    }
}
