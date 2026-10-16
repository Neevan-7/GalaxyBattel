
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatHUD : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthValue;

    [Header("Wave")]
    [SerializeField] private TMP_Text waveValue;
    [SerializeField] private TMP_Text enemiesRemainingValue;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreValue;
    [SerializeField] private TMP_Text enemiesDefeatedValue;

    private PlayerHealth playerHealth;
    private EnemySpawner enemySpawner;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();

        if (playerHealth == null)
            Debug.LogError("CombatHUD: PlayerHealth not found!");

        if (enemySpawner == null)
            Debug.LogError("CombatHUD: EnemySpawner not found!");

        UpdateHUD();
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        // Player health
        if (playerHealth != null)
        {
            int current = playerHealth.CurrentHealth;
            int maximum = playerHealth.MaxHealth;

            if (healthBar != null)
            {
                healthBar.maxValue = maximum;
                healthBar.value = current;
            }

            if (healthValue != null)
            {
                healthValue.text = current + " / " + maximum;
            }
        }

        // Current wave and remaining enemies
        if (enemySpawner != null)
        {
            if (waveValue != null)
            {
                waveValue.text =
                    "WAVE " +
                    enemySpawner.CurrentWave.ToString("00");
            }

            if (enemiesRemainingValue != null)
            {
                enemiesRemainingValue.text =
                    "HOSTILES: " +
                    enemySpawner.EnemiesRemaining.ToString("00");
            }
        }

        // Score and total kills
        if (GameManager.Instance != null)
        {
            if (scoreValue != null)
            {
                scoreValue.text =
                    "SCORE: " +
                    GameManager.Instance.Score.ToString("D6");
            }

            if (enemiesDefeatedValue != null)
            {
                enemiesDefeatedValue.text =
                    "KILLS: " +
                    GameManager.Instance.EnemiesDefeated.ToString("D2");
            }
        }
    }
}
