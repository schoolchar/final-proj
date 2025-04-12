using UnityEngine;

public class talk : MonoBehaviour
{
    
    public GameObject firsttime;
    public float startup;
    public float delayTime = 5f;

   
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (startup == 0f)
        {
            firsttime.SetActive(true);
            Invoke("DeactivateObject", delayTime); 
        }
        
    }

    void DeactivateObject()
    {
        if (firsttime != null)
        {
            firsttime.SetActive(false);
            startup++;
        }
    }
}