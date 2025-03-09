using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    //variables
    public Transform orientation;
    public Transform player;
    public Transform playerPhy;
    public Rigidbody rb;

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
        Vector3 viewDir = player.position - new Vector3 (transform.position.x, player.position.y, transform.position.z);    
        orientation.forward = viewDir.normalized;

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        if (inputDir != Vector3.zero)
        {
            playerPhy.forward = Vector3.Slerp(playerPhy.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

    }
}
