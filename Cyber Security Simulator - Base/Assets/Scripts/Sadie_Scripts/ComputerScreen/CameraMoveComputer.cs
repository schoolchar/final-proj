using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using TMPro;

public class CameraMoveComputer : MonoBehaviour
{
    protected FloorIsLava floorIsLava;
    [SerializeField] private AudioSource lavaSound;
    public AudioSource computerSound;
    [SerializeField] protected Password password;
    [SerializeField] protected CinemachineFreeLook cam;
    [SerializeField] protected cam camMovement;
    [SerializeField] protected PlayerMovement playerMovement;
    [SerializeField] protected PlayerShooting shooting;

    [SerializeField] private Transform moveToPoint;
    private Vector3 roundedMovePt;
    [SerializeField] private Transform computer;
    [SerializeField] protected Vector3 oldPos;
    protected Vector3 roundedOldPos;
    public float damping = 5;
    private float interpolateVal = 0.01f;
    private bool enterCutscene;
    private bool exitCutscene;

    [SerializeField] protected Transform lookAt;

    [SerializeField] private Material offMat;
    [SerializeField] private MeshRenderer[] securityCamParts;
    [SerializeField] private AudioSource[] securityCamNoise;

    private bool canExit = true;
    [SerializeField] private GameObject computerUIObj;
    [SerializeField] private Canvas computerUI;

    [Header("Post processing")]
    [SerializeField] private Volume volume;

    [Header("Computer Visible UI")]
    [SerializeField] private RawImage[] rawImg;
    [SerializeField] private Image[] normImg;
    [SerializeField] private TextMeshProUGUI[] extraInstructions;
    private Color decrementColor = new Color(0, 0, 0, 0.05f);

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
                StartCoroutine(FadeOut());
            }
        }
        
    }


    private void Update()
    {
        //Check if player/camera moves towards or away from the computer
        if(enterCutscene)
        {
            MoveCameraToComputer(moveToPoint, computerUI, computerUIObj);
        }

        if (exitCutscene)
        {
            MoveCameraToPlayer(oldPos, camMovement.playerPhy.gameObject.transform.rotation, computerUI, computerUIObj);
        }

        CheckCutscene(roundedMovePt, roundedOldPos, computer, playerMovement.gameObject.transform, lookAt);

        if(password.passwordActive && Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            ExitCutscene();
        }
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
            shooting.enabled = false;
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
        shooting.enabled = true;
        cam.LookAt = null;
        cam.Follow = null;
    } //END EnablePlayerMovement()

    /// <summary>
    /// Start movement away from computer
    /// </summary>
    public void ExitCutscene()
    {
        Debug.Log("Exit cutscene function");
        exitCutscene = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //if(password.passwordSolved == false)
            EnablePlayerMovement();
    } //END ExitCutscene()

    #region Movement

    /// <summary>
    /// Move camera away from the player and to the computer
    /// </summary>
    /// <param name="_moveTo">Position the camera needs to move to in front of computer</param>
    /// <param name="_UI">Computer UI that needs to be enabled</param>
    public void MoveCameraToComputer(Transform _moveTo, Canvas _UI, GameObject _UIObj)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        cam.transform.position =  Vector3.Lerp(cam.transform.position, _moveTo.position, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveTo.rotation, interpolateVal); //Close, not completely rotated though, also depends on where you start out
        //cam.LookAt = computer;

      
        //Enable UI and mouse movement
        _UIObj.SetActive(true);
        _UI.enabled = true;
        
        password.passwordActive = true;

        


    } //END MoveCameraToComputer()

    /// <summary>
    /// Move camera away from computer, back to following the player
    /// </summary>
    /// <param name="_moveToPos">Position near player to move to, recorded from before movement to the computer started</param>
    /// <param name="_moveToRot">Rotation towards computer</param>
    /// <param name="_UI">Computer Ui to disable</param>
    public void MoveCameraToPlayer(Vector3 _moveToPos, Quaternion _moveToRot, Canvas _UI, GameObject _UIObj)
    {
        Debug.Log("Move cam to player " + this.gameObject.name);
        cam.transform.position = Vector3.Lerp(cam.transform.position, _moveToPos, interpolateVal);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, _moveToRot, interpolateVal);

        //Disable Ui and mouse
        _UIObj.SetActive(false);
        _UI.enabled = false;
       /* Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/

    } //END MoveCameraToPlayer()

    /// <summary>
    /// Check if the camera is currently moving towards or away from the computer, and if it has reached its respective position
    /// </summary>
    /// <param name="_roundedEnter">Position to reach when entering the "cutscene"</param>
    /// <param name="_roundedExit">Position to reach when exiting the "cutscene"</param>
    /// <param name="_lookAtEnter">Position to look at when entering the "cutscene"</param>
    /// <param name="_lookAtExit">Position to look at when exiting the "cutscene"</param>
    public virtual void CheckCutscene(Vector3 _roundedEnter, Vector3 _roundedExit, Transform _lookAtEnter, Transform _lookAtExitFollow, Transform _lookAtExitLookAt)
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
                cam.LookAt = _lookAtExitLookAt;
                cam.Follow = _lookAtExitFollow;
                exitCutscene = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
               
                //_exitCut = false;
            }
        }


    }//END CheckCutscene
    #endregion

    public void SecurityCam()
    {
        password.deactivateSecurity.SetActive(false);
        lavaSound.Play();

        for (int i = 0; i < securityCamParts.Length; i++)
        {
            securityCamParts[i].materials[0] = offMat;
        }

        for(int i = 0; i < securityCamNoise.Length; i++)
        {
            securityCamNoise[i].Play();
        }
    }


    //Post-Processing
    public IEnumerator FadeOut()
    {
        volume.weight += 0.05f;
        
       
        yield return new WaitForSeconds(0.05f);

        if (volume.weight < 1)
            StartCoroutine(FadeOut());
        else if (volume.weight >= 1)
            StartCoroutine(WaitToFadeIn());

    }

    public IEnumerator WaitToFadeIn()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        volume.weight -= 0.05f;
        for (int i = 0; i < rawImg.Length; i++)
        {
            rawImg[i].color += decrementColor;
        }

        for (int i = 0; i < normImg.Length; i++)
        {
            normImg[i].color += decrementColor;
        }

        for(int i = 0; i < extraInstructions.Length; i++)
        {
            extraInstructions[i].color += decrementColor;
        }
        

        yield return new WaitForSeconds(0.05f);
        if (volume.weight > 0)
            StartCoroutine(FadeIn());
            
    }

} //END CameraMoveComputer.cs  
