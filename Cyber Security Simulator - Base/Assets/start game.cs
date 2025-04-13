using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{

    private void Update()
    {
        //Controller
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Start");
    }


    void ControllerInput()
    {

    }
}