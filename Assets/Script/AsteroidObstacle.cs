
using UnityEngine;

public class AsteroidObstacle : MonoBehaviour
{
    [Header("Asteroid Settings")]
    [SerializeField] private float health = 3f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 35f;
    [SerializeField] private int scoreReward = 25;
    [SerializeField] private int collisionDamage = 1;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        moveSpeed = Random.Range(0.8f, 2.2f);
        rotationSpeed = Random.Range(-50f, 50f);
    }

    private void Update()
    {
        // Drift downward through the gameplay area.
        transform.Translate(
            Vector3.down * moveSpeed * Time.deltaTime,
            Space.World
        );

        // Rotate naturally in space.
        transform.Rotate(
            Vector3.forward * rotationSpeed * Time.deltaTime
        );

        // Remove the asteroid after it leaves the screen.
        if (mainCamera != null &&
            mainCamera.WorldToViewportPoint(transform.position).y < -0.15f)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        // Add score integration here if your GameManager
        // has a public method for adding score.
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calls PlayerHealth.TakeDamage(int) if that method
            // exists on the player or one of its components.
            other.SendMessage(
                "TakeDamage",
                collisionDamage,
                SendMessageOptions.DontRequireReceiver
            );

            Destroy(gameObject);
        }
    }
}
