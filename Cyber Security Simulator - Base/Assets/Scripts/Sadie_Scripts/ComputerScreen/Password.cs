using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    public bool passwordActive;

    private string password = "C4ff13n3"; 

    public TMP_InputField passwordInput;
    public GameObject passwordInputObj;
    public Button deactivateSecurity;

    [Header("Controller input")]
    [SerializeField] private string currentInput;
    private bool canInput = true; //When we convert to new input system, if we do, this will not be necessary

    private void Start()
    {
        passwordInput.onEndEdit.AddListener(CheckPassword);
        passwordInput.onValueChanged.AddListener(GetCurrentString);
        passwordInput.text = "A";
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
        //If password is correct
        if(_input == password)
        {
            //Show button for deactivating cameras/turning on lava
            Debug.Log("Password correct");
            passwordActive = false;
            deactivateSecurity.enabled = true;
            passwordInput.enabled = false;
            passwordInputObj.SetActive(false);
        }
        //If password is incorrect
        else
        {
            Debug.Log("Wrong password");
            passwordInput.text = "";
        }
    }//END CheckPassword()

    void CheckControllerTyping()
    {
        //Change the current letter
        if(Input.GetAxis("Dpad Vertical") != 0 && canInput)
        {
            canInput = false;
            StartCoroutine(StopTyping());
            char[] _newInput = new char[currentInput.Length];
            for(int i = 0; i < currentInput.Length; i++)
            {
                if(i == currentInput.Length - 1)
                {
                    _newInput[i] = (char)((int)(currentInput[i]) + Input.GetAxis("Dpad Vertical"));
                }
                else
                {
                    _newInput[i] = currentInput[i];
                }
                
            }
            string _s = new string(_newInput);
            passwordInput.text = _s;
        }

        //Move to next letter
        if(Input.GetAxis("Dpad Horizontal") == 1 && canInput)
        {
            canInput = false;
            StartCoroutine(StopTyping());
            char[] _newInput = new char[currentInput.Length + 1];
            for (int i = 0; i < currentInput.Length; i++)
            {
                
                _newInput[i] = currentInput[i];

            }
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
    }

    public void GetCurrentString(string _input)
    {
        currentInput = _input;
        
    }

    IEnumerator StopTyping()
    {
        yield return new WaitForSeconds(0.3f);
        canInput = true;
    }

} //END Passwrod.cs
