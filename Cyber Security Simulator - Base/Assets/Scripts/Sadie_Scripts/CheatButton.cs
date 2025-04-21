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

    public Canvas canvasToShow; // Assign in inspector
    private bool isPlayerInTrigger = false;

    void Start()
    {
        if (canvasToShow != null)
        {
            canvasToShow.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            if (canvasToShow != null)
            {
                canvasToShow.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            if (canvasToShow != null)
            {
                canvasToShow.enabled = false;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PressButton(Collider other)
    {
            animator.SetBool("Open", true);

        if (canvasToShow != null)
            canvasToShow.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


