
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ConfirmPopup1 : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popupPanel;

    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Optional Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    public void ConfirmReset()
    {
        // Save the default values first.
        SettingsStorage.Reset();

        // Update the visible sliders immediately.
        if (masterSlider != null)
            masterSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultMaster);

        if (musicSlider != null)
            musicSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultMusic);

        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(
                SettingsStorage.DefaultSFX);

        // Apply the restored audio levels.
        SettingsStorage.Apply(audioMixer);

        // Close the confirmation popup.
        if (popupPanel != null)
            popupPanel.SetActive(false);

        Debug.Log("Settings reset to defaults.");
    }

    public void CancelReset()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
