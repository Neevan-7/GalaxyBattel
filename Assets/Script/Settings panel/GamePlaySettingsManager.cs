using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlaySettingsManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;

    [Header("Volume Sliders (0 to 100)")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Percentage Text")]
    [SerializeField] private TMP_Text masterValueText;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject accountPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject resetConfirmPopup;

    private const string MasterKey = "GD_MasterVolume";
    private const string MusicKey = "GD_MusicVolume";
    private const string SfxKey = "GD_SFXVolume";

    private const float DefaultVolume = 100f;

    private float masterVolume = DefaultVolume;
    private float musicVolume = DefaultVolume;
    private float sfxVolume = DefaultVolume;

    private void Awake()
    {
        // Load previously saved settings.
        masterVolume = PlayerPrefs.GetFloat(
            MasterKey, DefaultVolume);

        musicVolume = PlayerPrefs.GetFloat(
            MusicKey, DefaultVolume);

        sfxVolume = PlayerPrefs.GetFloat(
            SfxKey, DefaultVolume);

        // Set up slider ranges.
        SetupSlider(masterSlider);
        SetupSlider(musicSlider);
        SetupSlider(sfxSlider);

        // Display saved settings in the UI.
        SetSliderValue(masterSlider, masterVolume);
        SetSliderValue(musicSlider, musicVolume);
        SetSliderValue(sfxSlider, sfxVolume);

        // Listen for changes.
        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(OnMasterChanged);

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);

        ApplyAudioSettings();
        UpdateLabels();
    }

    private void SetupSlider(Slider slider)
    {
        if (slider == null) return;

        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.wholeNumbers = true;
    }

    private void SetSliderValue(Slider slider, float value)
    {
        if (slider != null)
            slider.SetValueWithoutNotify(
                Mathf.Clamp(value, 0f, 100f));
    }

    // Called whenever the Master slider moves.
    public void OnMasterChanged(float value)
    {
        masterVolume = value;
        ApplyAudioSettings();
        UpdateLabels();
    }

    // Called whenever the Music slider moves.
    public void OnMusicChanged(float value)
    {
        musicVolume = value;
        ApplyAudioSettings();
        UpdateLabels();
    }

    // Called whenever the SFX slider moves.
    public void OnSFXChanged(float value)
    {
        sfxVolume = value;
        ApplyAudioSettings();
        UpdateLabels();
    }

    private void ApplyAudioSettings()
    {
        // Master controls all audio played through AudioListener.
        AudioListener.volume = masterVolume / 100f;

        // Music volume.
        if (musicSource != null)
            musicSource.volume = musicVolume / 100f;

        // Sound-effects volume.
        if (sfxSources != null)
        {
            foreach (AudioSource source in sfxSources)
            {
                if (source != null)
                    source.volume = sfxVolume / 100f;
            }
        }
    }

    private void UpdateLabels()
    {
        if (masterValueText != null)
            masterValueText.text =
                Mathf.RoundToInt(masterVolume) + "%";

        if (musicValueText != null)
            musicValueText.text =
                Mathf.RoundToInt(musicVolume) + "%";

        if (sfxValueText != null)
            sfxValueText.text =
                Mathf.RoundToInt(sfxVolume) + "%";
    }

    // Apply button: save settings and close the panel.
    //public void ApplySettings()
    //{
    //    SaveSettings();

    //    if (settingsPanel != null)
    //        settingsPanel.SetActive(false);

    //    Debug.Log("Audio settings applied and saved.");
    //}


    public void ApplySettings()
    {
        SaveSettings();

        // Close only the audio settings panel.
        if (audioPanel != null)
            audioPanel.SetActive(false);

        // Keep the pause menu open.
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Debug.Log("Audio settings saved. Returning to pause menu.");
    }

    public void CloseSettings()
    {
        // Close only the audio settings panel.
        if (audioPanel != null)
            audioPanel.SetActive(false);

        // Return to the pause menu.
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void OpenSettings1()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void CloseSettings1()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OpenSettings()
    {
        // Make sure the pause menu remains active.
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        // Open the audio settings panel.
        if (audioPanel != null)
            audioPanel.SetActive(true);

        ShowAudioPanel();
    }

    //public void ClosePause()
    //{
    //    // Close only the audio settings panel.
    //    if (pausePanel != null)
    //        pausePanel.SetActive(false);

    //    // Return to the pause menu.
    //    if (pausePanel != null)
    //        pausePanel.SetActive(true);
    //}

    //public void OpenPause()
    //{
    //    // Make sure the pause menu remains active.
    //    if (pausePanel != null)
    //        pausePanel.SetActive(true);

    //    // Open the audio settings panel.
    //    if (pausePanel != null)
    //        pausePanel.SetActive(true);

    //    ShowAudioPanel();
    //}

    // Save button, if you want to save without closing.
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MasterKey, masterVolume);
        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.Save();

        ApplyAudioSettings();
    }

    // Open the settings panel.
    //public void OpenSettings()
    //{
    //    if (settingsPanel != null)
    //        settingsPanel.SetActive(true);

    //    ShowAudioPanel();
    //}

    //public void CloseSettings()
    //{
    //    if (settingsPanel != null)
    //        settingsPanel.SetActive(false);
    //}

    // Settings category navigation.
    public void ShowAudioPanel()
    {
        if (audioPanel != null) audioPanel.SetActive(true);
        if (accountPanel != null) accountPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        if (accountPanel != null) accountPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    public void ShowAccountPanel()
    {
        if (audioPanel != null) audioPanel.SetActive(false);
        if (accountPanel != null) accountPanel.SetActive(true);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    public void ShowGraphicsPanel()
    {
        if (audioPanel != null) audioPanel.SetActive(false);
        if (accountPanel != null) accountPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(true);
    }

    // Reset button: open the confirmation popup.
    public void OpenResetConfirmation()
    {
        if (resetConfirmPopup != null)
            resetConfirmPopup.SetActive(true);
    }

    // Confirm button inside the reset popup.
    public void ConfirmReset()
    {
        masterVolume = DefaultVolume;
        musicVolume = DefaultVolume;
        sfxVolume = DefaultVolume;

        SetSliderValue(masterSlider, masterVolume);
        SetSliderValue(musicSlider, musicVolume);
        SetSliderValue(sfxSlider, sfxVolume);

        ApplyAudioSettings();
        UpdateLabels();
        SaveSettings();

        if (resetConfirmPopup != null)
            resetConfirmPopup.SetActive(false);

        Debug.Log("Settings reset and saved.");
    }

    // Cancel button inside the reset popup.
    public void CancelReset()
    {
        if (resetConfirmPopup != null)
            resetConfirmPopup.SetActive(false);
    }

    private void OnDestroy()
    {
        if (masterSlider != null)
            masterSlider.onValueChanged.RemoveListener(OnMasterChanged);

        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
    }
}
