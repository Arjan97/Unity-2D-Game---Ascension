using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class playerJetpack : MonoBehaviour
{
    //jet engine & fuel
    public float jetForce = 5f;
    public float jetCapacity = 100f;
    public float jetFuelRate = 30f;
    public float fuel;
    public bool canFly;

    //thrust
    public float fuelThrust = 20f;
    public float thrustForce = 1000f;
    public float thrustDelay = 3f;
    private bool canThrust = true;
    private bool isThrustingForward;
    private Vector2 thrustDirection;
    private float horizontalMove;

    //particle
    public ParticleSystem effect;

    //components
    Rigidbody2D rb;
    private playerMovement player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuel = jetCapacity;
        player = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canFly)
        {
            jetFly();
        }
    }

    private void jetFly()
    {
        jetThrust();
         if (Input.GetKey(KeyCode.Space) && fuel > 0f && !player.IsGrounded())
        {
            fuel -= jetFuelRate * Time.deltaTime; //makes fuel meter go brr

            rb.AddForce(Vector2.up * jetForce); //force added for jumping
            effect.Play();
        }
        else
        {
            effect.Stop();
        }
    }

    //adds fuel from the refueler script, with max
    public void AddFuel(float amount)
    {
        fuel = Mathf.Min(fuel + amount, jetCapacity);
    }

    private void jetThrust()
    {
        if (Input.GetKey(KeyCode.LeftShift) && fuel > 30 && canThrust)
        {
            print("thrust");

            horizontalMove = Input.GetAxisRaw("Horizontal");
            thrustDirection = new Vector2(Mathf.Sign(horizontalMove), 0f);

            rb.AddForce(thrustDirection * thrustForce);
            Debug.Log("thrusting to" + thrustDirection);
            fuel -= fuelThrust;
            StartCoroutine(thrustDelayTimer(thrustDelay));
        }
    }

    //delay timer for thrusting
    private IEnumerator thrustDelayTimer(float delay)
    {
        canThrust = false;
        yield return new WaitForSeconds(delay);
        canThrust = true;
    }

    //draw the fuel meter on screen
    void OnGUI()
    {
        if(canFly)
        {
            GUI.Box(new Rect(10, 10, 100, 20), "Fuel: " + fuel.ToString("F0"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jetpack"))
        {
            canFly = true;
            Destroy(collision.gameObject);
        }
    }
}
