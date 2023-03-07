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
    public Animator anim;
    /*
    //Grappling
    private LineRenderer lineRend;
    private DistanceJoint2D distJoint;
    private Node selectedNode;
    */
    // Start is called before the first frame update
    void Start()
    {
        myConstantForce = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
        scaleX = transform.localScale.x;
        /*
        //grapple inits
        lineRend = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        lineRend.enabled = false;
        distJoint.enabled = false;
        selectedNode = null; */
    }

    // Update is called once per frame
    void Update()
    {
        //NodeBehavior();
        //Player movement init
        horizontalMove = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //flip sprite and set anims
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if(Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            anim.SetTrigger("Idle");
        }

        //dash and jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isDashing)
        {
                rb.velocity = Vector2.up * jumpForce;
                isGrounded = false;
                anim.SetBool("isJumping", true);
        } 

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && isGrounded && (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0))
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

                anim.SetBool("isSlinging", false);
                anim.SetBool("isJumping", true);
                rb.velocity = new Vector2(currentSwingable.GetComponent<Rigidbody2D>().velocity.x, currentSwingable.GetComponent<Rigidbody2D>().velocity.y + 10);

            }
        } 
    }

    private void FixedUpdate()
    {
        //move left and right and flip sprite
        Move();
    }

    public void Move()
    {
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

    //Grounded checker
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }
    }

    //Dash routine
    private IEnumerator Dash()
    {
        isDashing = true;
        dashDirection = new Vector2(Mathf.Sign(horizontalMove), 0f);

        anim.SetBool("isSliding", true);
        yield return new WaitForSeconds(dashTime);
        anim.SetBool("isSliding", false);
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vine")
        {
            other.GetComponent<Rigidbody2D>().velocity = vineVelocityWhenGrabbed + rb.velocity;
            swinging = true;
            anim.SetBool("isSlinging", true);
            anim.SetBool("isJumping", false);
            currentSwingable = other.transform;
        }
    }
    /*
    //grappling codes
    public void SelectNode(Node node)
    {
        selectedNode = node;
    }

    public void DeselectNode()
    {
        selectedNode = null;
    }

    private void NodeBehavior()
    {
        if (selectedNode == null)
        {
            lineRend.enabled = false;
            distJoint.enabled = false;

            return;
        }

        lineRend.enabled = true;
        distJoint.enabled = true;

        distJoint.connectedBody = selectedNode.GetComponent<Rigidbody2D>();

        if(selectedNode != null)
        {
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, selectedNode.transform.position);
        }
    } */
}