using UnityEngine;

public class closePage : MonoBehaviour
{
    public GameObject turnoff;
    public GameObject turnoff2;
    public GameObject turnon;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            turnoff.SetActive(false);

            turnoff2.SetActive(false);


            turnon.SetActive(true);
        }
    }
}