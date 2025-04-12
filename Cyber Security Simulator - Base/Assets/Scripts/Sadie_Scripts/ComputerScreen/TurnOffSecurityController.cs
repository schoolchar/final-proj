using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffSecurityController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton0) && GetComponent<Button>().enabled)
        {
            GetComponent<Button>().onClick.Invoke();
            this.enabled = false;
        }
    }
}
