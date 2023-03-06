using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //movementbasics
    public float speed = 10f;
    private bool isFacingRight = true;
    private float horizontal;

    //wall slide
    private bool isTouchingWall;
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isWallSliding;
    public float wallSlideSpeed;

    //jump
    private bool isJumping;
    public float jumpingPower = 16f;
    private bool doubleJump;
    //able to jump after not being grounded for a slight delay
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    //able to jump slightly above ground
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    //groundcheck
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //components
    private ConstantForce2D myConstantForce;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        myConstantForce = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            coyoteTimeCounter = coyoteTime;
            doubleJump= false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }

        //jump hold
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
        CheckIfWallSliding();
        Flip();
    }
    //fixed is called twice per frame
    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        } else if(!IsGrounded() && !isWallSliding && horizontal != 0){
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                Debug.Log("aids muur kk");
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void CheckIfWallSliding()
    {
        if(IsTouchingWall() && !IsGrounded() && rb.velocity.y < 0) 
        {
            isWallSliding = true;
            Debug.Log("kk muur aan");
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsTouchingWall()
    {

        return Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
