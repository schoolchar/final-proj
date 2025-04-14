using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressButton : MonoBehaviour
{
    [SerializeField] private AudioSource buttonPressSFX;
    gameManager gameManagerScript;

    private void Start()
    {
        gameManagerScript = FindAnyObjectByType<gameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            PressStupidButton();
            buttonPressSFX.Play();
        }
    }


    /// <summary>
    /// Disable functionality for level, let player has a free pass
    /// </summary>
    public void PressStupidButton()
    {
        gameManagerScript.escapeRoomWon = true;
        StartCoroutine(WaitToLoadHub());
       
    } //END PressStupidButton()

    IEnumerator WaitToLoadHub()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Start");
    }


}
