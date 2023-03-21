using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public float invincibilityTime = 0.3f;

    public float knockbackForce = 15f;
    private Rigidbody2D rb;
    private playerMovement pmove;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        pmove = GetComponent<playerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
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
        // Code to handle player death
        Debug.Log("Player has died.");
    }
}
