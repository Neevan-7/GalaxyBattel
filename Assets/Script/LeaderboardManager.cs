
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Row Creation")]
    [SerializeField] private Transform content;
    [SerializeField] private LeaderboardPlayerRow rowPrefab;

    [Header("Rank Medals: Gold, Silver, Bronze")]
    [SerializeField] private Sprite[] rankMedals;

    [Header("Cosmetic Sprites")]
    [SerializeField] private Sprite[] avatarFrames;
    [SerializeField] private Sprite[] avatars;
    [SerializeField] private Sprite[] ships;

    [Header("Optional Local Demo Entries")]
    [SerializeField] private LeaderboardEntry[] demoEntries;

    [Header("Display")]
    [SerializeField] private int maxRows = 100;

    private const string PlayerNameKey = "GD_PlayerName";
    private const string NicknameKey = "GD_PlayerNickname";
    private const string AvatarKey = "GD_AvatarIndex";
    private const string FrameKey = "GD_AvatarFrameIndex";
    private const string ShipKey = "GD_EquippedShip";
    private const string BestScoreKey = "GD_PlayerBestScore";

    private readonly List<GameObject> spawnedRows =
        new List<GameObject>();

    private void OnEnable()
    {
        RefreshLeaderboard();
    }

    //public void RefreshLeaderboard()
    //{
    //    if (content == null || rowPrefab == null)
    //    {
    //        Debug.LogError(
    //            "Assign Content and Row Prefab in LeaderboardManager.");
    //        return;
    //    }

    //    ClearRows();

    //    List<LeaderboardEntry> entries =
    //        new List<LeaderboardEntry>();

    //    // Optional sample entries for testing.
    //    if (demoEntries != null)
    //    {
    //        foreach (LeaderboardEntry entry in demoEntries)
    //        {
    //            if (entry != null)
    //                entries.Add(entry);
    //        }
    //    }

    //    // Add the actual local player's saved profile.
    //    entries.Add(new LeaderboardEntry
    //    {
    //        playerName = PlayerPrefs.GetString(
    //            PlayerNameKey, "Player"),

    //        nickname = PlayerPrefs.GetString(
    //            NicknameKey, "Rookie"),

    //        score = PlayerPrefs.GetInt(
    //            BestScoreKey, 0),

    //        avatarIndex = PlayerPrefs.GetInt(
    //            AvatarKey, 0),

    //        frameIndex = PlayerPrefs.GetInt(
    //            FrameKey, 0),

    //        shipIndex = PlayerPrefs.GetInt(
    //            ShipKey, 0)
    //    });

    //    List<LeaderboardEntry> sortedEntries = entries
    //        .OrderByDescending(entry => entry.score)
    //        .Take(Mathf.Max(1, maxRows))
    //        .ToList();

    //    for (int i = 0; i < sortedEntries.Count; i++)
    //    {
    //        LeaderboardPlayerRow row = Instantiate(
    //            rowPrefab, content, false);

    //        row.gameObject.SetActive(true);

    //        row.Setup(
    //            sortedEntries[i],
    //            i + 1,
    //            rankMedals,
    //            avatarFrames,
    //            avatars,
    //            ships);

    //        spawnedRows.Add(row.gameObject);
    //    }

    //    Debug.Log(
    //        "Leaderboard refreshed: " +
    //        sortedEntries.Count + " entries.");
    //}


    public void RefreshLeaderboard()
    {
        if (content == null || rowPrefab == null)
        {
            Debug.LogError(
                "Assign Content and Row Prefab in LeaderboardManager.");
            return;
        }

        ClearRows();

        List<LeaderboardEntry> entries =
            new List<LeaderboardEntry>();

        // Load test or existing leaderboard entries.
        if (demoEntries != null)
        {
            foreach (LeaderboardEntry entry in demoEntries)
            {
                if (entry != null &&
                    entry.id != "local-player")
                {
                    entries.Add(entry);
                }
            }
        }

        // Load the saved player profile.
        LeaderboardEntry localPlayer = new LeaderboardEntry
        {
            id = "local-player",

            playerName = PlayerPrefs.GetString(
                "GD_PlayerName", "Player"),

            nickname = PlayerPrefs.GetString(
                "GD_PlayerNickname", "Rookie"),

            score = PlayerPrefs.GetInt(
                "GD_PlayerBestScore", 0),

            avatarIndex = PlayerPrefs.GetInt(
                "GD_AvatarIndex", 0),

            frameIndex = PlayerPrefs.GetInt(
                "GD_AvatarFrameIndex", 0),

            shipIndex = PlayerPrefs.GetInt(
                "GD_EquippedShip", 0)
        };

        entries.Add(localPlayer);

        // Highest score gets rank 1.
        List<LeaderboardEntry> sortedEntries = entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.id)
            .Take(Mathf.Max(1, maxRows))
            .ToList();

        for (int i = 0; i < sortedEntries.Count; i++)
        {
            LeaderboardEntry entry = sortedEntries[i];

            LeaderboardPlayerRow row = Instantiate(
                rowPrefab, content, false);

            row.gameObject.SetActive(true);

            row.Setup(
                entry,
                i + 1,
                rankMedals,
                avatarFrames,
                avatars,
                ships);

            spawnedRows.Add(row.gameObject);

            if (entry.id == "local-player")
            {
                Debug.Log(
                    "Your leaderboard rank: " + (i + 1) +
                    " | Score: " + entry.score);
            }
        }
    }


    private void ClearRows()
    {
        foreach (GameObject row in spawnedRows)
        {
            if (row != null)
                Destroy(row);
        }

        spawnedRows.Clear();
    }
}
