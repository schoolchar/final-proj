using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public int enemiesKilled;
    public int totalHealth = 3;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI healthText;
    private bool canLoseHealth = true;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        enemiesKilled = 0; // how many enemies killed
    }

    public void EnemyKilled()
    {
        enemiesKilled++; //adds 1 to enemies killed
    }
    public void healthLose()
    {
        if (canLoseHealth)
        {
            StartCoroutine(HealthLossCooldown());
        }
    }

    private IEnumerator HealthLossCooldown()
    {
        canLoseHealth = false;

        Debug.Log("Health before: " + totalHealth);
        totalHealth -= 1;
        Debug.Log("Health after: " + totalHealth);
        healthText.text = totalHealth + "/3";

        yield return new WaitForSeconds(0.5f); // Wait for 0.5 second

        canLoseHealth = true;
    }
    void Update()
    {
        if (enemiesKilled == 5)
        {
            SceneManager.LoadSceneAsync("win");
        }
        if (totalHealth == 0)
        {
            SceneManager.LoadSceneAsync("lost");
        }
        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled;
        healthText.text = totalHealth + "/3";
    }
}