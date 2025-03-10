using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables
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

    //decides if on ground for jumping on layers
    public LayerMask isGround;

    //if player is touching ground
    bool grounded;

    //up/down left/right inputs
    float hInput;
    float vInput;

    //variable for direction
    Vector3 moveDirection;

    //ref for rigidbody
    Rigidbody rb;

    // on start up, i may be over-commenting
    private void Start()
    {
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //Test, remove/comment out when not testing double jump
        //ChangeHighJump();
    }

    //goes every update
    private void Update()
    {
        inputs();
        speedLimit();

        //makes a raycast to see if touching ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);
        //Debug.Log("Grounded: " + grounded);

        //makes drag only if touching ground, not in air
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
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
        if (Input.GetKey(jumpButton) && grounded && readyToJump)
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
    private void ChangeHighJump()
    {
        if(!highJump)
        {

            highJump = true;
            playerJumpForce *= 2;
        }
    } //END ChangeHighJump()


    /// <summary>
    /// Reverts to normal jump
    /// </summary>
    private void ChangeNormalJump()
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
}
