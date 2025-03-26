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
    [SerializeField] private Transform computer;
    public float damping = 5;
    private float interpolateVal = 0.01f;
    private bool enterCutscene;

    [SerializeField] private Canvas computerUI;

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
    }

    private void DisablePlayerMovement(Collider _other)
    {
        if(_other.gameObject.layer == 10)
        {
            Debug.Log("Trigger");
            playerMovement.enabled = false;
            cam.LookAt = null;
            cam.Follow = null;
            enterCutscene = true;
            //cam.axis
            //Still repsonds to input, disable
        }
        
    }

    private void MoveCameraToComputer()
    {
       cam.transform.position =  Vector3.Lerp(cam.transform.position, moveToPoint.position, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, computer.transform.rotation, interpolateVal); //Close, not completely rotated though, also depends on where you start out
        //cam.LookAt = computer;
        computerUI.enabled = true;  //When fix position check below, mve enavle to there
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(cam.transform.position == moveToPoint.position)
        {
            Debug.Log("Cam assigned look at computer");
            cam.LookAt = computer;
            enterCutscene = false;
        }
    }
}
