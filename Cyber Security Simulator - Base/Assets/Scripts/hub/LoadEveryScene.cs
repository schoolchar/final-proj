using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadEveryScene : MonoBehaviour
{
    public gameManager manager;

    [Header("Need to assign game manager to")]
    [SerializeField] private PlayerShooting shooting;
    [SerializeField] private WallRunning wallRunning;
    [SerializeField] private grapple pGrapple;
    [SerializeField] private PlayerSlide slide;

    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI gunText;
    public TextMeshProUGUI grappleText;
    public TextMeshProUGUI wallruntext;

    public GameObject speedrunOff;
    public GameObject speedrunOn;
    public GameObject invincOffl;
    public GameObject invincOnl;


    private void Start()
    {
        AssignVariables();
       manager.InitOnLoad();
        slide.manager = manager;
    }

    
    
    public void CheatHolderSpeedrun()
    {
        Debug.Log("Speedrun clicked");
        manager.ChangeSpeedrun();
    }

    public void CheatHolderInvincibility()
    {
        manager.ChangeInvincibility();
    }

    public void LoadHubOnTryAgain()
    {
        SceneManager.LoadScene("Start");
    }

    void AssignVariables()
    {

        Debug.Log("Supposed to assign");
        manager = FindAnyObjectByType<gameManager>();

        //manager.InitOnLoad();

        manager.sOn = speedrunOn;
        manager.sOff = speedrunOff;
        manager.inOn = invincOnl;
        manager.inOff = invincOffl;

        shooting.manager = manager;
        wallRunning.manager = manager;
        pGrapple.manager = manager;
        

        manager.enemiesKilledText = enemiesKilledText;
        manager.healthText = healthText;

        if(timerText != null)
        manager.timerText = timerText;

        manager.gunText = gunText;
        manager.grappleText = grappleText;
        manager.wallrunText = wallruntext;

       

        if (SceneManager.GetActiveScene().name == "Start")
        {
            manager.totalHealth = 3;
            manager.healthText.text = manager.totalHealth + "/3";
        }
    }
}
