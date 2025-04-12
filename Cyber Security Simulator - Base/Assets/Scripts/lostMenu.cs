using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lostMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void lostReset()
    {
        SceneManager.LoadSceneAsync("Hub");
    }
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
