using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBreakerPanel : CameraMoveComputer
{
    [SerializeField] private Canvas breakerPanelCanvas;
    [SerializeField] private CameraMoveComputer moveScript;

    bool enterCutsceneB;
    bool exitCutsceneB;

    [SerializeField] private Transform moveToPt;
    private Vector3 breakerRounddMovePt;
    [SerializeField] private Transform breakerObj;

    private void Start()
    {
        floorIsLava = FindAnyObjectByType<FloorIsLava>();
        breakerRounddMovePt = new Vector3(Mathf.Floor(moveToPt.position.x), Mathf.Floor(moveToPt.position.y), Mathf.Floor(moveToPt.position.z));
    }

    private void Update()
    {
        if(enterCutsceneB)
        {
            MoveCameraToComputer(moveToPt, breakerPanelCanvas);
        }

        if(exitCutsceneB)
        {
            MoveCameraToPlayer(oldPos, camMovement.playerPhy.gameObject.transform.rotation, breakerPanelCanvas);
        }

        CheckCutscene(breakerRounddMovePt, roundedOldPos, breakerObj, playerMovement.gameObject.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(floorIsLava.lava)
        {
            DisablePlayerMovement(other);
            if (other.gameObject.layer == 7)
            {
                enterCutsceneB = true;
            }
        }
       
    }

    void OpenBreakerPanel()
    {
       
    }

    public override void CheckCutscene(Vector3 _roundedEnter, Vector3 _roundedExit, Transform _lookAtEnter, Transform _lookAtExit)
    {
        if (enterCutsceneB)
        {

            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            if (_roundedCamPos == _roundedEnter)
            {
                Debug.Log("Cam assigned look at breaker");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtEnter;
                enterCutsceneB = false;
                //_enterCut = false;
            }
        }

        if (exitCutsceneB)
        {
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            if (_roundedCamPos == _roundedExit)
            {
                Debug.Log("Cam assigned look at player");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtExit;
                cam.Follow = _lookAtExit;
                exitCutsceneB = false;
                //_exitCut = false;
            }
        }
    }

    public void ExitCutsceneB()
    {
        exitCutsceneB = true;
        EnablePlayerMovement();
    }
}
