using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMoveComputer : MonoBehaviour
{
    protected FloorIsLava floorIsLava;
    [SerializeField] private Password password;
    [SerializeField] protected CinemachineFreeLook cam;
    [SerializeField] protected cam camMovement;
    [SerializeField] protected PlayerMovement playerMovement;

    [SerializeField] private Transform moveToPoint;
    private Vector3 roundedMovePt;
    [SerializeField] private Transform computer;
    [SerializeField] protected Vector3 oldPos;
    protected Vector3 roundedOldPos;
    public float damping = 5;
    private float interpolateVal = 0.01f;
    private bool enterCutscene;
    private bool exitCutscene;

    [SerializeField] private Canvas computerUI;

    private void Start()
    {
        floorIsLava = FindAnyObjectByType<FloorIsLava>();
        roundedMovePt = new Vector3(Mathf.Floor(moveToPoint.position.x), Mathf.Floor(moveToPoint.position.y), Mathf.Floor(moveToPoint.position.z));
    }


    private void OnTriggerEnter(Collider other)
    {
        if(!floorIsLava.lava)
        {
            //Acts as temporary trigger right now, may or may not be final
            DisablePlayerMovement(other);
            if (other.gameObject.layer == 7)
            {
                enterCutscene = true;
            }
        }
        
    }


    private void Update()
    {
        //Check if player/camera moves towards or away from the computer
        if(enterCutscene)
        {
            MoveCameraToComputer(moveToPoint, computerUI);
        }

        if(exitCutscene)
        {
            MoveCameraToPlayer(oldPos, camMovement.playerPhy.gameObject.transform.rotation, computerUI);
        }

        CheckCutscene(roundedMovePt, roundedOldPos, computer, playerMovement.gameObject.transform);
    }

    /// <summary>
    /// Disable player movement script to prevent from moving while looking at the computer
    /// </summary>
    protected void DisablePlayerMovement(Collider _other)
    {
        //Check that the player is the one who entered, this can be changed based on what the trigger is for the computer screen activation
        if(_other.gameObject.layer == 7)
        {
            //Disable player movement script, seperate camera from player
            Debug.Log("Trigger");
            playerMovement.enabled = false;
            cam.LookAt = null;
            cam.Follow = null;
            
            //Get camera position at this point to set back after the computer screen is done
            oldPos = cam.transform.position;
            roundedOldPos = new Vector3(Mathf.Floor(oldPos.x), Mathf.Floor(oldPos.y), Mathf.Floor(oldPos.z));
            //cam.axis
            //Still repsonds to input, disable
        }
        
    } //END DisablePlayerMovement()

    //On Click of button, start moving camera back to player
    public void EnablePlayerMovement()
    {
        //Re-enable player movement and assign player to the camera
        playerMovement.enabled = true;
        cam.LookAt = null;
        cam.Follow = null;
    } //END EnablePlayerMovement()

    /// <summary>
    /// Start movement away from computer
    /// </summary>
    public void ExitCutscene()
    {
        exitCutscene = true;
        EnablePlayerMovement();
    } //END ExitCutscene()

    #region Movement

    /// <summary>
    /// Move camera away from the player and to the computer
    /// </summary>
    /// <param name="_moveTo">Position the camera needs to move to in front of computer</param>
    /// <param name="_UI">Computer UI that needs to be enabled</param>
    public void MoveCameraToComputer(Transform _moveTo, Canvas _UI)
    {
       cam.transform.position =  Vector3.Lerp(cam.transform.position, _moveTo.position, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveTo.rotation, interpolateVal); //Close, not completely rotated though, also depends on where you start out
        //cam.LookAt = computer;
        
        //Enable UI and mouse movement
        _UI.enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        password.passwordActive = true;


    } //END MoveCameraToComputer()

    /// <summary>
    /// Move camera away from computer, back to following the player
    /// </summary>
    /// <param name="_moveToPos">Position near player to move to, recorded from before movement to the computer started</param>
    /// <param name="_moveToRot">Rotation towards computer</param>
    /// <param name="_UI">Computer Ui to disable</param>
    public void MoveCameraToPlayer(Vector3 _moveToPos, Quaternion _moveToRot, Canvas _UI)
    {
        Debug.Log("Move cam to player");
        cam.transform.position = Vector3.Lerp(cam.transform.position, _moveToPos, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveToRot, interpolateVal);

        //Disable Ui and mouse
        _UI.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    } //END MoveCameraToPlayer()

    /// <summary>
    /// Check if the camera is currently moving towards or away from the computer, and if it has reached its respective position
    /// </summary>
    /// <param name="_roundedEnter">Position to reach when entering the "cutscene"</param>
    /// <param name="_roundedExit">Position to reach when exiting the "cutscene"</param>
    /// <param name="_lookAtEnter">Position to look at when entering the "cutscene"</param>
    /// <param name="_lookAtExit">Position to look at when exiting the "cutscene"</param>
    public virtual void CheckCutscene(Vector3 _roundedEnter, Vector3 _roundedExit, Transform _lookAtEnter, Transform _lookAtExit)
    {
        //If moving towards computer
        if(enterCutscene)
        {
            //Round cam position down
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            //Check if in position
            if (_roundedCamPos == _roundedEnter)
            {
                Debug.Log("Cam assigned look at computer");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtEnter;
                enterCutscene = false;
                //_enterCut = false;
            }
        }

        //If moving away
        if(exitCutscene)
        {
            //Round cam position down
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            //Check if in position
            if (_roundedCamPos == _roundedExit)
            {
                Debug.Log("Cam assigned look at player");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtExit;
                cam.Follow = _lookAtExit;
                exitCutscene = false;
                //_exitCut = false;
            }
        }


    }//END CheckCutscene
    #endregion
} //END CameraMoveComputer.cs  
