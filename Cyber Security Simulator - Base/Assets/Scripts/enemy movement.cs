using UnityEngine;
using UnityEngine.AI;

public class enemyMovement : MonoBehaviour
{
    public Transform patrolRoute; // Parent containing waypoints
    public Transform player; // Player reference
    public GameObject bulletPrefab; // Bullet prefab
    public Transform bulletSpawnPoint; // Where the bullet spawns
    public float shootingRange = 10f; // The range within which the enemy can shoot
    public float maintainDistance = 5f; // The distance the enemy tries to maintain from the player
    public float fireRate = 1f; // Time between shots
    private float nextFireTime = 0f;
    private NavMeshAgent agent;
    private Transform[] locations;
    private int currentLocation = 0;
    private bool playerInRange = false;
    public Animator animator;
    public int PlayerBullet;
    public GameObject playerBullet;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
        animator.SetBool("Movement", true);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            playerInRange = true;
            MaintainDistanceFromPlayer();
            ShootPlayer();
        }
        else
        {
            playerInRange = false;
            if (!agent.pathPending && agent.remainingDistance < 0.2f)
            {
                MoveToNextPatrolLocation();
            }
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (locations.Length == 0) return;
        agent.SetDestination(locations[currentLocation].position);
        currentLocation = (currentLocation + 1) % locations.Length;
    }

    void InitializePatrolRoute()
    {
        locations = new Transform[patrolRoute.childCount];
        for (int i = 0; i < patrolRoute.childCount; i++)
        {
            locations[i] = patrolRoute.GetChild(i);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            Debug.Log("Player detected");
            playerInRange = true;
        }
        else if (other.gameObject.name == playerBullet.name + "(Clone)")
        {
            gameManager.instance.EnemyKilled(); //increase variable to win game
            Destroy(gameObject); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "player")
        {
            Debug.Log("Player out of range");
            playerInRange = false;
            MoveToNextPatrolLocation();
        }
    }

    void MaintainDistanceFromPlayer()
    {
        Vector3 directionToPlayer = (transform.position - player.position).normalized;
        Vector3 targetPosition = player.position + directionToPlayer * maintainDistance;
        agent.SetDestination(targetPosition);
    }

    void ShootPlayer()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = (player.position - bulletSpawnPoint.position).normalized * 20f; // Adjust bullet speed as necessary
            Destroy(bullet, 2f); // Destroy bullet after 2 seconds to avoid clutter
        }
    }
}