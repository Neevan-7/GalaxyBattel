
//using System.Collections;
//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    public static EnemySpawner Instance { get; private set; }

//    [Header("Enemy")]
//    [SerializeField] private GameObject enemyPrefab;

//    [Header("Wave Settings")]
//    [SerializeField] private float timeBetweenWaves = 3f;
//    [SerializeField] private float spawnRadius = 7f;

//    private int currentWave = 0;
//    private int enemiesRemaining = 0;
//    private bool waitingForNextWave = false;

//    public int CurrentWave => currentWave;
//    public int EnemiesRemaining => enemiesRemaining;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//    }

//    private void Start()
//    {
//        StartCoroutine(StartNextWave());
//    }

//    private IEnumerator StartNextWave()
//    {
//        waitingForNextWave = true;

//        yield return new WaitForSeconds(timeBetweenWaves);

//        if (IsGameOver())
//            yield break;

//        currentWave++;

//        int enemyCount = currentWave;
//        enemiesRemaining = enemyCount;
//        waitingForNextWave = false;

//        Debug.Log(
//            "WAVE " + currentWave +
//            " STARTED! Enemies: " + enemyCount
//        );

//        for (int i = 0; i < enemyCount; i++)
//        {
//            if (IsGameOver())
//                yield break;

//            Vector3 spawnPosition = GetSpawnPosition();

//            GameObject enemy = Instantiate(
//                enemyPrefab,
//                spawnPosition,
//                Quaternion.identity
//            );

//            EnemyAI ai = enemy.GetComponent<EnemyAI>();

//            if (ai != null)
//            {
//                float speedMultiplier =
//                    1f + (currentWave - 1) * 0.1f;

//                float fireRateMultiplier =
//                    Mathf.Max(0.5f, 1f - (currentWave - 1) * 0.05f);

//                ai.SetDifficulty(
//                    speedMultiplier,
//                    fireRateMultiplier
//                );
//            }
//        }
//    }

//    private Vector3 GetSpawnPosition()
//    {
//        //Transform player = GameObject.FindGameObjectWithTag(
//        //    "Player"
//        //)?.transform;

//        //Vector2 center = player != null
//        //    ? (Vector2)player.position
//        //    : Vector2.zero;

//        //Vector2 offset = Random.insideUnitCircle;

//        //if (offset.sqrMagnitude < 0.01f)
//        //    offset = Vector2.up;

//        //offset = offset.normalized * spawnRadius;

//        //return new Vector3(
//        //    center.x + offset.x,
//        //    center.y + offset.y,
//        //    0f
//        //);

//        Camera cam = Camera.main;

//        if (cam == null)
//            return Vector3.zero;

//        // Select one of the four screen edges.
//        int side = Random.Range(0, 4);

//        float x;
//        float y;

//        switch (side)
//        {
//            case 0: // Top
//                x = Random.Range(0.1f, 0.9f);
//                y = 0.9f;
//                break;

//            case 1: // Bottom
//                x = Random.Range(0.1f, 0.9f);
//                y = 0.1f;
//                break;

//            case 2: // Left
//                x = 0.1f;
//                y = Random.Range(0.1f, 0.9f);
//                break;

//            default: // Right
//                x = 0.9f;
//                y = Random.Range(0.1f, 0.9f);
//                break;
//        }

//        Vector3 position = cam.ViewportToWorldPoint(
//            new Vector3(x, y, 10f)
//        );

//        position.z = 0f;
//        return position;
//    }

//    public void EnemyDefeated()
//    {
//        if (IsGameOver() || waitingForNextWave)
//            return;

//        enemiesRemaining--;

//        Debug.Log(
//            "Enemies remaining in Wave " +
//            currentWave + ": " + enemiesRemaining
//        );

//        if (enemiesRemaining <= 0)
//        {
//            Debug.Log("Wave " + currentWave + " completed!");
//            StartCoroutine(StartNextWave());
//        }
//    }

//    private bool IsGameOver()
//    {
//        return GameManager.Instance != null &&
//               GameManager.Instance.IsGameOver;
//    }
//}



