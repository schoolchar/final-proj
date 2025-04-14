using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    public AudioSource pauseMusic; // Background music, just make it the cave sounds

    public bool isPaused = false;
    private AudioSource[] allAudioSources; // Stores all game audio sources
    
    //uis
    public GameObject howplay;
    public GameObject cheat;



    //Button indexes
    //0 - how to play
    //1 - cheats
    //2 - exit game
    //3 - main menu
    //4 - try again
    //5 - exit options
    //6 - speedrun
    //7 - invinsibility
    //8 - exit cheats
    //9 - exit how to play
    //10 - exit game


    [Header("Variables for making the pause menu work w controller")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] buttonImg;
    [SerializeField] private Color chooseColorFullButton;
    [SerializeField] private Color blankColorFullButton;
    bool opsOpen;
    bool how2PlayOpen;
    bool cheatsOpen;
    bool canInput;
    int buttonIndex;

    void Start()
    {
        isPaused = false;

        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume game time

        allAudioSources = FindObjectsOfType<AudioSource>(); // Get all audio sources
    }

    void Update()
    {
        // Toggle pause when pressing escape
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            howplay.SetActive(false);
            cheat.SetActive(false);
        }

        
    }

    // Resumes the game
    public void ResumeGame()
    {
        isPaused = false;

        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume game time

        // Resume all paused sounds
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != pauseMusic && !audio.isPlaying) // Avoid resuming pause music
            {
                audio.UnPause();
            }
        }
        if (pauseMusic != null)
            pauseMusic.Stop(); // Stop pause music
        
    }

    // Pauses the game
    public void PauseGame()
    {
        isPaused = true;
        buttonIndex = 0;
        //Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Freeze game time

        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != pauseMusic && audio.isPlaying) // Avoid pausing pause music
            {
                audio.Pause();
            }
        }

        if (pauseMusic != null) 
        pauseMusic.Play(); // Start playing pause menu music
        

       
    }

    // For quitting the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }



    void ControllerInput()
    {
        
    }


    void RevertColor()
    {
        Debug.Log("revert on " + (buttonIndex - 2));
        buttonImg[buttonIndex].color = blankColorFullButton;
    }

    void ChangeImage()
    {
        Debug.Log("change on " + (buttonIndex - 2));
        buttonImg[buttonIndex].color = chooseColorFullButton;
    }

    IEnumerator CanClick()
    {
        canInput = false;
        yield return new WaitForSeconds(0.3f);
        canInput = true;
    }
}
