using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.keysPickedUp++; // increment the number of keys picked up
            Destroy(gameObject); // destroy the key object
        }
    }
}
