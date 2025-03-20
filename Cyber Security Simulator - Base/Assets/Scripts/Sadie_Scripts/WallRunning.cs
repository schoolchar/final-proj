using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public bool canWallRun;

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
            Debug.Log("Debug mode on");
            //canWallRun = true;
        }
    }

    private void Update()
    {
        Debug.Log(isWallrunning);
        CheckForWall();
        StateMachine();
      


        DropFromWall();
       
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

       
            //Debug Rays
        Debug.DrawRay(transform.position, orientation.right * wallCheckDistance, Color.yellow);
        Debug.DrawRay(transform.position, -orientation.right * wallCheckDistance, Color.yellow);

        
    } //END CheckForWall()

    /// <summary>
    /// Check if player is grounded or not
    /// </summary>
    private bool AboveGround()
    {
        //Debug.Log("Above ground = " + Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground));
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
        if ((wallLeft || wallRight) && AboveGround() && canWallRun && !coolDownEnabled)
        {
            if(!isWallrunning)
            {
                StartWallRunning();
                StartCoroutine(CoolDownWallRunTransition());
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
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Get cross product of wall and upwards direction for where player will hover
        Vector3 _wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 _wallForward = Vector3.Cross(_wallNormal, transform.up);

        //Moves player along wall, keep to wall
        rb.AddForce(_wallForward * wallRunForce, ForceMode.Force);
        if (!player.grounded)
        {
            rb.AddForce((_wallNormal - transform.position) * wallRunForce, ForceMode.Force);
        }
        
    } //END WallRunMovement()

    /// <summary>
    /// Takes input to drop from wall while wallrunning
    /// </summary>
    void DropFromWall()
    {

        //Check for spacbar
        if (isWallrunning && (!wallRight && !wallLeft))
        {
            rb.useGravity = true;
            isWallrunning = false;
            StartCoroutine(CoolDownWallRunTransition());
        }
        

        //Check for distance from wall
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Press space");
            rb.useGravity = true;
            isWallrunning = false;
            StartCoroutine(CoolDownWallRunTransition());
            Debug.Log("Can wall run = " + canWallRun);
        }
    } //END DropFromWall

   

    /// <summary>
    /// Cooldown to prevent wall running enabling while dropping from wall
    /// </summary>
    private IEnumerator CoolDownWallRunTransition()
    {
        coolDownEnabled = true;
        yield return new WaitForSeconds(2);
        coolDownEnabled = false;
    } //END CoolDownWallRunTransition()

    

}