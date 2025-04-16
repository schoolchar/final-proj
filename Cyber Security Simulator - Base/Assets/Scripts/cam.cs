using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cam : MonoBehaviour
{
    //variables
    public Transform orientation;
    public Transform player;
    public Transform playerPhy;
    public Rigidbody rb;

    //vars from ashe
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float zoomSpeed = 10f;
    public CinemachineFreeLook freeLookCamera; // Reference to the Cinemachine Free Look Camera

    //testing grapple
    //public Transform lookAT;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotates player
        // commented out to test grapple

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        if (inputDir != Vector3.zero)
        {
            playerPhy.forward = Vector3.Slerp(playerPhy.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        //added to test grapple
        //Vector3 dirTolookAt = lookAT.position - new Vector3(transform.position.x, lookAT.position.y, transform.position.z);
        //orientation.forward = dirTolookAt.normalized;

        //playerPhy.forward = dirTolookAt.normalized;
    }

    private void LateUpdate()
    {
        // Aim Zoom with Cinemachine
        float targetFOV = Input.GetButton("Fire2") ? aimFOV : normalFOV;
        freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(freeLookCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}