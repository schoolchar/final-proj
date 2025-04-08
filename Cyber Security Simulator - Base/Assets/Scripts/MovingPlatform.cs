using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The direction in which the platform should move.")]
    public Vector3 moveDirection = Vector3.forward;

    [Tooltip("The distance the platform should move.")]
    public float moveDistance = 5f;

    [Tooltip("The speed at which the platform moves.")]
    public float moveSpeed = 2f;

    private bool triggered = false;

    // Positions
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + moveDirection.normalized * moveDistance;
    }

    // Trigger, make sure to give the platform a secondary box collider, and set it to "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // Make sure the player has the tag "Player"
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(MovePlatform());
        }
    }

    // Move the platform
    private IEnumerator MovePlatform()
    {
        float elapsedTime = 0f;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float duration = journeyLength / moveSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
}
