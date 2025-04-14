using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public string sceneToLoad;

    [Header("Audio Settings")]
    public AudioClip transitionSound;

    private AudioSource audioSource;
    private bool hasTriggered = false;

    [Header("Timer Reference")]
    public LevelTimer levelTimer;

    public gameManager manager;

    void Awake()
    {
        // Get or add an AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not found, adding one... <3");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // Pause the timer
            if (levelTimer != null)
            {
                levelTimer.PauseTimer();
            }

            StartCoroutine(PlaySoundAndChangeScene());
        }
    }

    IEnumerator PlaySoundAndChangeScene()
    {
        if (transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
            // Wait until the transition sound finishes
            yield return new WaitForSeconds(transitionSound.length);
        }
        else
        {
            Debug.LogWarning("Please add a sound.");
        }

        manager.escapeRoomWon = true;
        // Load scene
        SceneManager.LoadScene(sceneToLoad);
    }
}