using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    //vine swing
    public Vector2 vineVelocityWhenGrabbed;
    Transform currentSwingable;
    ConstantForce2D myConstantForce;
    bool swinging = false;
    float swingForce = 10f;
    //player movement
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashDistance = 5f;
    public float dashTime = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isDashing = false;
    private float horizontalMove;
    private Vector2 dashDirection;
    private float scaleX;

    //animations
    //public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        myConstantForce = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            if(Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.Play("Walk");
            }
            else
            {
                anim.Play("WalkBack");
            }
        }
        else
        {
            anim.Play("Idle");
        } */

        //Player movement init
        horizontalMove = Input.GetAxisRaw("Horizontal");
        //dash and jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
                rb.velocity = Vector2.up * jumpForce;
                isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
        
        //player swinging
        if (swinging)
        {
            myConstantForce.enabled = false;
            transform.position = currentSwingable.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                swinging = false;
                rb.velocity = new Vector2(currentSwingable.GetComponent<Rigidbody2D>().velocity.x, currentSwingable.GetComponent<Rigidbody2D>().velocity.y + swingForce);
            }
        } 
        /*
        if (swinging)
        {
            // Set the player's position to the swingable's position and orientation
            transform.position = currentSwingable.position;
            transform.rotation = currentSwingable.rotation;

            // Release the player from the vine if they press the jump button
            if (Input.GetButtonDown("Jump"))
            {
                swinging = false;
                rb.velocity = currentSwingable.right * swingForce;
            }
        } */
    }

    private void FixedUpdate()
    {
        //move left and right and flip sprite
        Move();
    }

    public void Move()
    {
        Flip();
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);

        //dash
        Vector2 movement = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);

        if (!isDashing)
        {
            rb.velocity = movement;
        }
        else
        {
            rb.velocity = dashDirection * (dashDistance / dashTime);
        }
    }

    public void Flip()
    {
        if (horizontalMove > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        if (horizontalMove < 0)
        {
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    //Grounded checker
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //Dash routine
    private IEnumerator Dash()
    {
        isDashing = true;
        dashDirection = new Vector2(Mathf.Sign(horizontalMove), 0f);

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vine")
        {
            other.GetComponent<Rigidbody2D>().velocity = vineVelocityWhenGrabbed;
            swinging = true;
            currentSwingable = other.transform;
        }
    }
}
