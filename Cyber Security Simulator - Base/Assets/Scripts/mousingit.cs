using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousingingit : MonoBehaviour
{

    public bool lockCursor = true;

    public void Start()
    {
        
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftCommand))
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    }
}