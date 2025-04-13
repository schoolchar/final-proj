using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checking : MonoBehaviour
{
    public gameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.combatUnlocked == false)
        {
            Debug.Log("false");
        }
        else if (manager.combatUnlocked == true)
        {
            Debug.Log("true");
        }

    }
}
