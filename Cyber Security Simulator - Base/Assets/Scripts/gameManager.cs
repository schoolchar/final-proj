using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Game manager
/// Manages options, scene loading, player deaths
/// </summary>
public class gameManager : MonoBehaviour
{
    public enum RoboState
    {
        HUMAN,
        HUMANROBO,
        ROBOT
    }


    [Header("Deaths and enemies killed")]
    public static gameManager instance;
    public int enemiesKilled;
    public int totalHealth = 3;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI healthText;
    public TMP_Text deathCount;
    private bool canLoseHealth = true;

    [Header("Options")]
    //Bools for options, toggled by buttons on options menu
    public bool invincibility;
    public bool speedrun;
    public GameObject inOff;
    public GameObject inOn;
    public GameObject sOff;
    public GameObject sOn;    

    //Put invincibility specific variables here vvv

    //Speedrun specific variables
    [Header("Speed run variables")]
    [SerializeField] private float timeToBeat = 240;
    public TextMeshProUGUI timerText;
    public bool setSpeedrunFromMain;
    private float timeLeft = 240;
    private int hubVisitCount = 0;
    private bool startTimer;


    [Header("LevelsComplete")]
    public bool combat;
    public bool parkour;
    public bool escapeRoom;

    [Header("Unlocks")]
    public bool combatUnlocked; //shooting
    public bool parkourUnlocked; //grapple
    public bool escapeRoomUnlocked; //wallrun
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI grappleText;
    public TextMeshProUGUI wallrunText;
    private int deaths = 0;

    [Header("Player Models")]
    public PlayerMovement playerM;
    public RoboState roboState;


    [Header("New Unlocks")]
    public bool combatWon;
    public bool parkourWon;
    public bool escapeRoomWon;
    public bool haswon;

