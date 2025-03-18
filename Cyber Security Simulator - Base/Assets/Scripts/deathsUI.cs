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

    private int deaths = 0;

    void Start()
    {
        uiText.text = "Deaths: " + deaths;
        Wallrun.text = "Wallrun offline";
        Shoot.text = "Gun offline";
        Grapple.text = "Grapple offline";
    }

    private void Update()
    {
        if (deaths == 1)
        {
            Shoot.text = "Gun online";
        }
        if (deaths == 2)
        {
            Grapple.text = "Grapple online";
        }
        if (deaths == 3)
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
