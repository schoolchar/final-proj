using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public AudioSource triggerSound;
    public DeadlyFloor deadlyFloor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone");

            if (triggerSound != null)
            {
                triggerSound.Play();
            }

            if (deadlyFloor != null)
            {
                Debug.Log("Unlocking deadly floor...");
                deadlyFloor.UnlockFloor();
                Debug.Log("unlockFloor set to: " + deadlyFloor.unlockFloor);
            }
            else
            {
                Debug.LogWarning("DeadlyFloor reference is not set!");
            }
        }
    }
}