using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [System.Serializable]
    public class EnemySpawnEntry
    {
        public GameObject enemyPrefab;

        [Min(0)]
        public int count = 1;
    }

    [System.Serializable]
    public class WaveDefinition
    {
        [Tooltip("Wave number this configuration represents.")]
        [Min(1)]
        public int waveNumber = 1;

        public EnemySpawnEntry[] enemies;
    }

    [Header("Wave Configurations")]
    [SerializeField] private WaveDefinition[] waveDefinitions;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float spawnRadius = 7f;

    private int currentWave = 0;
    private int enemiesRemaining = 0;
    private bool waitingForNextWave = false;
    private Coroutine waveCoroutine;

    public int CurrentWave => currentWave;
    public int EnemiesRemaining => enemiesRemaining;

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
        waveCoroutine = StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        waitingForNextWave = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        if (IsGameOver())
            yield break;

        currentWave++;

        WaveDefinition wave = GetWaveDefinition(currentWave);

        if (wave == null)
        {
            Debug.LogError("No valid enemy wave configuration!");
            yield break;
        }

        int extraWaveLevels =
            Mathf.Max(0, currentWave - wave.waveNumber);

        int totalToSpawn = 0;

        foreach (EnemySpawnEntry entry in wave.enemies)
        {
            if (entry == null || entry.enemyPrefab == null)
                continue;

            totalToSpawn += entry.count *
                (1 + extraWaveLevels / 2);
        }

        enemiesRemaining = totalToSpawn;
        waitingForNextWave = false;

        Debug.Log(
            "WAVE " + currentWave +
            " STARTED! Enemies: " + enemiesRemaining
        );

        foreach (EnemySpawnEntry entry in wave.enemies)
        {
            if (entry == null || entry.enemyPrefab == null)
                continue;

            int amount = entry.count *
                (1 + extraWaveLevels / 2);

            for (int i = 0; i < amount; i++)
            {
                if (IsGameOver())
                    yield break;

                Vector3 spawnPosition = GetSpawnPosition();

                GameObject enemy = Instantiate(
                    entry.enemyPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                EnemyAI ai = enemy.GetComponent<EnemyAI>();

                if (ai != null)
                {
                    float speedMultiplier =
                        1f + (currentWave - 1) * 0.1f;

                    float fireRateMultiplier =
                        Mathf.Max(
                            0.5f,
                            1f - (currentWave - 1) * 0.05f
                        );

                    ai.SetDifficulty(
                        speedMultiplier,
                        fireRateMultiplier
                    );
                }
            }
        }

        waveCoroutine = null;

        // Handle an empty or invalid wave configuration.
        if (enemiesRemaining <= 0)
        {
            Debug.LogWarning(
                "This wave has no valid enemies. Check the Inspector."
            );
            waveCoroutine = StartCoroutine(StartNextWave());
        }
    }

    private WaveDefinition GetWaveDefinition(int waveNumber)
    {
        if (waveDefinitions == null ||
            waveDefinitions.Length == 0)
        {
            return null;
        }

        // Use the matching configured wave, if present.
        foreach (WaveDefinition definition in waveDefinitions)
        {
            if (definition != null &&
                definition.waveNumber == waveNumber)
            {
                return definition;
            }
        }

        // For later waves, reuse the highest-numbered definition.
        WaveDefinition lastWave = null;

        foreach (WaveDefinition definition in waveDefinitions)
        {
            if (definition == null)
                continue;

            if (lastWave == null ||
                definition.waveNumber > lastWave.waveNumber)
            {
                lastWave = definition;
            }
        }

        return lastWave;
    }

    private Vector3 GetSpawnPosition()
    {
        Camera cam = Camera.main;

        if (cam == null)
            return Vector3.zero;

        int side = Random.Range(0, 4);

        float x;
        float y;

        switch (side)
        {
            case 0:
                x = Random.Range(0.1f, 0.9f);
                y = 0.9f;
                break;

            case 1:
                x = Random.Range(0.1f, 0.9f);
                y = 0.1f;
                break;

            case 2:
                x = 0.1f;
                y = Random.Range(0.1f, 0.9f);
                break;

            default:
                x = 0.9f;
                y = Random.Range(0.1f, 0.9f);
                break;
        }

        Vector3 position = cam.ViewportToWorldPoint(
            new Vector3(x, y, 10f)
        );

        position.z = 0f;
        return position;
    }

    public void EnemyDefeated()
    {
        if (IsGameOver() || waitingForNextWave)
            return;

        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);

        Debug.Log(
            "Enemies remaining in Wave " +
            currentWave + ": " + enemiesRemaining
        );

        if (enemiesRemaining == 0 && waveCoroutine == null)
        {
            Debug.Log("Wave " + currentWave + " completed!");
            waveCoroutine = StartCoroutine(StartNextWave());
        }
    }

    private bool IsGameOver()
    {
        return GameManager.Instance != null &&
               GameManager.Instance.IsGameOver;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
