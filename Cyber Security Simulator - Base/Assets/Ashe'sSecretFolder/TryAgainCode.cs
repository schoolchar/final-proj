using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainCode : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadSceneAsync("Start");
    }
}
