
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("Text")]
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null &&
            GameManager.Instance != null)
        {
            finalScoreText.text =
                GameManager.Instance.Score.ToString("D6");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
            GameManager.Instance.ResetForRestart();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scenes

        SceneManager.LoadScene("MainMenu");
    }

    public void OpenPause()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ClosePause()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
}
