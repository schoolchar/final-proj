using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMoveComputer : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cam;
    [SerializeField] private cam camMovement;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Transform moveToPoint;
    private Vector3 roundedMovePt;
    [SerializeField] private Transform computer;
    [SerializeField] private Vector3 oldPos;
    private Vector3 roundedOldPos;
    public float damping = 5;
    private float interpolateVal = 0.01f;
    private bool enterCutscene;
    private bool exitCutscene;

    [SerializeField] private Canvas computerUI;

    private void Start()
    {
        roundedMovePt = new Vector3(Mathf.Floor(moveToPoint.position.x), Mathf.Floor(moveToPoint.position.y), Mathf.Floor(moveToPoint.position.z));
    }


    private void OnTriggerEnter(Collider other)
    {
        //Acts as temporary trigger right now, may or may not be final
        DisablePlayerMovement(other);
    }


    private void Update()
    {
        if(enterCutscene)
        {
            MoveCameraToComputer();
        }

        if(exitCutscene)
        {
            MoveCameraToPlayer();
        }
    }

    private void DisablePlayerMovement(Collider _other)
    {
        if(_other.gameObject.layer == 7)
        {
            Debug.Log("Trigger");
            playerMovement.enabled = false;
            cam.LookAt = null;
            cam.Follow = null;
            enterCutscene = true;
            oldPos = cam.transform.position;
            roundedOldPos = new Vector3(Mathf.Floor(oldPos.x), Mathf.Floor(oldPos.y), Mathf.Floor(oldPos.z));
            //cam.axis
            //Still repsonds to input, disable
        }
        
    }

    //On Click of button, start moving camera back to player
    public void EnablePlayerMovement()
    {
        exitCutscene = true;
        playerMovement.enabled = true;
        cam.LookAt = null;
        cam.Follow = null;
    }

    private void MoveCameraToComputer()
    {
       cam.transform.position =  Vector3.Lerp(cam.transform.position, moveToPoint.position, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, moveToPoint.rotation, interpolateVal); //Close, not completely rotated though, also depends on where you start out
        //cam.LookAt = computer;
        
        computerUI.enabled = true;  //When fix position check below, mve enavle to there
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

        if(_roundedCamPos == roundedMovePt)
        {
            Debug.Log("Cam assigned look at computer");

            //Need to fix it is very jarring
            cam.LookAt = computer;
            enterCutscene = false;           
        }
    }


    private void MoveCameraToPlayer()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, oldPos, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camMovement.playerPhy.gameObject.transform.rotation, interpolateVal);

        computerUI.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

        if (_roundedCamPos == roundedOldPos)
        {
            Debug.Log("Cam assigned look at player");

            //Need to fix it is very jarring
            cam.LookAt = playerMovement.gameObject.transform;
            cam.Follow = playerMovement.gameObject.transform;
            exitCutscene = false;
        }
    }
}
