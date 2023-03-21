using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3;
    public float currentHealth;
    public float invincibilityTime = 0.3f;
    private float previousHealth;

    public float knockbackForce = 15f;
    private Rigidbody2D rb;
    private playerMovement pmove;
    private SpriteRenderer spriteRenderer;

    public float regenDelay = 5f; // Delay before regenerating health
    public float regenRate = 0.1f; // Amount of health regenerated per second
    private float timeSinceLastDamage; // Time since the player last took damage

    public static Transform currentSpawnPoint;
    public static Transform currentCheckpoint;


    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        pmove = GetComponent<playerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousHealth = currentHealth;
        currentSpawnPoint = transform;
    }
    public static void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        currentSpawnPoint = checkpoint;
    }
    private void Update()
    {
        if (Time.time - timeSinceLastDamage >= regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Player takes damage
            StartCoroutine(TakeDamageCoroutine());
        }
        timeSinceLastDamage = Time.time;
    }
    private IEnumerator TakeDamageCoroutine()
    {
        // Make player invincible for a short period of time
        pmove.enabled = false;
        if (!pmove.isFacingRight)
        {
            rb.AddForce((Vector2.right + Vector2.up) * (knockbackForce + Mathf.Abs(rb.velocity.x)), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce((Vector2.left + Vector2.up) * (knockbackForce + Mathf.Abs(rb.velocity.x)), ForceMode2D.Impulse);
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(invincibilityTime);
        pmove.enabled = true;
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
        else if (currentSpawnPoint != null)
        {
            transform.position = currentSpawnPoint.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }

        // Reset health
        currentHealth = maxHealth;
    }
}
