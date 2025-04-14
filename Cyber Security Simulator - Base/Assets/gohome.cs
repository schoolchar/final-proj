using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fun : MonoBehaviour
{
    public void PlayGame()
    {
        Destroy(FindAnyObjectByType<gameManager>().gameObject);
        SceneManager.LoadSceneAsync("Hub");
    }
}