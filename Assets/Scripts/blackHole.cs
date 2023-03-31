using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackHole : MonoBehaviour
{
    public float force = 10f; // The force of the black hole's gravity
    public float distanceThreshold = 5f; // The distance at which the player will start being pulled in
    public float velocityThreshold = 10f; // The velocity at which the player will be pulled in
    private Rigidbody2D playerRb; // The player's rigidbody component

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); // Find the player's rigidbody component
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // If the object is the player
        {
            Vector2 forceDirection = (Vector2)transform.position - (Vector2)other.transform.position; // Get the direction of the force
            float distance = Vector2.Distance(transform.position, other.transform.position); // Get the distance to the black hole
            float velocity = playerRb.velocity.magnitude; // Get the player's velocity
            Debug.Log("player detected");
            if (distance <= distanceThreshold && velocity >= velocityThreshold) // If the player is close enough and moving fast enough
            {
                Debug.Log("adding force");
                playerRb.AddForce(forceDirection.normalized * force * Time.deltaTime); // Apply a force towards the black hole
            }
        }
    }
}
