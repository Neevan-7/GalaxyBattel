
using System.Collections;
using UnityEngine;

public class LevelWaveRunner : MonoBehaviour
{
    public static LevelWaveRunner Instance { get; private set; }

    [Header("Level Database")]
    [SerializeField] private LevelDefinition[] levels;

    [Header("Player / World")]
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [Header("Results")]
    [SerializeField] private MissionResultsUI resultsUI;

    private LevelDefinition activeLevel;
    private int aliveEnemies;
    private bool missionEnded;
    private bool bossSpawned;

    public int CurrentWave { get; private set; }
    public int TotalWaves =>
        activeLevel != null && activeLevel.waves != null
            ? activeLevel.waves.Length : 0;

    public bool MissionEnded => missionEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Prevent the old wave spawner from spawning
        // enemies alongside this level system.
        EnemySpawner oldSpawner =
            FindFirstObjectByType<EnemySpawner>();

        if (oldSpawner != null)
            oldSpawner.enabled = false;

        int selectedId =
            PlayerPrefs.GetInt("GD_SelectedLevelId", 1);

        foreach (LevelDefinition level in levels)
        {
            if (level != null && level.levelId == selectedId)
            {
                activeLevel = level;
                break;
            }
        }

        if (activeLevel == null)
        {
            Debug.LogError(
                "LevelWaveRunner: Selected level not found. " +
                "Assign the level assets in the Inspector.");
            return;
        }

        if (!LevelProgress.IsUnlocked(activeLevel))
        {
            Debug.LogWarning("This level is still locked.");
            return;
        }

        if (backgroundRenderer != null &&
            activeLevel.backgroundImage != null)
        {
            backgroundRenderer.sprite =
                activeLevel.backgroundImage;
        }

        StartCoroutine(RunMission());
    }

    private IEnumerator RunMission()
    {
        if (activeLevel.waves == null ||
            activeLevel.waves.Length == 0)
        {
            Debug.LogError("This level has no configured waves.");
            yield break;
        }

        for (int i = 0; i < activeLevel.waves.Length; i++)
        {
            if (missionEnded)
                yield break;

            CurrentWave = i + 1;

            LevelWave wave = activeLevel.waves[i];

            if (wave.enemyPrefab == null)
            {
                Debug.LogError(
                    "Missing enemy prefab in wave " + CurrentWave);
                yield break;
            }

            for (int j = 0; j < wave.enemyCount; j++)
            {
                if (missionEnded)
                    yield break;

                SpawnEnemy(
                    wave.enemyPrefab,
                    wave.spawnRadius);

                yield return new WaitForSeconds(
                    wave.spawnInterval);
            }

            // Wait for every enemy in this wave to die.
            while (aliveEnemies > 0 && !missionEnded)
                yield return null;
        }

        if (missionEnded)
            yield break;

        if (activeLevel.bossPrefab != null)
        {
            yield return new WaitForSeconds(
                activeLevel.bossSpawnDelay);

            if (missionEnded)
                yield break;

            bossSpawned = true;

            SpawnEnemy(
                activeLevel.bossPrefab, 8f);

            while (aliveEnemies > 0 && !missionEnded)
                yield return null;
        }

        if (!missionEnded)
            CompleteMission();
    }

    private void SpawnEnemy(GameObject prefab, float radius)
    {
        if (prefab == null || missionEnded)
            return;

        Vector3 center = player != null
            ? player.position : transform.position;

        Vector2 offset = Random.insideUnitCircle.normalized *
                         Random.Range(radius * 0.7f, radius);

        Vector3 spawnPosition = center +
            new Vector3(offset.x, offset.y, 0f);

        // Increment first, before the enemy can die.
        aliveEnemies++;

        GameObject spawned = Instantiate(
            prefab, spawnPosition, Quaternion.identity);

        LevelSpawnedEnemy marker =
            spawned.GetComponent<LevelSpawnedEnemy>();

        if (marker == null)
            marker = spawned.AddComponent<LevelSpawnedEnemy>();

        marker.Initialize(this);
    }

    public void NotifyEnemyDefeated(LevelSpawnedEnemy enemy)
    {
        if (missionEnded || enemy == null)
            return;

        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }

    public void FailMission()
    {
        if (missionEnded)
            return;

        missionEnded = true;
        StopAllCoroutines();
    }

    private void CompleteMission()
    {
        if (missionEnded)
            return;

        missionEnded = true;

        int score = GameManager.Instance != null
            ? GameManager.Instance.Score : 0;

        PlayerHealth health =
            player != null
                ? player.GetComponent<PlayerHealth>()
                : FindFirstObjectByType<PlayerHealth>();

        float healthPercent = health != null &&
                              health.MaxHealth > 0
            ? (float)health.CurrentHealth / health.MaxHealth
            : 0f;

        int stars = CalculateStars(score, healthPercent);

        LevelProgress.SaveResult(
            activeLevel.levelId, stars, score);

        if (resultsUI != null)
            resultsUI.ShowVictory(
                activeLevel, stars, score);
        else
            Debug.Log(
                "Mission complete! Stars earned: " + stars);
    }

    private int CalculateStars(int score, float healthPercent)
    {
        // Completion always awards at least one star.
        int stars = 1;

        if (healthPercent >=
            activeLevel.healthPercentForTwoStars)
        {
            stars = 2;
        }

        if (healthPercent >=
                activeLevel.healthPercentForThreeStars &&
            score >= activeLevel.scoreForThreeStars)
        {
            stars = 3;
        }

        return stars;
    }

    public void NotifyGameOver()
    {
        FailMission();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
