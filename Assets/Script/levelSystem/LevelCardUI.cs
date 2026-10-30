using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCardUI : MonoBehaviour
{
    [Header("Card UI")]
    [SerializeField] private Image cardFrame;
    [SerializeField] private Image planetImage;
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private Button cardButton;

    [Header("Card Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite lockedSprite;

    [Header("Star Sprites")]
    [SerializeField] private Sprite earnedStarSprite;
    [SerializeField] private Sprite emptyStarSprite;

    private LevelDefinition level;
    private Action<LevelDefinition> onSelected;

    public void Setup(
        LevelDefinition definition,
        Action<LevelDefinition> selectionCallback)
    {
        level = definition;
        onSelected = selectionCallback;

        if (level == null)
        {
            cardButton.interactable = false;
            return;
        }

        if (planetImage != null)
            planetImage.sprite = level.planetImage;

        if (levelNumberText != null)
            levelNumberText.text = level.levelId.ToString("00");

        if (levelNameText != null)
            levelNameText.text = level.levelName;

        bool unlocked = LevelProgress.IsUnlocked(level);

        if (lockOverlay != null)
            lockOverlay.SetActive(!unlocked);

        if (cardButton != null)
        {
            cardButton.interactable = unlocked;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() =>
            {
                if (unlocked)
                    onSelected?.Invoke(level);
            });
        }

        RefreshStars();
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (cardFrame == null || level == null)
            return;

        if (!LevelProgress.IsUnlocked(level))
            cardFrame.sprite = lockedSprite;
        else if (selected)
            cardFrame.sprite = selectedSprite;
        else
            cardFrame.sprite = normalSprite;
    }

    public int LevelId => level != null ? level.levelId : -1;

    private void RefreshStars()
    {
        int stars = LevelProgress.GetStars(level.levelId);

        if (starImages == null)
            return;

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].sprite =
                    i < stars ? earnedStarSprite : emptyStarSprite;
            }
        }
    }
}