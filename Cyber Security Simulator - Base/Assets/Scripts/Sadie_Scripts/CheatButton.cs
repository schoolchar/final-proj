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

    [SerializeField] private Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            animator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            animator.SetBool("Open", false);
        }
    }


   
}//END CheatButton.cs
