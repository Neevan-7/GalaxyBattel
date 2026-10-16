
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ConfirmPopup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popupPanel;

    [Header("Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Optional Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(ConfirmReset);
            confirmButton.onClick.AddListener(ConfirmReset);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveListener(CancelReset);
            cancelButton.onClick.AddListener(CancelReset);
        }
    }


    public void ConfirmReset()
    {
        SettingsStorage.Reset();

        if (masterSlider != null)
            masterSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultMaster);

        if (musicSlider != null)
            musicSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultMusic);

        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultSFX);

        SettingsStorage.Apply(audioMixer);

        if (popupPanel != null)
            popupPanel.SetActive(false);

        Debug.Log(
            $"Reset complete: Master={SettingsStorage.Master}, " +
            $"Music={SettingsStorage.Music}, SFX={SettingsStorage.SFX}");
    }


    public void CancelReset()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        Debug.Log("Reset cancelled.");
    }
}