    #region Monobehaviours
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        combatUnlocked = false;
        parkourUnlocked = false;
        escapeRoomUnlocked = false;
        haswon = false;
    }
    void Start()
    {

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Hub")
        {
            inOff.SetActive(true);
            inOn.SetActive(false);
            sOff.SetActive(false);
            sOn.SetActive(false);
        }
        invincibility = false;
        enemiesKilled = 0; // how many enemies killed
        roboState = RoboState.HUMAN;
        //InitOnLoad();
    }


    private void Update()
    {
        //winning
        if (combatWon == true && parkourWon == true && escapeRoomWon == true && haswon == false)
        {
            SceneManager.LoadSceneAsync("win");
            haswon = true;
        }

        //Health
        //gets scene name
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Hub")
        {
            invincibilityOn();
            speedRunOn();
        }


        
        if (totalHealth == 0)
        {
            IncrementDeaths();
            if (currentScene.name == "Combat")
            {
                enemiesKilled = 0;
                combatUnlocked = true;
            }
            if (currentScene.name == "EscapeRoomTest")
            {
                escapeRoomUnlocked = true;
            }
            if (currentScene.name == "Parkour")
            {
                parkourUnlocked = true;
            }
            SceneManager.LoadScene(currentScene.name);
            totalHealth = 3;
        }

        if (enemiesKilled == 5)
        {
            combatWon = true;
            SceneManager.LoadSceneAsync("Start");
            enemiesKilled = 0;
        }

        if (currentScene.name != "Hub")
        {
            enemiesKilledText.text = "Enemies Killed: " + enemiesKilled;
            healthText.text = totalHealth + "/3";
        }

        //Timer/speedrun
        if (startTimer)
        {
            Timer();
        }


    }
    #endregion

    /// <summary>
    /// Call every time a new scene is created, assigns UI elements
    /// </summary>
    public void InitOnLoad()
    {
        //Assign text mesh for enemies killed, health, and speedrun timer

          enemiesKilledText = GameObject.FindGameObjectWithTag("EnemiesKilledText").GetComponent<TextMeshProUGUI>();
          healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
          timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TextMeshProUGUI>();

          //Assign skills that are online for the text to show this
          gunText = GameObject.FindGameObjectWithTag("GunStatusText").GetComponent<TextMeshProUGUI>();
          grappleText = GameObject.FindGameObjectWithTag("GrappleStatusText").GetComponent<TextMeshProUGUI>();
          wallrunText = GameObject.FindGameObjectWithTag("WallrunStatusText").GetComponent<TextMeshProUGUI>();

        // Death counter
        deathCount = GameObject.FindGameObjectWithTag("DeathCounterText").GetComponent<TextMeshProUGUI>();


        if (SceneManager.GetActiveScene().name == "Start" && hubVisitCount == 0)
        {
            if (setSpeedrunFromMain)
            {
                timerText.enabled = true;
                speedrun = true;
                startTimer = true;
                hubVisitCount = 1;
            }
        }

        deathCount.text = "Deaths: " + deaths;

        Color green = Color.green;
        Color red = Color.red;

        //Combat skill unlocked?
        if (combatUnlocked)
        {
            gunText.text = "Online";
            gunText.color = green;
        }
        else
        {
            gunText.text = "Offline";
            gunText.color = red;
        }

        //Grapple skill unlocked?
        if (parkourUnlocked)
        {
            grappleText.text = "Online";
            grappleText.color = green;
        }
        else
        {
            grappleText.text = "Offline";
            grappleText.color = red;
        }

        //Wallrun skill unlocked?
        if (escapeRoomUnlocked)
        {
            wallrunText.text = "Online";
            wallrunText.color = green;
        }
        else
        {
            wallrunText.text = "Offline";
            wallrunText.color = red;
        }

       

        //FindUI();

    } //END InitOnLoad()

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
    public void IncrementDeaths()
    {
        deaths++;
        deathCount.text = "Deaths: " + deaths;
        Debug.Log("Death count = " + deaths);
        if(deaths >= 2 && roboState == RoboState.HUMAN)
        {
            roboState = RoboState.HUMANROBO;
            playerM.ChangePlayerModel();
        }

        if(deaths >= 5 && roboState == RoboState.HUMANROBO)
        {
            
            roboState= RoboState.ROBOT;
            playerM.ChangePlayerModel();
        } 
    }
    public int GetDeathCount()
    {
        return deaths;
    }

    #region Turn on options
    /// <summary>
    /// Called when speedrun option button is pressed
    /// </summary>
    public void ChangeSpeedrun()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            setSpeedrunFromMain = !setSpeedrunFromMain;
            return;
        }

        speedrun = !speedrun;

        if(speedrun)
        {
            startTimer = true;

            timerText.enabled = true;
        }
        else
        {
            startTimer = false;

            timerText.enabled = false;
            timeLeft = 240;
        }
        
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
        Debug.Log("Time = " + Time.deltaTime + "Time to beat = " + timeLeft);
        timeLeft -= Time.deltaTime;

        //Round down for text, show on UI
        int _timeRounded = (int)timeLeft;

        //Null check for debugging, but all scenes should ultimately have the Ui in
        if (timerText != null)
        {
            timerText.text = "Time left: " + _timeRounded.ToString();
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

    #region invincibility 
    public void invincibilityOn()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (invincibility == true)
        {
            combatUnlocked = true;
            parkourUnlocked = true;
            escapeRoomUnlocked = true;
            totalHealth = 3;

            inOff.SetActive(false);
            inOn.SetActive(true);
        }
        else if (invincibility == false)
        {
            inOff.SetActive(true);
            inOn.SetActive(false);
        }
    }

    public void speedRunOn()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (speedrun == true || setSpeedrunFromMain == true)
        {
            sOff.SetActive(false);
            sOn.SetActive(true);
        }
        else if (speedrun == false && setSpeedrunFromMain == false)
        {
            sOff.SetActive(true);
            sOn.SetActive(false);
        }
    }
    #endregion
}