using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyFloor : MonoBehaviour
{
    public GameObject player;
    private gameManager gm;
    public bool unlockFloor = false;

    private void Start()
    {
        gm = gameManager.instance;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (unlockFloor == true)
        {
            if (other.gameObject == player)
            {
                if (gm != null)
                {
                    gm.totalHealth = 0; // This sets the health to 0
                }

            }
        }
    }
    public void UnlockFloor()
    {
        if (!unlockFloor)
        {
            unlockFloor = true;
        }
    }
}

