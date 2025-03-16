using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    private bool canSlide;
    private bool currentlySliding;
    [SerializeField] private float slideForce;
    private float timeSlide = 2f;
    public GameObject slideDirection;
    public bool debugMode;
    public Rigidbody rb;

    public DisplayDeaths displayDeaths; //added for death manager

    // Start is called before the first frame update
    void Start()
    {
        if (debugMode)
        {
            AllowSliding();
        }
    }
    public void AllowSliding()
    {
        canSlide = true;
    }//End AllowSliding()

    // Update is called once per frame
    void Update()
    {
        Slide();

    }
    private void Slide()
    {
        //TEMP input, up to change, this is for testing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide && displayDeaths.GetDeathCount() >= 3)
        {
            currentlySliding = true;
            StartCoroutine(EndSlide());

        }

        if (currentlySliding)
        {
            //Add forward force to slide
            Vector3 direction = (slideDirection.transform.position - transform.position).normalized;
            rb.AddForce(direction * slideForce, ForceMode.Impulse);
        }
    } //END Slide()

    private IEnumerator EndSlide()
    {
        yield return new WaitForSeconds(timeSlide);
        currentlySliding = false;
    }
}
