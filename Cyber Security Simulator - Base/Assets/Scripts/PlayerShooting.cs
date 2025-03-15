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

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Left mouse button or controller equivalent
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
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

    private void Start()
    {
        Destroy(gameObject, projectileLifetime); // Destroy projectile after set time
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);
        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(shootDamage); // Damage enemy
            }
        }
        Destroy(gameObject); // Destroy projectile on collide
    }
}
