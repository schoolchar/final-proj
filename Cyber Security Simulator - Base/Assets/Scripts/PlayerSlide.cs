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
    public GameObject playerPhys;
    public bool debugMode;
    public Rigidbody rb;


    //gets game manager for unlocks
    public gameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        //finds game manager
        manager = FindAnyObjectByType<gameManager>();

        if (manager.escapeRoomUnlocked == true)
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
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton5)) && canSlide) //&& displayDeaths.GetDeathCount() >= 3
        {
            currentlySliding = true;
            StartCoroutine(EndSlide());

        }

        if (currentlySliding)
        {
            //Add forward force to slide
            //Vector3 direction = (slideDirection.transform.position - transform.position).normalized;
            rb.AddForce(playerPhys.transform.forward * slideForce, ForceMode.Impulse);
        }
    } //END Slide()

    private IEnumerator EndSlide()
    {
        yield return new WaitForSeconds(timeSlide);
        currentlySliding = false;
    }
}
