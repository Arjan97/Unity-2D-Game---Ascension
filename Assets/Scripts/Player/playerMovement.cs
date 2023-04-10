using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //movementbasics
    public float speed = 10f;
    public bool isFacingRight = true;
    private float horizontal;
    private bool moveable = true;

    //wall slide
    public Transform wallCheck;
    public float wallCheckDistance;
    private bool isWallSliding;
    public float wallSlideSpeed;
    private bool canWallSlide = true;
    public float wallSlideDuration = 8f;
    //wall jump
    public float wallJumpDuration;
    bool canWallJump = true;
    private bool hasWallJumped = false;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private LayerMask wallLayerMask;
    private Coroutine wallJumpCoroutine;
    private float lastWallJump;

    //jump
    private bool isJumping;
    public float jumpingPower = 16f;
    //private bool doubleJump;
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

    //anims/effects
    public Animator anim;
    public ParticleSystem dust;

    void Start()
    {
        myConstantForce = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (moveable)
        {
            Flip();
            Jumps();
        }

        CheckIfWallSliding();
        CheckIfMoveable();
    }
    //fixed is called twice per frame
    private void FixedUpdate()
    {
        if (moveable)
        {
            ApplyMovement();
        }
    }
    private void CheckIfMoveable()
    {
        if (IsGrounded() || IsTouchingWall() && canWallSlide)
        {
            moveable = true;
            canWallSlide = true;
        }
    }
    private void Jumps()
    {
        if (isWallSliding && canWallJump && IsTouchingWall())
        {
            anim.SetBool("isJumping", false);
            WallJump();
        } else if (IsGrounded())
        {
            canWallJump= true;
            Jump();
        }
    }

    private void Jump()
    {
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            coyoteTimeCounter = coyoteTime;
            //doubleJump= false;
            anim.SetBool("isJumping", false);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;

            anim.SetBool("isJumping", true);
            /*if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }*/
            dust.Play();
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
            anim.SetBool("isJumping", true);
        }
    }

    private void WallJump()
    {
        //wallslide jump
        if (Input.GetButton("Jump") && !IsGrounded())
        {
            if (wallJumpCoroutine != null)
            {
                StopCoroutine(wallJumpCoroutine);
            }
            //StartCoroutine(DisableMovement(1f));
            Vector2 direction = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
            rb.AddForce(direction, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

            //hasWallJumped = true;
            //StartCoroutine(WallJumpCooldown());
            wallJumpCoroutine = StartCoroutine(WallJumpCooldown());
            lastWallJump = gameObject.transform.position.x;

        }
    }

    private void ApplyMovement()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
            anim.SetBool("isJumping", false);
            hasWallJumped = false;
            canWallSlide = true;
            lastWallJump = 0;
        }
        else if(!IsGrounded() && !isWallSliding && horizontal != 0){
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            dust.Stop();
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            //canWallSlide = false;
            dust.Stop();
        }


        if (isWallSliding && canWallSlide)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                StartCoroutine(WallSlideDuration());
                dust.Play();
            }
        }
    }

    private void CheckIfWallSliding()
    {
        if(IsTouchingWall() && !IsGrounded() && rb.velocity.y < 0) 
        {
            isWallSliding = true;

            anim.SetBool("isSliding", true);
        }
        else if (!canWallSlide)
        {
            anim.SetBool("isSliding", false);
        } else
        {
            isWallSliding = false;
            anim.SetBool("isSliding", false);
        }
    }
    private IEnumerator WallJumpCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        hasWallJumped = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            facingDirection = facingDirection * -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            dust.Play();
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsTouchingWall()
    {

        return Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayerMask);
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    IEnumerator DisableMovement(float time)
    {
        moveable = false;
        yield return new WaitForSeconds(time);
        moveable= true;
    }
    private IEnumerator WallSlideDuration()
    {
         canWallSlide = true;
        Debug.Log("wallslide cooling smeh");
        yield return new WaitForSeconds(wallSlideDuration);
        //canWallSlide = false;
        Debug.Log("wallslide cooling cannot");
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
