using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Game manager
/// Manages options, scene loading, player deaths
/// </summary>
public class gameManager : MonoBehaviour
{
    [Header("Deaths and enemies killed")]
    public static gameManager instance;
    public int enemiesKilled;
    public int totalHealth = 3;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI healthText;
    private bool canLoseHealth = true;

    [Header("Options")]
    //Bools for options, toggled by buttons on options menu
    public bool invincibility;
    public bool speedrun;

    //Put invincibility specific variables here vvv

    //Speedrun specific variables
    [Header("Speed run variables")]
    [SerializeField] private float timeToBeat;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeLeft;
    private int hubVisitCount;
    private bool startTimer;

    [Header("LevelsComplete")]
    public bool combat;
    public bool parkour;
    public bool escapeRoom;

    [Header("Unlocks")]
    public bool combatUnlocked;
    public bool parkourUnlocked;
    public bool escapeRoomUnlocked;

    #region Monobehaviours
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        combatUnlocked = false;
        parkourUnlocked = false;
        escapeRoomUnlocked = false;
}
    void Start()
    {
        enemiesKilled = 0; // how many enemies killed
    }


    private void Update()
    {
        //Health

        if (enemiesKilled == 5)
        {
            SceneManager.LoadSceneAsync("win");
            enemiesKilled = 0; 
        }
        if (totalHealth == 0)
        {
            SceneManager.LoadSceneAsync("lost");
        }
        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled;
        healthText.text = totalHealth + "/3";

        //Timer/speedrun
        if (startTimer)
        {
            Timer();
        }


    }
    #endregion

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
        if(healthText != null)
            healthText.text = totalHealth + "/3";

        yield return new WaitForSeconds(0.5f); // Wait for 0.5 second

        canLoseHealth = true;
    }
   
    #region Turn on options
    /// <summary>
    /// Called when speedrun option button is pressed
    /// </summary>
    public void ChangeSpeedrun()
    {
        speedrun = !speedrun;
    } //END ChangeSpeedrun()

    /// <summary>
    /// Called when invincibility option button is pressed
    /// </summary>
    public void ChangeInvincibility()
    {
        invincibility = !invincibility;
    } //END ChangeInvincibility()

    #endregion


    #region Speedrun mode methods
    /// <summary>
    /// Starts the timer for the game when the player first enters the hub
    /// Called upon entrance to the hub
    /// </summary>
    public void StartSpeedrunTimer()
    {
        if (SceneManager.GetActiveScene().name == "Hub" && speedrun)
        {
            hubVisitCount++;
        }

        if (hubVisitCount == 1)
        {
            startTimer = true;
        }
    } //END StartSpeedrunTimer()


    /// <summary>
    /// In speedrun mode, decreases timer, loads lose scene when time is out
    /// </summary>
    private void Timer()
    {
        timeLeft -= Time.deltaTime;

        //Round down for text, show on UI
        int _timeRounded = (int)timeLeft;

        //Null check for debugging, but all scenes should ultimately have the Ui in
        if (timerText != null)
        {
            timerText.text = _timeRounded.ToString();
        }

        if (timeLeft <= 0)
        {
            SceneManager.LoadScene("lost");
        }
    } //END Timer()

    /// <summary>
    /// Find text for timer in each scene
    /// </summary>
    public void FindUI()
    {
        timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TextMeshProUGUI>();

    } //END FindUI()
    #endregion
}