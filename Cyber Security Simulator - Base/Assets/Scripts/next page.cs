using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    public GameObject objectToDeactivate; 
    public GameObject objectToActivate;   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            objectToDeactivate.SetActive(false);

           
            objectToActivate.SetActive(true);
        }
    }
}