using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatButton : MonoBehaviour
{
    [SerializeField] private Collider compCollider;
    [SerializeField] private Collider breakerCollider;
    [SerializeField] private CameraMoveComputer compScript;
    [SerializeField] private TriggerBreakerPanel breakerTriggerScript;
    [SerializeField] private BreakerPanel breakerScript;
    //Also some variable for opening the door

    /// <summary>
    /// Disable functionality for level, let player has a free pass
    /// </summary>
    public void PressStupidButton()
    {
        //Disable all of the puzzle components
        compCollider.enabled = false;
        breakerCollider.enabled = false;
        compScript.enabled = false;
        breakerTriggerScript.enabled = false;
        breakerScript.enabled = false;

        //Enable whatever allows player to pass
    } //END PressStupidButton()
   
}//END CheatButton.cs
