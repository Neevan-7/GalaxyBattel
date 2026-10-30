
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionResultsUI : MonoBehaviour
{
    [Header("Results Panel")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Image planetImage;
    [SerializeField] private TMP_Text missionTitleText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text totalStarsText;

    [Header("Star Images")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Color earnedStarColor = Color.yellow;
    [SerializeField] private Color emptyStarColor = Color.gray;

    [Header("Buttons")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button levelSelectButton;

    [Header("Level Database")]
    [SerializeField] private LevelDefinition[] levels;

    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField]
    private string levelSelectSceneName =
        "LevelSelectScene";

    private LevelDefinition completedLevel;

    //private void Start()
    //{
    //    if (victoryPanel != null)
    //        victoryPanel.SetActive(false);
    //}

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Register all navigation buttons.
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(PlayNextLevel);
            nextLevelButton.onClick.AddListener(PlayNextLevel);
        }

        if (replayButton != null)
        {
            replayButton.onClick.RemoveListener(ReplayLevel);
            replayButton.onClick.AddListener(ReplayLevel);
        }

        if (levelSelectButton != null)
        {
            levelSelectButton.onClick.RemoveListener(OpenLevelSelect);
            levelSelectButton.onClick.AddListener(OpenLevelSelect);
        }

        Time.timeScale = 1f;
    }

    public void ShowVictory(
        LevelDefinition level, int stars, int score)
    {
        completedLevel = level;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (planetImage != null)
            planetImage.sprite = level.planetImage;

        if (missionTitleText != null)
            missionTitleText.text =
                level.levelName + " COMPLETE!";

        if (finalScoreText != null)
            finalScoreText.text = ""+score;

        if (totalStarsText != null)
            totalStarsText.text =
                " " + LevelProgress.TotalStars;

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].color = i < stars
                    ? earnedStarColor
                    : emptyStarColor;
            }
        }

        bool hasNextUnlocked = FindNextUnlockedLevel() != null;

        if (nextLevelButton != null)
            nextLevelButton.interactable = hasNextUnlocked;

        Time.timeScale = 0f;
    }

    private LevelDefinition FindNextUnlockedLevel()
    {
        if (completedLevel == null || levels == null)
            return null;

        LevelDefinition next = null;

        foreach (LevelDefinition level in levels)
        {
            if (level == null ||
                level.levelId <= completedLevel.levelId ||
                !LevelProgress.IsUnlocked(level))
                continue;

            if (next == null || level.levelId < next.levelId)
                next = level;
        }

        return next;
    }

    public void PlayNextLevel()
    {
        LevelDefinition next = FindNextUnlockedLevel();

        if (next == null)
            return;

        PlayerPrefs.SetInt("GD_SelectedLevelId", next.levelId);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ReplayLevel()
    {
        if (completedLevel == null)
            return;

        PlayerPrefs.SetInt(
            "GD_SelectedLevelId", completedLevel.levelId);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectSceneName);
    }
}
