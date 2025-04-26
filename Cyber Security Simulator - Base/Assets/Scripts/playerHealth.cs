using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public GameObject enemyBullet;


   

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == enemyBullet.name + "(Clone)")
        {
            gameManager.instance.healthLose(); //increase variable to win game
        }
    }
}
