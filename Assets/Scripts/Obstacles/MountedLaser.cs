using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MountedLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    public float shootingInterval = 1f;
    public float shootingRange = 10f;
    public float damage = 0.5f;
    public float laserSpeed = 15f;
    private float timeSinceLastShot = 0f;
    private Transform player;

    void Start()
    {
        timeSinceLastShot = shootingInterval;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < shootingRange)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= shootingInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
    }

    void Shoot()
    {
        // Instantiate a laser beam prefab and set its position and rotation
        GameObject laserBeam = Instantiate(laserPrefab) as GameObject;
        laserBeam.transform.position = transform.position;

        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Rotate the laser beam towards the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserBeam.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Get the Rigidbody2D component of the laser beam
        Rigidbody2D rb = laserBeam.GetComponent<Rigidbody2D>();

        // Set the velocity of the laser beam to move towards the player
        rb.velocity = direction * laserSpeed;

        // Play laser sound effect
        laserBeam.GetComponent<AudioSource>().Play();

        // Destroy the laser beam after 5 seconds
        Destroy(laserBeam, 5f);
    }
}
