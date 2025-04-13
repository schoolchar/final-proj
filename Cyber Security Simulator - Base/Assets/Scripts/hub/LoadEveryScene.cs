using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEveryScene : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Supposed to assign");
        FindAnyObjectByType<gameManager>().InitOnLoad();
    }
}
