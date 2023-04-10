using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float speed = 1f; // The speed at which the lava rises
    public float maxHeight = 10f; // The maximum height that the lava can reach
    public Transform player; // A reference to the player object
    private bool isRising = true; // Whether the lava is currently rising or not
    private Vector3 startingPosition; // The starting position of the lava

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isRising)
        {
            // Move the lava upward
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // Check if the lava has reached the maximum height
            if (transform.position.y >= startingPosition.y + maxHeight)
            {
                isRising = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Die();
                // Reset the lava to its starting position
                transform.position = startingPosition;
            }
        }
    }
}
