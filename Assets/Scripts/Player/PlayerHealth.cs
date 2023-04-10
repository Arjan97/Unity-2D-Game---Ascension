using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3;
    public float currentHealth;
    public float invincibilityTime = 0.5f;
    private float previousHealth;

    public float knockbackForce = 15f;
    private Rigidbody2D rb;
    private playerMovement pmove;
    private SpriteRenderer spriteRenderer;

    public float regenDelay = 5f; // Delay before regenerating health
    public float regenRate = 0.1f; // Amount of health regenerated per second
    private float timeSinceLastDamage; // Time since the player last took damage
    private bool isInvincible = false; // Whether the player is currently invincible

    public static Transform currentSpawnPoint;
    public static Transform currentCheckpoint;
    private AudioSource ass;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        pmove = GetComponent<playerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousHealth = currentHealth;
        currentSpawnPoint = transform;
        ass = GetComponent<AudioSource>();
    }
    public static void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        currentSpawnPoint = checkpoint;
    }
    private void Update()
    {
        /*
        if (Time.time - timeSinceLastDamage >= regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        } */
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isInvincible)
        {
            currentHealth -= damageAmount;
            Debug.Log("Player took " + damageAmount + " damage.");
            ass.Play();

            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (currentHealth % 0.5f != 0)
            {
                StartCoroutine(TakeDamageCoroutine());
            }
            else
            {
                Die();
            }

            timeSinceLastDamage = Time.time;
        }
       
    }
    private IEnumerator TakeDamageCoroutine()
    {
        // Make player invincible for a short period of time
        isInvincible = true;
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
        yield return new WaitForSeconds(0.2f);
        pmove.enabled = true;
        yield return new WaitForSeconds(invincibilityTime);
        spriteRenderer.color = Color.white;
        isInvincible = false;

    }

    public void Die()
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
       // currentHealth = maxHealth;
    }
}
