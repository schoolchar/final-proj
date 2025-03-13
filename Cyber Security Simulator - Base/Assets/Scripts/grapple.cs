using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple : MonoBehaviour
{
    // Reference to movement script
    private PlayerMovement pm;
    // Camera's location
    public Transform cam;
    // Tip of grappling gun for start of grapple happens, can be set so when we have model
    public Transform grapplingTip;
    // Sets which object can be grappled to, makes grapple points with layer
    public LayerMask Grappleable;
    // Distance for how far can grapple
    public float maxGrappleDistance;
    // Time after using grappling for you to actually be pulled
    public float grappleDelayer;
    // Location of grapple
    private Vector3 grapplePoint;
    // Reference to the line being drawn from grappling gun
    public LineRenderer lineRenderer;

    public float overshootYAxis;
    public float overshootXAxis;

    // Cooldown for when grappling hook can be used and amount of time
    public float grapplingCooldown;
    private float grapplingCooldownTime;

    // Key for grappling gun
    public KeyCode grappleKey = KeyCode.Mouse1;

    // If currently grappling or not
    private bool grappling;

    private void Start()
    {
        // Gets variables from player movement script
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Checks if key is pressed 
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        // Makes it so grappling cooldown is counting down
        if (grapplingCooldownTime > 0)
            grapplingCooldownTime -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lineRenderer.SetPosition(0, grapplingTip.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    private void StartGrapple()
    {
        // Checks if cooldown is active
        if (grapplingCooldownTime > 0) return;

        // Says player is currently grappling
        grappling = true;

        // Freezes player at start of grapple
        pm.freeze = true;

        // Debug: Log the camera position and forward direction
        Debug.Log("Camera Position: " + cam.position);
        Debug.Log("Camera Forward: " + cam.forward);

        // Get the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxGrappleDistance, Grappleable))
        {
            grapplePoint = hit.point;

            // Debug: Log the hit point
            Debug.Log("Grapple Hit Point: " + grapplePoint);

            // Calls grappling function with a delay set by grappleDelayer
            Invoke(nameof(ExecuteGrapple), grappleDelayer);
        }
        // If there was no grappling point in distance, stops the grapple after reaching that distance
        else
        {
            grapplePoint = ray.origin + ray.direction * maxGrappleDistance;

            // Debug: Log the max grapple point
            Debug.Log("Max Grapple Point: " + grapplePoint);

            Invoke(nameof(StopGrapple), grappleDelayer);
        }

        // Enables the drawn line and sets the end as where grappling gun hits
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, grapplingTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        // Unfreezes player
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        // Waits a second then stops grapple
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        // Says player isn't grappling
        grappling = false;

        // Sets cooldown for when player can grapple again
        grapplingCooldownTime = grapplingCooldown;

        // Deactivates drawn line
        lineRenderer.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}