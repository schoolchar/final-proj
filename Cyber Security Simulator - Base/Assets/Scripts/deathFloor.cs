using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject player;

    // Reference to the DisplayDeaths script
    public DisplayDeaths displayDeaths;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the object
        if (other.gameObject == player)
        {
            // Increment the deaths count
            displayDeaths.IncrementDeaths();

            // Teleport the player to the coordinates (0, 0, 0)
            player.transform.position = new Vector3(0, 1, 0);
        }
    }
}
