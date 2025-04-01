using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    private string password = "password"; //temp password

    public TMP_InputField passwordInput;
    public GameObject passwordInputObj;
    public Button deactivateSecurity;

    private void Start()
    {
        passwordInput.onEndEdit.AddListener(CheckPassword);
    }

    public void CheckPassword(string _input)
    {
        if(_input == password)
        {
            Debug.Log("Password correct");
            deactivateSecurity.enabled = true;
            passwordInput.enabled = false;
        }
        else
        {
            Debug.Log("Wrong password");
            passwordInput.text = "";
        }
    }
}
