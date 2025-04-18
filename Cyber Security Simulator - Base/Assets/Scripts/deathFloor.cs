using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject player;
    private gameManager gm;

    private void Start()
    {
        gm = gameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the object
        if (other.gameObject == player)
        {
            if (gm != null)
            {
                gm.totalHealth = 0; // This sets the health to 0
            }

            // Reset player position to a specific point (e.g., 0, 1, 0)
            // player.transform.position = new Vector3(0, 1, 0);
        }
    }
}
