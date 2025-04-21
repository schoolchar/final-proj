using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Camera movement for breaker panel, similar to computer screen
/// </summary>
public class TriggerBreakerPanel : CameraMoveComputer
{
    [Header("Breaker Panel")]
    [SerializeField] private GameObject breakerPanelCanvasObj;
    [SerializeField] private Canvas breakerPanelCanvas;
    [SerializeField] private CameraMoveComputer moveScript;
    [SerializeField] private BreakerPanel breakerPanel;
    bool enterCutsceneB;
    bool exitCutsceneB;

    public Light targetLight;

    [SerializeField] private Transform moveToPt;
    private Vector3 breakerRounddMovePt;
    [SerializeField] private Transform breakerObj;
    [SerializeField] private AudioSource breakerSound;
    public bool debugMode;
    private void Start()
    {
        floorIsLava = FindAnyObjectByType<FloorIsLava>();
        breakerRounddMovePt = new Vector3(Mathf.Floor(moveToPt.position.x), Mathf.Floor(moveToPt.position.y), Mathf.Floor(moveToPt.position.z));
    }

    private void Update()
    {
        //Check if player has triggered whatever to open the breaker panel, or close breaker panel
        if(enterCutsceneB)
        {
            MoveCameraToComputer(moveToPt, breakerPanelCanvas, breakerPanelCanvasObj);
        }

        if(exitCutsceneB)
        {
            Debug.Log("Exit cutscene on breaker triggered");
            MoveCameraToPlayer(oldPos, camMovement.playerPhy.gameObject.transform.rotation, breakerPanelCanvas, breakerPanelCanvasObj);
        }

        CheckCutscene(breakerRounddMovePt, roundedOldPos, breakerObj, playerMovement.gameObject.transform);

        if (breakerPanelCanvas.enabled && Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            
            ExitCutscene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player has already done the computer password, then allow them to open breaker panle
        if(floorIsLava.lava || debugMode)
        {
            DisablePlayerMovement(other);
            if (other.gameObject.layer == 7)
            {
                enterCutsceneB = true;
                StartCoroutine(FadeOut());
            }
        }
       
    }

    /// <summary>
    /// Manage player/camera movement in and out of the breaker panel
    /// </summary>
    public override void CheckCutscene(Vector3 _roundedEnter, Vector3 _roundedExit, Transform _lookAtEnter, Transform _lookAtExit)
    {
        //If entering the breaker panel movement
        if (enterCutsceneB)
        {
            //Round current cam position down
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            //Check if the camera is in position
            if (_roundedCamPos == _roundedEnter)
            {
                Debug.Log("Cam assigned look at breaker");

                //Need to fix it is very jarring
                cam.LookAt = _lookAtEnter;
                enterCutsceneB = false;
                breakerPanel.breakerActive = true;
                //_enterCut = false;
            }
        }

        //If exiting the breaker panel movement
        if (exitCutsceneB)
        {
            //Round current camera position down
            Vector3 _roundedCamPos = new Vector3(Mathf.Floor(cam.transform.position.x), Mathf.Floor(cam.transform.position.y), Mathf.Floor(cam.transform.position.z));

            //If camera is in position, reset to look at player
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
    } //END CheckCutscene()

    /// <summary>
    /// Exit the camera movement and let player move again
    /// </summary>
    public void ExitCutsceneB()
    {
        exitCutsceneB = true;
        EnablePlayerMovement();
    } //END ExitCutsceneB()


    public void PlayWinSound()
    {
        breakerSound.Play();
        targetLight.color = Color.white;
    }
} //END TriggerBreakerPanel.cs
