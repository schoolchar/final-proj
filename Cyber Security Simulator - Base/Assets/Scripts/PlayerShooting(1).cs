using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera; // Assign your camera here
    public float shootDamage = 25f; // Damage per shot
    public LayerMask hitLayers; // Put the enemy layer here
    public GameObject projectilePrefab; // Projectile prefab
    public Transform firePoint; // Where the projectile is fired from, make an empty game object
    public float projectileSpeed = 50f; // Speed of the projectile
    public AudioSource shootSound; // Shooting sound effect
    public float projectileLifetime = 5f; // Time before the projectile gets destroyed

    public DisplayDeaths displayDeaths; //added for death manager

    public Animator animator;//added for animator

    //gets the gamemanager for unlocks
    public gameManager manager;

    public PauseMenu pauseMenu;

    private void Start()
    {
        //finds game manager
        manager = FindAnyObjectByType<gameManager>();
    }
    void Update()
    {
        
        if (Input.GetButton("Fire2") && manager.combatUnlocked == true && !pauseMenu.isPaused) // Right mouse is held
        {
            Debug.Log("Shoot" + pauseMenu.isPaused);
            if (Input.GetButtonDown("Fire1") && manager.combatUnlocked == true) // Left mouse pressed
            {
                ShootVerDos();
            }
        }
        else if (Input.GetButtonDown("Fire1") && manager.combatUnlocked == true && !pauseMenu.isPaused) // Left mouse pressed (No right mouse)
        {
            Debug.Log("Shoot" + pauseMenu.isPaused);
            Shoot();
        }
    }



    // Shoot ver 1, it's the normal shoot
    void Shoot()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }

        //animation
        animator.SetTrigger("Shoot");

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Add player velocity to projectile
            CharacterController playerController = GetComponent<CharacterController>();
            Vector3 playerVelocity = playerController != null ? playerController.velocity : Vector3.zero;

            rb.velocity = firePoint.forward * projectileSpeed + playerVelocity;
        }

        Projectile projectileScript = projectile.AddComponent<Projectile>();
        projectileScript.shootDamage = shootDamage;
        projectileScript.hitLayers = hitLayers;
        projectileScript.projectileLifetime = projectileLifetime;

        // Prevents projectile from hitting the player
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Shoot ver 2, it's the aimed shoot
void ShootVerDos()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }

        //plays animation
        animator.SetTrigger("Shoot");

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Raycast from the center of the camera forward
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint = ray.origin + ray.direction * 100f;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, hitLayers)) // If it hits something
        {
            targetPoint = hit.point;
        }

        // Set projectile direction
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        Projectile projectileScript = projectile.AddComponent<Projectile>();
        projectileScript.shootDamage = shootDamage;
        projectileScript.hitLayers = hitLayers;
        projectileScript.projectileLifetime = projectileLifetime;
    }
}

    public class Projectile : MonoBehaviour
{
    public float shootDamage;
    public LayerMask hitLayers;
    public float projectileLifetime;

    private Transform shooter; // Store the shoot transform

    public void SetShooter(Transform shooterTransform)
    {
        shooter = shooterTransform;
    }

    private void Start()
    {
        Destroy(gameObject, projectileLifetime);

        Collider shooterCollider = shooter?.GetComponent<Collider>();
        Collider myCollider = GetComponent<Collider>();
        if (shooterCollider != null && myCollider != null)
        {
            Physics.IgnoreCollision(myCollider, shooterCollider);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Ignore if object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            return; // Do nothing
        }

        Debug.Log("Yeehaw, bullet hit: " + other.gameObject.name);

        // Check if the object is an opp
        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(shootDamage); // Damage enemy
            }
        }

        Destroy(gameObject); // Destroy projectile
    }

}