using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public LayerMask wall;
    public LayerMask ground;
    public float wallRunForce;

    private float horizontal;
    private float vertical;


    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;


    public Transform orientation;
    private PlayerMovement player;
    private Rigidbody rb;

    //To add to player movement
    public bool isWallrunning;
    public float wallRunSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(isWallrunning)
        {
            WallRunMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wall);
    }


    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    private void StateMachine()
    {
        //Get player input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //State 1 - Wallrun
        if ((wallLeft || wallRight) && vertical > 0 && AboveGround())
        {
            if(!isWallrunning)
            {
                StartWallRunning();
            }
        }
        //State 3 - None
        else
        {
            if(isWallrunning)
            {
                StopWallRun();
            }
        }
    }


    private void StartWallRunning()
    {
        isWallrunning = true;
    }

    private void StopWallRun()
    {
        isWallrunning = false;
    }

    private void WallRunMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 _wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 _wallForward = Vector3.Cross(_wallNormal, transform.up);

        rb.AddForce(_wallForward * wallRunForce, ForceMode.Force);
    }

}