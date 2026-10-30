
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class LevelSelectUI : MonoBehaviour
//{
//    [Header("Level Data")]
//    [SerializeField] private LevelDefinition[] levels;

//    [Header("Level Grid")]
//    [SerializeField] private Transform content;
//    [SerializeField] private LevelCardUI cardPrefab;

//    [Header("Mission Details")]
//    [SerializeField] private Image selectedPlanetImage;
//    [SerializeField] private TMP_Text selectedLevelName;
//    [SerializeField] private TMP_Text descriptionText;
//    [SerializeField] private TMP_Text difficultyText;
//    [SerializeField] private TMP_Text starsText;
//    [SerializeField] private TMP_Text unlockRequirementText;
//    [SerializeField] private Button playButton;

//    [Header("Scene Names")]
//    [SerializeField] private string gameplaySceneName = "Gameplay";
//    [SerializeField] private string mainMenuSceneName = "MainMenu";

//    private LevelDefinition selectedLevel;
//    private readonly List<LevelCardUI> cards =
//        new List<LevelCardUI>();

//    private void Start()
//    {
//        BuildLevelCards();

//        if (levels != null && levels.Length > 0)
//        {
//            LevelDefinition firstUnlocked = null;

//            foreach (LevelDefinition level in levels)
//            {
//                if (level != null &&
//                    LevelProgress.IsUnlocked(level))
//                {
//                    firstUnlocked = level;
//                    break;
//                }
//            }

//            if (firstUnlocked != null)
//                SelectLevel(firstUnlocked);
//        }
//    }

//    private void BuildLevelCards()
//    {
//        foreach (LevelCardUI oldCard in cards)
//        {
//            if (oldCard != null)
//                Destroy(oldCard.gameObject);
//        }

//        cards.Clear();

//        if (content == null || cardPrefab == null ||
//            levels == null)
//        {
//            Debug.LogError(
//                "LevelSelectUI: Assign Content, Card Prefab and Levels.");
//            return;
//        }

//        foreach (LevelDefinition level in levels)
//        {
//            if (level == null)
//                continue;

//            LevelCardUI card = Instantiate(cardPrefab, content);
//            card.Setup(level, SelectLevel);
//            cards.Add(card);
//        }
//    }

//    public void SelectLevel(LevelDefinition level)
//    {
//        if (level == null)
//            return;

//        selectedLevel = level;

//        if (selectedPlanetImage != null)
//            selectedPlanetImage.sprite = level.planetImage;

//        if (selectedLevelName != null)
//            selectedLevelName.text =
//                "LEVEL " + level.levelId + " - " + level.levelName;

//        if (descriptionText != null)
//            descriptionText.text = level.description;

//        if (difficultyText != null)
//        {
//            difficultyText.text =
//                level.waves.Length + " WAVES + BOSS";
//        }

//        if (starsText != null)
//            starsText.text =
//                "STARS: " +
//                LevelProgress.GetStars(level.levelId) + " / 3";

//        bool unlocked = LevelProgress.IsUnlocked(level);

//        if (unlockRequirementText != null)
//        {
//            unlockRequirementText.text = unlocked
//                ? "MISSION AVAILABLE"
//                : "REQUIRES " + level.requiredTotalStars +
//                  " TOTAL STARS";
//        }

//        if (playButton != null)
//            playButton.interactable = unlocked;
//    }

//    public void PlaySelectedLevel()
//    {
//        if (selectedLevel == null ||
//            !LevelProgress.IsUnlocked(selectedLevel))
//            return;

//        Time.timeScale = 1f;

//        PlayerPrefs.SetInt(
//            "GD_SelectedLevelId", selectedLevel.levelId);

//        PlayerPrefs.Save();

//        SceneManager.LoadScene(gameplaySceneName);
//    }

//    public void BackToMainMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(mainMenuSceneName);
//    }

//    public void RefreshLevelCards()
//    {
//        BuildLevelCards();

