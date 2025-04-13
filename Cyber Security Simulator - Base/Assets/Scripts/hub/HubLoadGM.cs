using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubLoadGM : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    void Awake()
    {
        if(FindAnyObjectByType<gameManager>() == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }

}
