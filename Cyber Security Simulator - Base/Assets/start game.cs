using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class mainMenu : MonoBehaviour
{
    //Button indexes 
    //0 - play
    //1 - options
    //2 - how to play
    //3 - cheats
    //4 - exit options
    //5 - speedrun
    //6 - invinsibility
    //7 - exit cheats
    //8 - exit how to play
    //9 - exit game



    int buttonIndex = 0;
    bool canInput = true;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button howToPlayB;
    [SerializeField] private Button cheatsB;
    [SerializeField] private Button exitGameB;
    [SerializeField] private Button exitOptions;
    [SerializeField] private Button speedrunB;
    [SerializeField] private Button invincibilityB;
    [SerializeField] private Button exitHow2PlayB;
    [SerializeField] private Button exitCheatsB;

    [Header("UI for controller input")]
    [SerializeField] private Image playButton;
    [SerializeField] private Image opButton;
    [SerializeField] private Color chooseColor;
    [SerializeField] private Color blankColor;
    [SerializeField] private Color chooseColorFullButton;
    [SerializeField] private Color blankColorFullButton;
    [SerializeField] private Image[] menuButtonImg;

    //What is active?
    bool optionsOpen;
    bool howToPlay;
    bool cheats;

    private void Start()
    {
        playButton.color = chooseColor;
    }

    private void Update()
    {
        //Controller
        ControllerInput();
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Start");
    }


    void ControllerInput()
    {
        //Switch bw play and options
        if(Input.GetAxis("Dpad Vertical") == 1 && canInput && buttonIndex == 1)
        {
            buttonIndex = 0;
            playButton.color = chooseColor;
            opButton.color = blankColor;
            StartCoroutine(CanClick());
        }
        else if(Input.GetAxis("Dpad Vertical") == -1 && canInput && buttonIndex == 0)
        {
            
            buttonIndex = 1;
            playButton.color = blankColor;
            opButton.color = chooseColor;
            StartCoroutine(CanClick());
        }

        //Switch vertical on options
        if (Input.GetAxis("Dpad Horizontal") != 0 && canInput && optionsOpen) //2 - 4 + 9
        {
            Debug.Log("Read from horizontal ops open");
            float _i = Input.GetAxis("Dpad Horizontal");

            if (_i < 0)
            {
                if (buttonIndex - 1 >= 2)
                {
                    Debug.Log("Less than 2");
                    if(buttonIndex == 9)
                    {
                        RevertColor();
                        buttonIndex = 4;
                        ChangeImage();
                    }
                    else
                    {
                        RevertColor();
                        buttonIndex += -1;
                        ChangeImage() ;
                    }
                    
                }
            }
            else if (_i > 0)
            {
                if (buttonIndex  == 4)
                {
                    RevertColor();
                    buttonIndex = 9;
                    ChangeImage();
                }

               else  if (buttonIndex < 4)
                {
                    RevertColor();
                    buttonIndex += 1;
                    ChangeImage();
                }
            }
           

            

            StartCoroutine(CanClick());
        }

        //Switch vertical on cheat menus
        if (Input.GetAxis("Dpad Horizontal") == 1 && canInput && cheats) //5-7
        {
            float _i = Input.GetAxis("Dpad Horizontal");

           if(_i < 0)
            {
                if(buttonIndex - 1 >= 5)
                {
                    RevertColor();
                    buttonIndex += -1;
                    ChangeImage();
                }
            }
           else if(_i > 0)
            {
                if (buttonIndex + 1 <= 7)
                {
                    RevertColor();
                    buttonIndex += 1;
                    ChangeImage();
                }
            }

            Debug.Log(buttonIndex);

            StartCoroutine(CanClick());
        }




        //Press a key
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if(buttonIndex == 0) //Click play
            {
                PlayGame();
            }
            else if(buttonIndex == 1) //Click options
            {
                optionsButton.onClick.Invoke();

                buttonIndex = 2;
                howToPlay = false;
                cheats = false;
                optionsOpen = true;
                ChangeImage();
            }
            else if(buttonIndex == 2) //Click how to play on options menu
            {
                howToPlayB.onClick.Invoke();
                RevertColor();
                buttonIndex = 8;
                howToPlay = true;
                optionsOpen=false;
                ChangeImage() ;
            }
            else if(buttonIndex == 3) //Click cheats on optioins menu
            {
                cheatsB.onClick.Invoke();
                RevertColor() ;
                buttonIndex = 5;
                cheats = true;
                optionsOpen=false;
                ChangeImage() ;
            }
            else if(buttonIndex == 4) //Click X on how to play menu
            {
                
                Application.Quit();

            }
            else if(buttonIndex == 5) //Click speedrun on cheats menu
            {
                speedrunB.onClick.Invoke();
            }
            else if(buttonIndex == 6) //Click invincibility on cheats menu
            {
                invincibilityB.onClick.Invoke();
            }
            else if(buttonIndex == 7) //Click X on cheats menu
            {
                exitCheatsB.onClick.Invoke();
                RevertColor();
                buttonIndex = 3;
                cheats = false;
                optionsOpen = true;
                ChangeImage() ;
            }
            else if(buttonIndex == 8) //Click X on how to play menu
            {
                exitHow2PlayB.onClick.Invoke();
                RevertColor();
                buttonIndex = 2;
                howToPlay = false;
                optionsOpen = true;
                ChangeImage();
            }
            else if(buttonIndex == 9) //Click Exit game on options menu
            {
                exitOptions.onClick.Invoke();
                buttonIndex = 1;
            }
          
            
        }

        Debug.Log(buttonIndex);

    }

    void RevertColor()
    {
        Debug.Log("revert on " + (buttonIndex - 2));
        menuButtonImg[buttonIndex - 2].color = blankColorFullButton;
    }

    void ChangeImage()
    {
        Debug.Log("change on " + (buttonIndex - 2));
        menuButtonImg[buttonIndex - 2].color = chooseColorFullButton;
    }

    //Wait for input
    IEnumerator CanClick()
    {
        canInput = false;
        yield return new WaitForSeconds(0.3f);
        canInput = true;
    }
}