using UnityEngine;

public class TriggerCanvasPopup : MonoBehaviour
{
    public Canvas canvasToShow; // Assign in inspector
    private bool isPlayerInTrigger = false;

    void Start()
    {
        if (canvasToShow != null)
        {
            canvasToShow.enabled = false;
        }
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            if (canvasToShow != null)
            {
                canvasToShow.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            if (canvasToShow != null)
            {
                canvasToShow.enabled = false;
            }
        }
    }
}
