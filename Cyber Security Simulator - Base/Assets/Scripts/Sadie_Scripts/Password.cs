using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    public bool passwordActive;
    public bool passwordSolved;
    

    private string password = "C4FF13N3";
    

    public TMP_InputField passwordInput;
    public GameObject passwordInputObj;
    public GameObject deactivateSecurity;
    [SerializeField] private RawImage inputInstructions;
    [SerializeField] private RawImage pWordInstructions;
    [SerializeField] private TextMeshProUGUI moreInstructions;
    [SerializeField] private RawImage wrongPword;
    [SerializeField] private RawImage background;
    [SerializeField] private Sprite desktopTxture;

    

    [Header("Controller input")]
    [SerializeField] private string currentInput;
    private bool canInput = true; //When we convert to new input system, if we do, this will not be necessary

    [Header("SFX")]
    [SerializeField] private AudioClip login;
    [SerializeField] private AudioClip wrongPassword;
    [SerializeField] private AudioSource computerSound;

    private void Start()
    {
        passwordInput.onEndEdit.AddListener(CheckPassword);
        passwordInput.onValueChanged.AddListener(GetCurrentString);
        passwordInput.text = "A";
        passwordInput.ActivateInputField();
    }

    private void Update()
    {
        CheckControllerTyping();
    }

    /// <summary>
    /// Activated on player submitting what they type, checks password
    /// </summary>
    public void CheckPassword(string _input)
    {
        _input = _input.ToUpper();

        //If password is correct
        if(_input == password)
        { 
            //Show button for deactivating cameras/turning on lava
            Debug.Log("Password correct");
            computerSound.clip = login;
            computerSound.Play();
            passwordSolved = true;
            passwordActive = false;
            deactivateSecurity.SetActive(true);
            passwordInput.enabled = false;
            passwordInputObj.SetActive(false);
            inputInstructions.enabled = false;
            pWordInstructions.enabled = false;
            moreInstructions.enabled = false;
            wrongPword.enabled = false;


            background.texture = desktopTxture.texture;

            Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;


        }
        //If password is incorrect
        else
        {
            Debug.Log("Wrong password");
            computerSound.clip = wrongPassword;
            computerSound.Play();
            passwordInput.text = "";
            wrongPword.enabled = true;
            StartCoroutine(WrongPasswordStopShowing());
        }
    }//END CheckPassword()

    /// <summary>
    /// Allow controller use for typing in input field
    /// </summary>
    void CheckControllerTyping()
    {
        //Change the current letter
        if(Input.GetAxis("Dpad Vertical") != 0 && canInput)
        {
            canInput = false;
            StartCoroutine(StopTyping());
            char[] _newInput = new char[currentInput.Length];
            //Loop through string in input field to get to the last letter
            for(int i = 0; i < currentInput.Length; i++)
            {
                //On last letter
                if(i == currentInput.Length - 1)
                {
                    //Use dpad to increase or decrease ascii value of last letter in string, simulates a scroll mechanic
                    _newInput[i] = (char)((int)(currentInput[i]) + Input.GetAxis("Dpad Vertical"));
                }
                else
                {
                    _newInput[i] = currentInput[i];
                }
                
            }
            //Replace
            string _s = new string(_newInput);
            passwordInput.text = _s;
        }

        //Move to next letter
        if(Input.GetAxis("Dpad Horizontal") == 1 && canInput)
        {
            canInput = false;
            StartCoroutine(StopTyping());
            char[] _newInput = new char[currentInput.Length + 1];
            //Replace input field with same letters
            for (int i = 0; i < currentInput.Length; i++)
            {
                
                _newInput[i] = currentInput[i];

            }
            //Append default A to the end of the input field string
            _newInput[_newInput.Length - 1] = 'A';
            string _s = new string(_newInput);
            passwordInput.text = _s;
        }

        //Backspace
        if(Input.GetAxis("Dpad Horizontal") == -1 && canInput)
        {
            canInput = false;
            StartCoroutine(StopTyping());
            char[] _newInput = new char[currentInput.Length -1];
            for(int i = 0; i < _newInput.Length; i++)
            {
                //If the loop has reached the last letter, change to default A
                if (i == _newInput.Length - 1)
                {
                    _newInput[i] = 'A';
                }
                else
                {
                    _newInput[i] = currentInput[i];
                }
            }
            string _s = new string(_newInput);
            passwordInput.text = _s;
        }

        //Submit
        if(Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            CheckPassword(passwordInput.text);
        }
    }//END CheckCOntrollerTyping()

    /// <summary>
    /// Get whatever is currently in the input field
    /// </summary>
    public void GetCurrentString(string _input)
    {
        currentInput = _input;
        
    } //END GetCurrentString()

    /// <summary>
    /// Acts as get key down for dpad
    /// </summary>
    IEnumerator StopTyping()
    {
        yield return new WaitForSeconds(0.3f);
        canInput = true;
    } //END StopTyping()

    IEnumerator WrongPasswordStopShowing()
    {
        yield return new WaitForSeconds(3);
        wrongPword.enabled = false;
    }

} //END Passwrod.cs
