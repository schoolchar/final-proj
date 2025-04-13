using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad; 

    void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player")) 
        {
            // Load the scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}