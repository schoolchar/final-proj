using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject objectToDeactivate; 
    public GameObject objectToActivate;   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            objectToDeactivate.SetActive(false);

           
            objectToActivate.SetActive(true);
        }
    }
}