using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wilburrr : MonoBehaviour
{
    public GameObject tutorial;
    public AudioSource audioSource;
    public AudioClip triggerSound;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            tutorial.SetActive(true);

            if (audioSource != null && triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound);
            }
        }
    }
}

