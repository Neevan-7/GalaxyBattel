
using System;

[Serializable]
public class LeaderboardEntry
{
    public string id;
    public string playerName;
    public string nickname;
    public int score;
    public int avatarIndex;
    public int frameIndex;
    public int shipIndex;

    // Supports existing LeaderboardUI constructor calls.
    public LeaderboardEntry(
        string id,
        string playerName,
        int score,
        int avatarIndex,
        int frameIndex,
        int shipIndex)
    {
        this.id = id;
        this.playerName = playerName;
        this.nickname = "";
        this.score = score;
        this.avatarIndex = avatarIndex;
        this.frameIndex = frameIndex;
        this.shipIndex = shipIndex;
    }

    // Supports the object initializer in LeaderboardManager.
    public LeaderboardEntry()
    {
        id = "";
        playerName = "Player";
        nickname = "";
        score = 0;
        avatarIndex = 0;
        frameIndex = 0;
        shipIndex = 0;
    }
}
