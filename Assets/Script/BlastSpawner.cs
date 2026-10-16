
using UnityEngine;

public class BlastSpawner : MonoBehaviour
{
    public static BlastSpawner Instance { get; private set; }

    [SerializeField] private GameObject blastPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBlast(Vector3 position)
    {
        if (blastPrefab == null)
        {
            Debug.LogWarning("Blast prefab is not assigned!");
            return;
        }

        Instantiate(
            blastPrefab,
            position,
            Quaternion.identity
        );
    }
}