//        if (selectedLevel != null)
//            SelectLevel(selectedLevel);
//    }
//}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "GamePlay";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private const string SelectedLevelKey = "GD_SelectedLevelId";

    [Header("Level List")]
    [SerializeField] private LevelDefinition[] levels;
    [SerializeField] private Transform content;
    [SerializeField] private LevelCardUI levelCardPrefab;

    [Header("Aside Preview")]
    [SerializeField] private Image previewPlanetImage;

    [Header("Aside Content")]
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text bossText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Image[] detailStarImages;

    [Header("Launch")]
    [SerializeField] private Button launchButton;
    [SerializeField] private Sprite launchNormalSprite;
    [SerializeField] private Sprite launchCompletedSprite;
    [SerializeField] private Image launchButtonImage;

    private readonly List<LevelCardUI> cards = new();
    private LevelDefinition selectedLevel;

    //private void Start()
    //{
    //    BuildLevelCards();

    //    if (levels != null)
    //    {
    //        foreach (LevelDefinition level in levels)
    //        {
    //            if (level != null && LevelProgress.IsUnlocked(level))
    //            {
    //                SelectLevel(level);
    //                break;
    //            }
    //        }
    //    }

    //    if (launchButton != null)
    //    {
    //        launchButton.onClick.RemoveAllListeners();
    //        launchButton.onClick.AddListener(LaunchSelectedLevel);
    //    }
    //}

    private void Start()
    {
        BuildLevelCards();

        int savedLevelId =
            PlayerPrefs.GetInt(SelectedLevelKey, 1);

        LevelDefinition initialLevel = null;

        // Restore previously selected level.
        foreach (LevelDefinition level in levels)
        {
            if (level != null &&
                level.levelId == savedLevelId &&
                LevelProgress.IsUnlocked(level))
            {
                initialLevel = level;
                break;
            }
        }

        // Fallback to first unlocked level.
        if (initialLevel == null)
        {
            foreach (LevelDefinition level in levels)
            {
                if (level != null &&
                    LevelProgress.IsUnlocked(level))
                {
                    initialLevel = level;
                    break;
                }
            }
        }

        if (initialLevel != null)
            SelectLevel(initialLevel);
    }

    //private void BuildLevelCards()
    //{
    //    foreach (Transform child in content)
    //        Destroy(child.gameObject);

    //    cards.Clear();

    //    if (levels == null || content == null || levelCardPrefab == null)
    //        return;

    //    foreach (LevelDefinition level in levels)
    //    {
    //        if (level == null)
    //            continue;

    //        LevelCardUI card = Instantiate(levelCardPrefab, content);
    //        card.Setup(level, SelectLevel);
    //        cards.Add(card);
    //    }
    //}

    private void BuildLevelCards()
    {
        // Validate references BEFORE removing existing cards.
        if (content == null)
        {
            Debug.LogError("LevelSelectUI: Content is not assigned!");
            return;
        }

        if (levelCardPrefab == null)
        {
            Debug.LogError("LevelSelectUI: Level Card Prefab is not assigned!");
            return;
        }

        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("LevelSelectUI: No LevelDefinition assets assigned!");
            return;
        }

        // Clear old generated cards only after validation.
        foreach (Transform child in content)
            Destroy(child.gameObject);

        cards.Clear();

        // Generate the cards.
        foreach (LevelDefinition level in levels)
        {
            if (level == null)
            {
                Debug.LogWarning("LevelSelectUI: A level entry is empty!");
                continue;
            }

            LevelCardUI card = Instantiate(levelCardPrefab, content);
            card.Setup(level, SelectLevel);
            cards.Add(card);
        }

        Debug.Log("Level cards created: " + cards.Count);
    }

    public void SelectLevel(LevelDefinition level)
    {
        if (level == null || !LevelProgress.IsUnlocked(level))
            return;

        selectedLevel = level;

        PlayerPrefs.SetInt(SelectedLevelKey, level.levelId);
        PlayerPrefs.Save();

        //foreach (LevelCardUI card in cards)
        //{
        //    // Card selection state is updated below by checking its
        //    // associated level. Add SetLevelReference if needed.
        //    card.SetSelected(false);
        //}

        foreach (LevelCardUI card in cards)
            card.SetSelected(card.LevelId == level.levelId);

        if (previewPlanetImage != null)
            previewPlanetImage.sprite = level.planetImage;

        if (levelNameText != null)
            levelNameText.text = level.levelName;

        if (descriptionText != null)
            descriptionText.text = level.description;

        if (waveText != null)
            waveText.text =
                (level.waves == null ? 0 : level.waves.Length)
                + " WAVES";

        if (bossText != null)
            bossText.text = level.bossPrefab != null
                ? "BOSS: " + level.bossPrefab.name
                : "BOSS: NOT ASSIGNED";

        int earnedStars = LevelProgress.GetStars(level.levelId);

        if (rewardText != null)
            rewardText.text = "BEST RATING: " + earnedStars + " / 3 STARS";

        if (detailStarImages != null)
        {
            for (int i = 0; i < detailStarImages.Length; i++)
            {
                if (detailStarImages[i] != null)
                    detailStarImages[i].enabled = true;
            }
        }

        if (launchButton != null)
            launchButton.interactable = true;

        if (launchButtonImage != null)
        {
            launchButtonImage.sprite = earnedStars > 0
                ? launchCompletedSprite
                : launchNormalSprite;
        }
    }

    //private void LaunchSelectedLevel()
    //{
    //    if (selectedLevel == null ||
    //        !LevelProgress.IsUnlocked(selectedLevel))
    //        return;

    //    PlayerPrefs.SetInt("GD_SelectedLevelId", selectedLevel.levelId);
    //    PlayerPrefs.Save();

    //    UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    //}

    private void LaunchSelectedLevel()
    {
        if (selectedLevel == null ||
            !LevelProgress.IsUnlocked(selectedLevel))
            return;

        Time.timeScale = 1f;

        PlayerPrefs.SetInt(
            SelectedLevelKey, selectedLevel.levelId);

        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            gameplaySceneName);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            mainMenuSceneName);
    }

}