using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip triggerSound;

    private bool hasTriggered = false; // Keeps track of whether the trigger already happened

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (audioSource != null && triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound);
            }
        }
    }
}
