using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    public AudioSource pauseMusic; // Background music, just make it the cave sounds

    public bool isPaused = false;
    private AudioSource[] allAudioSources; // Stores all game audio sources
    
    //uis
    public GameObject howplay;
    public GameObject cheat;

    //Only for escape room
    [SerializeField] private Password password;

    //Button indexes
    //0 - how to play
    //1 - cheats
    //2 - exit game
    //3 - main menu
    //4 - try again
    //5 - speedrun
    //6 - invinsibility
    //7 - exit cheats
    //8 - exit how to play


    [Header("Variables for making the pause menu work w controller")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] buttonImg;
    [SerializeField] private Color chooseColorFullButton;
    [SerializeField] private Color blankColorFullButton;
    bool opsOpen;
    bool how2PlayOpen;
    bool cheatsOpen;
    bool canInput = true;
    int buttonIndex;

    public void LoadHub()
    {
        SceneManager.LoadScene("Start");
    }

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
                if(password != null)
                {
                    if(password.passwordActive)
                    {
                        return;
                    }
                }
                ResumeGame();
            }
            else
            {
                if (password != null)
                {
                    if (password.passwordActive)
                    {
                        return;
                    }
                }
                PauseGame();
            }
        }

        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ControllerInput();
        }
        else
        {
            /*Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/
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
        //howplay.SetActive(false);
       // cheat.SetActive(false);
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
        opsOpen = true;
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
        
        int i = 0;
        if (Input.GetAxis("Dpad Horizontal") != 0 && canInput)
        {
            Debug.Log("Pause controller called");

            if (Input.GetAxis("Dpad Horizontal") > 0)
            {
                i = 1;
            }
            else if(Input.GetAxis("Dpad Horizontal") < 0)
            {
                i = -1;
            }

            //Switch indexes
            if(opsOpen) //0-4
            {
                if(buttonIndex + i >= 0 && buttonIndex + i <=4)
                {
                    RevertColor();
                    buttonIndex += i;
                    ChangeImage();
                }
            }
            else if (how2PlayOpen) //8
            {

            }
            else if(cheatsOpen) //5-7
            {
                if (buttonIndex + i >= 6 && buttonIndex + i <= 8)
                {
                    RevertColor();
                    buttonIndex += i;
                    ChangeImage();
                }
            }
            StartCoroutine(CanClick());
            Debug.Log(buttonIndex +  " " + i);
        }

        //Click on buttons
        if(Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            buttons[buttonIndex].onClick.Invoke();
            if(buttonIndex == 0)
            {
                buttonIndex = 8;
                opsOpen = false;
                how2PlayOpen = true;
                ChangeImage() ;
            }
            else if(buttonIndex == 1)
            {
                buttonIndex = 6;
                opsOpen = false;
                cheatsOpen = true;
                ChangeImage() ;
            }
            else if(buttonIndex == 2)
            {
                //Quit game
            }
            else if(buttonIndex == 3)
            {
                //Go to main menu
            }
            else if(buttonIndex == 4)
            {
                //Reload scene
            }
            else if(buttonIndex == 5)
            {
                //Speedrun
            }
            else if(buttonIndex == 6)
            {
                //Invincibility
            }
            else if(buttonIndex == 7)
            {
                buttonIndex = 1;
                opsOpen = true;
                cheatsOpen = false;
                ChangeImage() ;
            }
            else if(buttonIndex == 8)
            {
                buttonIndex = 0;
                opsOpen = true;
                how2PlayOpen = false;
                ChangeImage() ;
            }
          
        }
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
        Debug.Log("Cannot input");
        yield return new WaitForSecondsRealtime(0.3f);
        Debug.Log("Can input again");
        canInput = true;
    }
}
