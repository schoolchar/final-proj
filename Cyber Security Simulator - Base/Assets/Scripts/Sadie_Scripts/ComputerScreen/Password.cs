using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    private string password = "C4ff13n3"; 

    public TMP_InputField passwordInput;
    public GameObject passwordInputObj;
    public Button deactivateSecurity;

    private void Start()
    {
        passwordInput.onEndEdit.AddListener(CheckPassword);
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

} //END Passwrod.cs
