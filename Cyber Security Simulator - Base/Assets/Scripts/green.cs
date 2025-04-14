using UnityEngine;

public class BoxColliderColorChange : MonoBehaviour
{
    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
    }
}