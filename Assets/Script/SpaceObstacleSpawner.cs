
using UnityEngine;

public class SpaceObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private GameObject[] asteroidPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2.5f;
    [SerializeField] private float spawnHeightOffset = 1.1f;
    [SerializeField] private float horizontalPadding = 0.08f;

    [Header("Random Scale")]
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private float maxScale = 1.0f;

    private float spawnTimer;

    private void Awake()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;
    }

    private void Update()
    {
        if (gameCamera == null || asteroidPrefabs.Length == 0)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        GameObject prefab = asteroidPrefabs[
            Random.Range(0, asteroidPrefabs.Length)
        ];

        // Random point just above the camera's visible area.
        float x = Random.Range(
            horizontalPadding,
            1f - horizontalPadding
        );

        Vector3 viewportPosition = new Vector3(
            x,
            spawnHeightOffset,
            Mathf.Abs(gameCamera.transform.position.z)
        );

        Vector3 spawnPosition =
            gameCamera.ViewportToWorldPoint(viewportPosition);

        GameObject asteroid = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))
        );

        float scale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * scale;
    }
}
