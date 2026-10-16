
using UnityEngine;

public class EnemyPowerUpDrop : MonoBehaviour
{
    [Header("Drop Chance")]
    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.30f;

    [Header("Power-Up Prefabs")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Header("Spawn Position")]
    [SerializeField] private float spawnOffset = 0.5f;

    public void DropRandomPowerUp()
    {
        // No prefabs assigned? Do not drop anything.
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
            return;

        // Decide whether this enemy drops a power-up.
        if (Random.value > dropChance)
            return;

        // Choose one of the assigned power-ups randomly.
        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject selectedPrefab = powerUpPrefabs[index];

        if (selectedPrefab == null)
            return;

        Vector2 offset = Random.insideUnitCircle * spawnOffset;
        Vector3 spawnPosition = transform.position + (Vector3)offset;

        Instantiate(
            selectedPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log("Enemy dropped: " + selectedPrefab.name);
    }
}
