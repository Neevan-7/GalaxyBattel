
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject criditPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject leaderBoardPanel;

    [Header("Settings Controls")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private TMP_Text volumeLabel;

    private float previousVolume = 1f;

    private void Start()
    {
        Time.timeScale = 1f;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.value = AudioListener.volume;

            masterVolumeSlider.onValueChanged.AddListener(
                SetMasterVolume
            );
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = AudioListener.volume <= 0f;

            muteToggle.onValueChanged.AddListener(SetMute);
        }

        UpdateVolumeLabel(AudioListener.volume);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            Debug.LogError("Enter your gameplay scene name.");
            return;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

     public void OpenCridit()
    {
        if (criditPanel != null)
            criditPanel.SetActive(true);
    }

    public void CloseCridit()
    {
        if (criditPanel != null)
            criditPanel.SetActive(false);
    }

    public void OpenHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(false);
    }

    public void Openprofile()
    {
        if(profilePanel !=null)
            profilePanel.SetActive(true);
    }

    public void Closeprofile()
    {
        if(profilePanel!=null)
            profilePanel.SetActive(false);
    }

    public void Openleader()
    {
        if (leaderBoardPanel != null)
            leaderBoardPanel.SetActive(true);
    }

    public void Closeleader()
    {
        if (leaderBoardPanel != null)
            leaderBoardPanel.SetActive(false);
    }
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;

        if (volume > 0f)
            previousVolume = volume;

        if (muteToggle != null)
            muteToggle.SetIsOnWithoutNotify(volume <= 0f);

        UpdateVolumeLabel(volume);
    }

    public void SetMute(bool muted)
    {
        if (muted)
        {
            if (AudioListener.volume > 0f)
                previousVolume = AudioListener.volume;

            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = Mathf.Clamp01(
                previousVolume > 0f ? previousVolume : 1f
            );
        }

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(
                AudioListener.volume
            );
        }

        UpdateVolumeLabel(AudioListener.volume);
    }

    private void UpdateVolumeLabel(float volume)
    {
        if (volumeLabel != null)
        {
            volumeLabel.text =
                "MASTER VOLUME: " +
                Mathf.RoundToInt(volume * 100f) + "%";
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        Debug.Log("Quit clicked. Stop Play mode to exit the Editor test.");
#else
        Application.Quit();
#endif
    }
}
