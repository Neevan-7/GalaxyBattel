
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 5f;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.5f;

    [Header("Crystal Drop")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField, Range(0f, 1f)] private float crystalDropChance = 0.25f;
    [SerializeField] private int crystalCoinValue = 10;

    private bool deathHandled;

    private Rigidbody2D rb;
    private Transform player;
    private float nextFireTime;
    private Vector2 moveDirection;


    private float baseMoveSpeed;
    private float baseFireRate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
        baseFireRate = fireRate;
    }

    private void Start()
    {
        //baseMoveSpeed = moveSpeed;
        //baseFireRate = fireRate;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError(
                "EnemyAI: No GameObject tagged Player found.",
                this
            );
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        Vector2 direction =
            (Vector2)(player.position - transform.position);

        float distance = direction.magnitude;

        if (distance > 0.001f)
        {
            direction.Normalize();

            // Face the player. Assumes the enemy nose points up.
            float angle =
                Mathf.Atan2(direction.y, direction.x)
                * Mathf.Rad2Deg - 90f;

            rb.MoveRotation(angle);
        }

        // Chase until the enemy reaches attack range.
        if (distance > attackRange)
            moveDirection = direction;
        else
            moveDirection = Vector2.zero;

        // Fire when the player is within range.
        if (distance <= attackRange &&
            Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;
        if (GameManager.Instance != null &&
       GameManager.Instance.IsGameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "EnemyAI: Assign Projectile Prefab and FirePoint.",
                this
            );
            return;
        }

        Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );
    }


    public void StopAttacking()
    {
        moveDirection = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        enabled = false;
    }

    public void SetDifficulty(
     float speedMultiplier,
     float fireRateMultiplier)
    {
        moveSpeed = baseMoveSpeed * speedMultiplier;

        fireRate = Mathf.Max(
            0.5f,
            baseFireRate * fireRateMultiplier
        );
    }


    private void HandleDeath()
    {
        // Prevent duplicate rewards if death is triggered more than once.
        if (deathHandled) return;
        deathHandled = true;

        // Roll for a crystal drop.
        if (crystalPrefab != null &&
            Random.value <= crystalDropChance)
        {
            Instantiate(
                crystalPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Notify the spawner exactly once.
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.EnemyDefeated();
        }

        Destroy(gameObject);
    }

}
