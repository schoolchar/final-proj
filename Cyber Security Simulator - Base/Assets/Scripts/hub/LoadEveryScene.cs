using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private void Start()
    {
        AssignVariables();
       manager.InitOnLoad();
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


        shooting.manager = manager;
        wallRunning.manager = manager;
        pGrapple.manager = manager;
        slide.manager = manager;

        manager.enemiesKilledText = enemiesKilledText;
        manager.healthText = healthText;

        if(timerText != null)
        manager.timerText = timerText;

        manager.gunText = gunText;
        manager.grappleText = grappleText;
        manager.wallrunText = wallruntext;
    }
}
