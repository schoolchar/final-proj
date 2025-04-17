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


    // If currently grappling or not
    private bool grappling;

    //gets game manager for unlocks
    public gameManager manager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip grappleConnectSound;

    private void Start()
    {
        // Gets variables from player movement script
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Checks if key is pressed 
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton4)) && manager.parkourUnlocked == true) StartGrapple();

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
        if (grapplingCooldownTime > 0) return;

        // Freeze the player and mark as grappling
        grappling = true;
        pm.freeze = true;

        // Raycast from center of screen (crosshair)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Try direct hit
        if (Physics.Raycast(ray, out hit, maxGrappleDistance, Grappleable))
        {
            if (hit.collider.CompareTag("grapple"))
            {
                grapplePoint = hit.point;

                if (audioSource && grappleConnectSound)
                    audioSource.PlayOneShot(grappleConnectSound);

                Invoke(nameof(ExecuteGrapple), grappleDelayer);
            }
            else
            {
                TryAimAssist(ray);
                return;
            }
        }
        else
        {
            TryAimAssist(ray);
            return;
        }

        // Draw the line
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, grapplingTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    // This is aim assist for the grapple, using SphereCast
    private void TryAimAssist(Ray originalRay)
    {
        if (Physics.SphereCast(originalRay, 2f, out RaycastHit assistHit, maxGrappleDistance, Grappleable))
        {
            if (assistHit.collider.CompareTag("grapple"))
            {
                Debug.Log("AIM ASSIST snapped to: " + assistHit.collider.name);
                grapplePoint = assistHit.point;

                if (audioSource && grappleConnectSound)
                    audioSource.PlayOneShot(grappleConnectSound);

                Invoke(nameof(ExecuteGrapple), grappleDelayer);

                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, grapplingTip.position);
                lineRenderer.SetPosition(1, grapplePoint);

                grappling = true;
                pm.freeze = true;
                return;
            }
        }

        // Fallback: No hit at all, shoot straight out
        grapplePoint = originalRay.origin + originalRay.direction * maxGrappleDistance;
        Invoke(nameof(StopGrapple), grappleDelayer);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, grapplingTip.position);
        lineRenderer.SetPosition(1, grapplePoint);

        grappling = true;
        pm.freeze = true;
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