using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables

    //added for death manager
    public DisplayDeaths displayDeaths; 

    //freezes player for grappling
    public bool freeze;

    //stops playing moving while grappling
    public bool activeGrapple;

    private bool enableMovementOnNextTouch;
    //sets moveSpeed
    public float moveSpeed;

    //sets drag for player, 0 drag slippery like ice.
    public float groundDrag;

    //gets the orientation 
    public Transform orientation;

    //players height
    public float playerHeight;

    //how high jump
    public float playerJumpForce;
    public bool highJump = false;

    //how long after jump till can jump again
    public float jumpCooldown;

    //for when in air movement, after jumping
    public float airMultiplier;

    //sets if can jump
    bool readyToJump;

    public KeyCode jumpButton = KeyCode.Space;
    public KeyCode jumpButtonController = KeyCode.JoystickButton3;

    //decides if on ground for jumping on layers
    public LayerMask isGround;

    //if player is touching ground
    public bool grounded;

    //up/down left/right inputs
    public float hInput;
    public float vInput;

    //variable for direction
    Vector3 moveDirection;

    //ref for rigidbody
    Rigidbody rb;

    //vars from ashe - shooting 
    private bool isAiming = false; // Is the player aiming or not
    private float originalMoveSpeed; // Store movement speed
    public float aimSpeedMultiplier = 0.5f; // Slows movement while aiming
    public Transform cameraTransform; //cameras transform

    //Connect to wallrunning
    [SerializeField] private WallRunning wallRunning;

    //connects animator
    public Animator animator;

    //sounds
    public AudioSource walkingAudioSource;
    public AudioClip walkingClip;

    [Header("Debugging")]
    public bool debugMode;

    // on start up, i may be over-commenting
    private void Start()
    {
        animator.SetBool("Movement", false);
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //wallRunning.canWallRun = false;

        //Test, remove/comment out when not testing double jump amd slide

    }

    //goes every update
    private void Update()
    {
        inputs();
        speedLimit();
        HandleAiming();

        if (rb.velocity.magnitude > 0)
        {
            animator.SetBool("Movement", true);
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.PlayOneShot(walkingClip);
            }
        }
        else if (rb.velocity.magnitude <= 0)
        {
            animator.SetBool("Movement", false);
            walkingAudioSource.Stop();
        }

        //makes a raycast to see if touching ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);
        //Debug.Log("Grounded: " + grounded);

        //makes drag only if touching ground, not in air
        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0;


        //if freeze is true, freezes player
        if (freeze)
        {
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
        else if (activeGrapple)
        {
            moveSpeed = 7;
        }
        else
        {
            moveSpeed = 7;
        }
        // changes highjump, slide, and wallrun to active when at 3 deaths
        if (debugMode && displayDeaths.GetDeathCount() >= 3)
        {
            ChangeHighJump();
        }
    }

    //also every update but different
    private void FixedUpdate()
    {
        moving();
    }

    //gets if player is using horizontal or vertical inputs
    private void inputs()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        //if jump button pressed and on ground
        if ((Input.GetKey(jumpButton) || Input.GetKey(jumpButtonController)) && grounded && readyToJump)
        {
            readyToJump = false;

            jump();

            //calls the jumpReset function, delayed by jumpCooldown variable
            Invoke(nameof(jumpReset), jumpCooldown);
        }
    }

    //moves the player by adding force and using the orientation input for direction where to go
    private void moving()
    {
        //if grappling cant move
        if (activeGrapple) return;

        moveDirection = orientation.forward * vInput + orientation.right * hInput;

        //changes amount of force if on air or on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    //limits players speed
    private void speedLimit()
    {
        //if grappling cant move
        if (activeGrapple) return;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    //makes player jump
    private void jump()
    {
        //sets y velocity to 0 so always jumping same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //up force set by playerJumpForce, impulse so only once
        rb.AddForce(transform.up * playerJumpForce, ForceMode.Impulse);
        //Debug.Log("Jump Force: " + playerJumpForce + ", jump");
    }

    /// <summary>
    /// Changes to high jump, doubles jump force
    /// </summary>
    public void ChangeHighJump()
    {
        if(!highJump)
        {

            highJump = true;
            playerJumpForce *= 2;
            wallRunning.canWallRun = true;
        }
    } //END ChangeHighJump()


    /// <summary>
    /// Reverts to normal jump
    /// </summary>
    public void ChangeNormalJump()
    {
        if(highJump)
        {

            highJump = false;
            playerJumpForce /= 2;
        }
    }// END ChangeNormalJump()

    //sets readyToJump to true so player can jump again
    private void jumpReset()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Access canSlide variable to change to true
    /// </summary>


    /// <summary>
    /// Get input for slide
    /// </summary>


    //calculates force for moving playing to grappling point in grappling gun 
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    //velocity for grappling
    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            resetGrappling();

            GetComponent<grapple>().StopGrapple();
        }
    }

    public void resetGrappling()
    {
        activeGrapple = false;
    }

    // for above code its for grapple will fix later

    //handle aiming for shooting script from ashe player controller
    void HandleAiming()
    {
        if (Input.GetButton("Fire2") && displayDeaths.GetDeathCount() >= 1) // Right Mouse Button
        {
            if (!isAiming)
            {
                isAiming = true;
                moveSpeed = originalMoveSpeed * aimSpeedMultiplier; // Slow movement
            }
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            transform.forward = cameraRight;
        }
        else
        {
            if (isAiming) // Reset when stopping aim
            {
                isAiming = false;
                moveSpeed = originalMoveSpeed; // Restore movement speed
            }
        }
    }
}
