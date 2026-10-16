
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }

    public int Score { get; private set; }
    public int EnemiesDefeated { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        IsGameOver = false;
        Score = 0;
        EnemiesDefeated = 0;
    }

    public void RegisterEnemyDefeated(int points)
    {
        if (IsGameOver)
            return;

        EnemiesDefeated++;
        Score += Mathf.Max(0, points);

        Debug.Log(
            "Score: " + Score +
            " | Enemies defeated: " + EnemiesDefeated
        );
    }


    public void EndGame()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        Debug.Log("GAME OVER!");

        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(
            FindObjectsSortMode.None
        );

        foreach (EnemyAI enemy in enemies)
        {
            enemy.StopAttacking();
        }

        GameOverUI gameOverUI =
            FindFirstObjectByType<GameOverUI>();

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();

        // Freeze gameplay after displaying the panel.
        Time.timeScale = 0f;
    }

    public void ResetForRestart()
    {
        Time.timeScale = 1f;

        IsGameOver = false;
        Score = 0;
        EnemiesDefeated = 0;
    }

}
