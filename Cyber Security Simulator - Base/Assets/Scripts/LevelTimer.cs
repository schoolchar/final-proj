using UnityEngine;
using TMPro;
using System.Collections;
public class LevelTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float levelTime = 60f;
    private float timeRemaining;
    private bool timeExpired = false;
    private int lastFullSecond;
    private bool isPaused = false;
    public Light targetLight;
    public Light spotLight;

    [Header("UI Reference")]
    public TextMeshProUGUI timerText;

    [Header("Audio Clips")]
    public AudioClip tickSound;
    public AudioClip lossSound;
    private AudioSource audioSource;

    public gameManager manager;
   
    public LoadEveryScene loadEveryScene;

    void Start()
    {
        timeRemaining = levelTime;
        lastFullSecond = Mathf.CeilToInt(timeRemaining);
        UpdateTimerDisplay();

        // Try to get an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not found, adding one... :)");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        manager = FindAnyObjectByType<gameManager>();

    }

    void Update()
    {

        // If the timer is paused, do nothing
        if (isPaused)
            return;

        if (!timeExpired && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // This will wait a second before playing the tick noise
            int currentFullSecond = Mathf.CeilToInt(timeRemaining);
            if (currentFullSecond < lastFullSecond)
            {
                lastFullSecond = currentFullSecond;
                if (tickSound != null)
                {
                    audioSource.PlayOneShot(tickSound);
                }
            }

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timeExpired = true;
                StartCoroutine(HandleTimeOut());
            }

            // Update timer, as the name implies
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        timerText.text = timeRemaining.ToString("F0");
    }

    // This will play the loss sound and wait until it finishes
    IEnumerator HandleTimeOut()
    {
        if (lossSound != null)
        {
            ChangeLightToRed();
            audioSource.PlayOneShot(lossSound);
            // Waits for the duration of the loss sound clip
            yield return new WaitForSeconds(lossSound.length);
        }
        else
        {
            Debug.LogWarning("Please assign the death noise.");
        }

        manager.totalHealth = 0;
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    void ChangeLightToRed()
    {
        if (targetLight != null)
        {
            targetLight.color = Color.red;
            spotLight.color = Color.red;
        }
    }
}

