
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Scroll View Content")]
    [SerializeField] private Transform content;
    [SerializeField] private LeaderboardPlayerRow rowPrefab;

    [Header("Rank Medals: Gold, Silver, Bronze")]
    [SerializeField] private Sprite[] rankMedals;

    [Header("Player Cosmetics")]
    [SerializeField] private Sprite[] avatarFrames;
    [SerializeField] private Sprite[] avatars;
    [SerializeField] private Sprite[] ships;

    private readonly List<LeaderboardEntry> entries =
        new List<LeaderboardEntry>();

    private void Start()
    {
        // Temporary sample data for testing.
        entries.Add(new LeaderboardEntry(
            "p01", "GalaxyKing", 1245680, 0, 0, 0));

        entries.Add(new LeaderboardEntry(
            "p02", "NovaRider", 1132540, 1, 1, 1));

        entries.Add(new LeaderboardEntry(
            "p03", "ShadowAce", 987320, 2, 2, 2));

        entries.Add(new LeaderboardEntry(
            "p04", "StarBlaze", 876450, 3, 3, 3));

        entries.Add(new LeaderboardEntry(
            "p05", "CosmicWolf", 742380, 4, 1, 4));

        entries.Add(new LeaderboardEntry(
            "p06", "VoidRanger", 601530, 5, 4, 5));

        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        // Clear old rows.
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        // Highest score first.
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Create exactly one row per player.
        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardPlayerRow row =
                Instantiate(rowPrefab, content);

            row.Setup(
                entries[i],
                i + 1,
                rankMedals,
                avatarFrames,
                avatars,
                ships);
        }
    }
}
