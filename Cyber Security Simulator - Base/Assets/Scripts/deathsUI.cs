using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDeaths : MonoBehaviour
{
    public TMP_Text uiText;

    //texts for displaying whats active
    public TMP_Text Wallrun;
    public TMP_Text Shoot;
    public TMP_Text Grapple;

    //gets game manager for unlocks
    public gameManager manager;


    private int deaths = 0;

    void Start()
    {
        //finds game manager
        manager = FindAnyObjectByType<gameManager>();

        uiText.text = "Deaths: " + deaths;
        Wallrun.text = "Wallrun offline";
        Shoot.text = "Gun offline";
        Grapple.text = "Grapple offline";
    }

    private void Update()
    {
        if (manager.combatUnlocked == true)
        {
            Shoot.text = "Gun online";
        }
        if (manager.parkourUnlocked == true)
        {
            Grapple.text = "Grapple online";
        }
        if (manager.escapeRoomUnlocked == true)
        {
            Wallrun.text = "Wallrun online";
        }
    }
    public void IncrementDeaths()
    {
        deaths++;
        uiText.text = "Deaths: " + deaths;
    }
    public int GetDeathCount()
    {
        return deaths;
    }
}
