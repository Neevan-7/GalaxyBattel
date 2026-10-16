
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Crystal Drop")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField, Range(0f, 1f)]
    private float crystalDropChance = 0.25f;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.IsGameOver)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Enemy health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // Existing score/reward logic
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemyDefeated(100);
        }

        // Drop a crystal before destroying the enemy
        if (crystalPrefab != null &&
            Random.value < crystalDropChance)
        {
            Instantiate(
                crystalPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Existing wave completion logic
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.EnemyDefeated();
        }

        // Existing explosion effect

        if (BlastSpawner.Instance != null)
            BlastSpawner.Instance.SpawnBlast(transform.position);

        // Drop a random power-up before destroying the enemy.
        EnemyPowerUpDrop dropper = GetComponent<EnemyPowerUpDrop>();

        if (dropper != null)
            dropper.DropRandomPowerUp();

        Destroy(gameObject);
    }
}
