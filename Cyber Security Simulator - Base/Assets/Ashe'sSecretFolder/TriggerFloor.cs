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
                Debug.Log("Making the floor deadly...");
                deadlyFloor.UnlockFloor();

                Renderer floorRenderer = deadlyFloor.GetComponent<Renderer>();
                if (floorRenderer != null)
                {
                    floorRenderer.material.color = Color.red;
                }
                else
                {
                    Debug.LogWarning("DeadlyFloor does not have a renderer");
                }
            }
            else
            {
                Debug.LogWarning("DeadlyFloor reference not set");
            }
        }
    }
}