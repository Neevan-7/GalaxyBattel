
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPlayerRow : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image rankMedal;
    [SerializeField] private TMP_Text rankNumber;
    [SerializeField] private Image avatarFrame;
    [SerializeField] private Image playerAvatar;
    [SerializeField] private Image shipIcon;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text scoreText;

    public void Setup(
        LeaderboardEntry entry,
        int rank,
        Sprite[] rankMedals,
        Sprite[] avatarFrames,
        Sprite[] avatars,
        Sprite[] ships)
    {
        if (entry == null)
            return;

        if (playerName != null)
        {
            playerName.text = string.IsNullOrWhiteSpace(entry.nickname)
                ? entry.playerName
                : entry.playerName + "\n<size=70%>@" +
                  entry.nickname + "</size>";
        }

        if (scoreText != null)
            scoreText.text = entry.score.ToString("N0");

        bool hasMedal =
            rank >= 1 && rank <= 3 &&
            rankMedals != null &&
            rankMedals.Length >= rank &&
            rankMedals[rank - 1] != null;

        if (rankMedal != null)
        {
            rankMedal.gameObject.SetActive(hasMedal);

            if (hasMedal)
                rankMedal.sprite = rankMedals[rank - 1];
        }

        if (rankNumber != null)
        {
            rankNumber.gameObject.SetActive(!hasMedal);

            if (!hasMedal)
                rankNumber.text = rank.ToString();
        }

        SetSprite(avatarFrame, avatarFrames, entry.frameIndex);
        SetSprite(playerAvatar, avatars, entry.avatarIndex);
        SetSprite(shipIcon, ships, entry.shipIndex);
    }

    private void SetSprite(
        Image target, Sprite[] sprites, int index)
    {
        if (target == null)
            return;

        bool valid = sprites != null &&
                     index >= 0 &&
                     index < sprites.Length &&
                     sprites[index] != null;

        target.enabled = valid;

        if (valid)
            target.sprite = sprites[index];
    }
}
