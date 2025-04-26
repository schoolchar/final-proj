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

    [SerializeField] private Animator playerAnimator;

    gameManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<gameManager>();

        if(manager.roboState == gameManager.RoboState.HUMANROBO || manager.roboState == gameManager.RoboState.ROBOT)
        {
            manager.playerM.ChangePlayerModel();
            playerAnimator = manager.playerM.animator;
        }

        
    }

    private void OnCollisionEnter(Collision collision) //Change to enter later, stay rn to debug
    {
        KillPlayer(collision);
    }

    void KillPlayer(Collision _collision)
    {
        if (lava && _collision.gameObject.layer == 7)
        {
            playerAnimator.SetTrigger("Zap");
            StartCoroutine(WaitToKill());
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

    IEnumerator WaitToKill()
    {
        yield return new WaitForSeconds(8);
        // Increment the deaths count
        //displayDeaths.IncrementDeaths();
        // Teleport the player to begining of level
        //manager.healthLose();
        manager.escapeRoomUnlocked = true;
        manager.wallrunText.text = "Online";
        manager.wallrunText.color = Color.green;
        if (manager.totalHealth > 0)
            player.transform.position = respawnPt.position;
        else if (manager.totalHealth == 0)
        {
            StartCoroutine(WaitToLoadHub());
        }

        if (manager.roboState == gameManager.RoboState.HUMANROBO || manager.roboState == gameManager.RoboState.ROBOT)
        {
            manager.playerM.ChangePlayerModel();
            playerAnimator = manager.playerM.animator;
        }
    }
}
