using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Instance Variables
    //Movement
    private Rigidbody2D rb; //Player Physics
    private BoxCollider2D coll; //Player Collider
    [SerializeField] private float defaultSpeed; //Speed of Horizontal Movement
    [SerializeField] private float jumpSpeed; //Speed of Jump
    [SerializeField] private float acceleration; //Acceleration speed of player
    [SerializeField] private float deceleration; //Deceleration speed of player
    [SerializeField] private float velPower; //Power of velocity, default is 1
    [SerializeField] private float frictionAmount; //Amount of friction, default is 0.2
    [SerializeField] private LayerMask jumpableGround; //Jumpable Layer
    private float dirX;//Direction of Player
    private bool canDoubleJump; //Can the player double jump

    //Animation
    private Animator anim; //Animator
    private SpriteRenderer spr; //Sprite Renderer
    private enum MovementState {idle, running, jump, fall} //0 for idle, 1 for running, etc.

    //Sound
    [SerializeField] private AudioSource jumpSound;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Initializes rb
        coll = GetComponent<BoxCollider2D>(); //Initializes coll
        anim = GetComponent<Animator>(); //Initializes anim
        spr = GetComponent<SpriteRenderer>(); //Initializes Sprite Renderer
    }

    // Update is called once per frame
    private void Update()
    {
        //Horizontal Movement
        dirX = Input.GetAxis("Horizontal");

        float targetSpeed = dirX * defaultSpeed;

        float speedDifference = targetSpeed - rb.velocity.x;

        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velPower) * Mathf.Sign(speedDifference);
        
        rb.AddForce(movement * Vector2.right);

        //Friction
        friction();

        //Jumping
        if (isGrounded()) {
            canDoubleJump = true;
            if (Input.GetButtonDown("Jump")) {
               rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
               jumpSound.Play(); //Play Sound
            }  
        } else {
            if (Input.GetButtonDown("Jump") && canDoubleJump) {
               rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
               jumpSound.Play(); //Play Sound
               canDoubleJump = false;
           }
        }

        

        //Animation
        animationUpdate(dirX);

    }

    //Animation Controller
    private void animationUpdate(float input) {
        MovementState state;

        //Horizontal Movement (Idle <-> Running)
        if (input > 0.01f) {
            state = MovementState.running;
            spr.flipX = false;
        } else if (input < -0.01f) {
            state = MovementState.running;
            spr.flipX = true;
        } else {
            state = MovementState.idle;
        }

        //Verticle Movement (Idle/Running <-> Jumping/Falling)
        if (rb.velocity.y > 0.1f) {
            state = MovementState.jump;
        } else if (rb.velocity.y < -0.1f) {
            state = MovementState.fall;
        }

        //Sets Animation State Value
        anim.SetInteger("State", (int) state);
    }

    //Creates a new collider based on the player's collider but shifted down
    //The function then checks if the new collider overlaps with the collider of the ground
    //Afterwards it returns true or false
    private bool isGrounded() {
        //Arguments: Center of Box, Size of Box, Angle of Box (float),
        //Direction of Box Shift, Distance of the Box Shift, Detects Collision With Layer
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.01f, jumpableGround);
    }

    private void friction() {
        if (isGrounded() && Mathf.Abs(dirX) < 0.01f) {
            float amountOfFriction = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amountOfFriction *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amountOfFriction, ForceMode2D.Impulse);
        }
    }
}