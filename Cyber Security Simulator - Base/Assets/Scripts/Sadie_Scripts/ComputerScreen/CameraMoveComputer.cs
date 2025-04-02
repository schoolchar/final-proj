using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMoveComputer : MonoBehaviour
{
    protected FloorIsLava floorIsLava;
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

    protected void DisablePlayerMovement(Collider _other)
    {
        if(_other.gameObject.layer == 7)
        {
            Debug.Log("Trigger");
            playerMovement.enabled = false;
            cam.LookAt = null;
            cam.Follow = null;
            
            oldPos = cam.transform.position;
            roundedOldPos = new Vector3(Mathf.Floor(oldPos.x), Mathf.Floor(oldPos.y), Mathf.Floor(oldPos.z));
            //cam.axis
            //Still repsonds to input, disable
        }
        
    }

    //On Click of button, start moving camera back to player
    public void EnablePlayerMovement()
    {
        playerMovement.enabled = true;
        cam.LookAt = null;
        cam.Follow = null;
    }

    public void ExitCutscene()
    {
        exitCutscene = true;
        EnablePlayerMovement();
    }

    #region Movement
    //Reusability?
    public void MoveCameraToComputer(Transform _moveTo, Canvas _UI)
    {
       cam.transform.position =  Vector3.Lerp(cam.transform.position, _moveTo.position, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveTo.rotation, interpolateVal); //Close, not completely rotated though, also depends on where you start out
        //cam.LookAt = computer;
        
        _UI.enabled = true;  //When fix position check below, mve enavle to there
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

       
    }


    public void MoveCameraToPlayer(Vector3 _moveToPos, Quaternion _moveToRot, Canvas _UI)
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, _moveToPos, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveToRot, interpolateVal);

        _UI.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }


    public virtual void CheckCutscene(Vector3 _roundedEnter, Vector3 _roundedExit, Transform _lookAtEnter, Transform _lookAtExit)
    {
        if(enterCutscene)
        {

            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            if (_roundedCamPos == _roundedEnter)
            {
                Debug.Log("Cam assigned look at computer");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtEnter;
                enterCutscene = false;
                //_enterCut = false;
            }
        }

        if(exitCutscene)
        {
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

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


    }
    #endregion
}
