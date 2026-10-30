
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewLevel",
    menuName = "Galaxy Defender/Level Definition"
)]
public class LevelDefinition : ScriptableObject
{
    [Header("Level Information")]
    public int levelId = 1;
    public string levelName = "Earth Orbit";
    [TextArea(2, 4)]
    public string description = "Defend Earth from incoming enemies.";

    [Header("Visuals")]
    public Sprite planetImage;
    public Sprite backgroundImage;

    [Header("Star Unlocking")]
    [Min(0)]
    public int requiredTotalStars = 0;

    [Header("Waves")]
    public LevelWave[] waves;

    [Header("Final Boss")]
    public GameObject bossPrefab;
    [Min(0f)]
    public float bossSpawnDelay = 2f;

    [Header("Star Rating")]
    [Min(0)]
    public int scoreForThreeStars = 1000;

    [Range(0f, 1f)]
    public float healthPercentForTwoStars = 0.5f;

    [Range(0f, 1f)]
    public float healthPercentForThreeStars = 0.8f;
}

[System.Serializable]
public class LevelWave
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;

    [Min(1)]
    public int enemyCount = 5;

    [Min(0.1f)]
    public float spawnInterval = 1f;

    [Min(1f)]
    public float spawnRadius = 7f;
}
