
using UnityEngine;

public static class LevelProgress
{
    private const string Prefix = "GD_Level_";
    private const string TotalStarsKey = "GD_TotalStars";

    public static int TotalStars =>
        PlayerPrefs.GetInt(TotalStarsKey, 0);

    public static int GetStars(int levelId)
    {
        return PlayerPrefs.GetInt(
            Prefix + levelId + "_Stars", 0);
    }

    public static int GetBestScore(int levelId)
    {
        return PlayerPrefs.GetInt(
            Prefix + levelId + "_BestScore", 0);
    }

    public static bool IsUnlocked(LevelDefinition level)
    {
        if (level == null)
            return false;

        // The first level should always be available.
        if (level.levelId == 1)
            return true;

        return TotalStars >= level.requiredTotalStars;
    }

    public static void SaveResult(
        int levelId, int earnedStars, int score)
    {
        earnedStars = Mathf.Clamp(earnedStars, 0, 3);

        int oldStars = GetStars(levelId);
        int oldScore = GetBestScore(levelId);

        // Never overwrite better previous results.
        if (earnedStars > oldStars)
        {
            PlayerPrefs.SetInt(
                Prefix + levelId + "_Stars", earnedStars);
        }

        if (score > oldScore)
        {
            PlayerPrefs.SetInt(
                Prefix + levelId + "_BestScore", score);
        }

        // Recalculate total stars from all saved level ratings.
        // This prevents replaying a level from farming stars.
        int total = 0;

        for (int id = 1; id <= 100; id++)
        {
            total += GetStars(id);
        }

        PlayerPrefs.SetInt(TotalStarsKey, total);
        PlayerPrefs.Save();
    }

    public static void ResetAllProgress()
    {
        PlayerPrefs.DeleteKey(TotalStarsKey);

        for (int id = 1; id <= 100; id++)
        {
            PlayerPrefs.DeleteKey(Prefix + id + "_Stars");
            PlayerPrefs.DeleteKey(Prefix + id + "_BestScore");
        }

        PlayerPrefs.Save();
    }
}
