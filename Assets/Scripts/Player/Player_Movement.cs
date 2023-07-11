using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //Instance Variables
    //Movement
    [SerializeField] private float defaultSpeed; //Speed of Horizontal Movement
    [SerializeField] private float jumpSpeed; //Normal speed of jump
    [SerializeField] private float jumpCutMultiplier; //How much speed the player loses if let go of jump
    [SerializeField] private float acceleration; //Acceleration speed of player
    [SerializeField] private float deceleration; //Deceleration speed of player
    [SerializeField] private float velPower; //Power of velocity, default is 1
    [SerializeField] private float maxCoyoteTime; //Time you get to jump while not on platforms
    [SerializeField] private float fallGravityMultiplier; //Modifies how fast you fall
    [SerializeField] private float hangGravityMultiplier; //Modifies how long you hang in air
    [SerializeField] private float hangVelocityActivate; //Velocity required to activate hang time
    [SerializeField] private float maxFallSpeed; //Max fall speed
    [SerializeField] private LayerMask jumpableGround; //Jumpable Layer
    private Rigidbody2D rb; //Player Physics
    private BoxCollider2D coll; //Player Collider
    private bool isJumping; //Is the player in a jump
    private float coyoteTime; //The actual coyote timer that ticks down
    private float gravityScale; //Modifies gravity while falling
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
        coyoteTime = maxCoyoteTime; //Sets the timer to max
        gravityScale = rb.gravityScale; //Gets player rigidbody gravity
    }

    // Update is called once per frame
    private void Update()
    {
        //Horizontal Movement
        //Gets player direction
        dirX = Input.GetAxis("Horizontal");

        //Speed the player is trying to reach
        float targetSpeed = dirX * defaultSpeed;

        //The difference between target speed and actual speed
        float speedDifference = targetSpeed - rb.velocity.x;

        //If the target speed exists, accelerate towards it. Otherwise decelerate.
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        //Multiply the difference in speed by the rate of acceleration
        //Sign function determines which direction it should apply it in
        float movement =
        Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velPower) * Mathf.Sign(speedDifference);

        //Applies force to player
        rb.AddForce(movement * Vector2.right);

        //Jumping
        //If grounded, reset coyote time and double jump
        //Else, it checks the coyote time first to see if the player can do normal jump
        //If not, the player uses double jump
        if (isGrounded()) {
            coyoteTime = maxCoyoteTime; //Resets coyote time
            canDoubleJump = true; //Resets double jump
            if (Input.GetButtonDown("Jump")) {
                jump();
            }
        } else {
            coyoteTime -= Time.deltaTime; //Coyote time ticks down
            if (Input.GetButtonDown("Jump")) {
                //If coyote time hasn't passed and has not jumped, do normal jump
                if (coyoteTime > 0 && !isJumping) {
                    jump();
                } else if (canDoubleJump) { //If coyote time passed, do double jump
                    jump();
                    canDoubleJump = false;
                }
            }
        }

        //If jump is not held down and the player is grounded, reset isJumping
        if(!Input.GetButton("Jump")) {
            if (isGrounded()) {
                isJumping = false; //Resets jump status
            }
        }

        //If the player lets go of jump and is moving upwards, cut the jump.
        if(Input.GetButtonUp("Jump") && isJumping && rb.velocity.y > 0.01f) {
            //Push player downwards by a fraction of their current vertical velocity
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
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
        if (rb.velocity.y > 0.01f) {
            state = MovementState.jump;
        } else if (rb.velocity.y < -0.01f) {
            state = MovementState.fall;
        }

        //Controls gravity of jumping
        jumpGravity();

        //Sets Animation State Value
        anim.SetInteger("State", (int) state);
    }

    //Jumping function
    private void jump() {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);  
        jumpSound.Play(); //Play Sound
    }

    //Jumping gravity
    private void jumpGravity() {
        if (rb.velocity.y < -maxFallSpeed) {
            rb.gravityScale = 0;
        } else {
            if (rb.velocity.y < -0.01f) {
                rb.gravityScale = gravityScale * fallGravityMultiplier;
            } else if (rb.velocity.y > hangVelocityActivate) {
                rb.gravityScale = gravityScale * hangGravityMultiplier;
            } else {
                rb.gravityScale = gravityScale;
            }
        }
    }

    //Creates a new collider based on the player's collider but shifted down
    //The function then checks if the new collider overlaps with the collider of the ground
    //Afterwards it returns true or false
    private bool isGrounded() {
        //Arguments: Center of Box, Size of Box, Angle of Box (float),
        //Direction of Box Shift, Distance of the Box Shift, Detects Collision With Layer
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.01f, jumpableGround);
    }


}