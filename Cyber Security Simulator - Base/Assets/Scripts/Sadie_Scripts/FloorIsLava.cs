using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : MonoBehaviour
{
    public bool lava;
    //[SerializeField] private DisplayDeaths displayDeaths;
    [SerializeField] private GameObject player;

    private void OnCollisionStay(Collision collision) //Change to enter later, stay rn to debug
    {
        KillPlayer(collision);
    }

    void KillPlayer(Collision _collision)
    {
        if (lava && _collision.gameObject.layer == 10)
        {
            // Increment the deaths count
            //displayDeaths.IncrementDeaths();
            Debug.Log("Player died of lava");
            // Teleport the player to the coordinates (0, 0, 0) on death by lava, these coordinates can be changed later
            player.transform.position = new Vector3(0, 5, 0);
        }
    }

    public void TurnOnLava()
    {
        Debug.Log("Lava on");
        lava = true;
    }
}
