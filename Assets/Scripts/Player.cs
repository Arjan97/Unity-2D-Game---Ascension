using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 vineVelocityWhenGrabbed;
    Transform currentSwingable;
    ConstantForce2D myConstantForce;
    bool swinging = false;

    public float speed = 200f; //Controls velocity multiplier
    private Rigidbody2D rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        myConstantForce = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (swinging)
        {
            myConstantForce.enabled = false;
            transform.position = currentSwingable.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                swinging = false;
                rb.velocity = currentSwingable.GetComponent<Rigidbody2D>().velocity;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Vine")
        {
            other.GetComponent<Rigidbody2D>().velocity = vineVelocityWhenGrabbed;
            swinging = true;
            currentSwingable = other.transform;
        }
    }
}
