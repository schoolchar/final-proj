using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorIsLava : MonoBehaviour
{
    public bool lava;
    //[SerializeField] private DisplayDeaths displayDeaths;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPt;

    [SerializeField] AudioSource lavaSound;
    [SerializeField] private ParticleSystem[] lavaParticles;

    gameManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<gameManager>();
    }

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
            //manager.healthLose();
            if(manager.totalHealth > 0) 
                player.transform.position = respawnPt.position;
            else if(manager.totalHealth == 0)
            {
                StartCoroutine(WaitToLoadHub());
            }
        }
    }

    //Called  on computer ui button that turns off security
    public void TurnOnLava()
    {
        Debug.Log("Lava on");
        lava = true;

        for (int i = 0; i < lavaParticles.Length; i++)
        {
            lavaParticles[i].Play();
        }
    }

    public void TurnOffLava()
    {
        if(lavaSound != null)
        {
            lavaSound.Stop();
        }

        for(int i = 0; i < lavaParticles.Length; i++)
        {
            lavaParticles[i].Stop();
        }

    }

    IEnumerator WaitToLoadHub()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Start");
    }
}
