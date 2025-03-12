using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    private bool canWallRun;

    public LayerMask wall;
    public LayerMask ground;
    public float wallRunForce;

    private float horizontal;
    private float vertical;


    public float wallCheckDistance;
    public float wallCheckDistanceEnd;
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

    private bool coolDownEnabled; //Test for ending wall run, when enabled cannot exit wall run state

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();

        if(player.debugMode)
        {
            Debug.Log("Debug mode");
            canWallRun = true;
        }
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();

        //Test for input on ending wall run
        if(Input.GetKeyDown(KeyCode.Space) && !coolDownEnabled && isWallrunning)
        {
            //isWallrunning = false;
            EndWallRunMovement();
        }
    }

    private void FixedUpdate()
    {
        if(isWallrunning)
        {
            WallRunMovement();
        }
        
    }

    /// <summary>
    /// Check sides of player for wall to run on
    /// </summary>
    private void CheckForWall()
    {
        //Use raycasts at player's right and left
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wall);

        if(wallRight || wallLeft)
        {
            Debug.Log("Wall in range");
        }
            //Debug Rays
        Debug.DrawRay(transform.position, orientation.right * wallCheckDistance, Color.yellow);
        Debug.DrawRay(transform.position, -orientation.right * wallCheckDistance, Color.yellow);

        
    } //END CheckForWall()

    /// <summary>
    /// Check if player is grounded or not
    /// </summary>
    private bool AboveGround()
    {
        Debug.Log("Above ground = " + Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground));
        return Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    } //END AboveGround()


    /// <summary>
    /// Switches player between wall running and not wallrunning states
    /// </summary>
    private void StateMachine()
    {
        //Get player input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log("Vertical = " + vertical);
        //State 1 - Wallrun, check for wall on side and if player is jumping
        if ((wallLeft || wallRight) && AboveGround() && canWallRun)
        {
            if(!isWallrunning)
            {
                Debug.Log("Wallrunning");
                StartWallRunning();
                StartCoroutine(CoolDownWallRunTransition());
            }
        }
        //State 3 - None, when player is not on wall
        else
        {
            if(isWallrunning)
            {
                Debug.Log("Away from wall, no wallrun");
                StopWallRun();
                //EndWallRunMovement();
            }
        }
    } //END StateMachine()


    private void StartWallRunning()
    {
        isWallrunning = true;
    }

    private void StopWallRun()
    {
       // Debug.Log("End wallrunning");
        isWallrunning = false;
    }

    /// <summary>
    /// COntrol movement while wall running
    /// </summary>
    private void WallRunMovement()
    {
        //Disable gravity, set new velocity to prevent moving downwards
        Debug.Log("Wall run movement");
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Get cross product of wall and upwards direction for where player will hover
        Vector3 _wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 _wallForward = Vector3.Cross(_wallNormal, transform.up);

        //Moves player along wall
        rb.AddForce(_wallForward * wallRunForce, ForceMode.Force);
    } //END WallRunMovement()

    
    private void EndWallRunMovement()
    {
       // Debug.Log("Enable gravity");
        rb.useGravity = true;
    }

    private IEnumerator CoolDownWallRunTransition()
    {
        coolDownEnabled = true;
        yield return new WaitForSeconds(2);
        coolDownEnabled = false;
    }

}