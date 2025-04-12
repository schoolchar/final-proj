using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : MonoBehaviour
{
    public bool lava;
    //[SerializeField] private DisplayDeaths displayDeaths;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPt;

    private void OnCollisionEnter(Collision collision) //Change to enter later, stay rn to debug
    {
        KillPlayer(collision);
    }

    void KillPlayer(Collision _collision)
    {
        if (lava && _collision.gameObject.layer == 7)
        {
            // Increment the deaths count
            //displayDeaths.IncrementDeaths();
            // Teleport the player to begining of level
            player.transform.position = respawnPt.position;
        }
    }

    //Called  on computer ui button that turns off security
    public void TurnOnLava()
    {
        Debug.Log("Lava on");
        lava = true;
    }
}
