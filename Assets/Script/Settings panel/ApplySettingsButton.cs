
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ApplySettingsButton : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Optional Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    public void ApplySettings()
    {
        if (masterSlider == null ||
            musicSlider == null ||
            sfxSlider == null)
        {
            Debug.LogError("Assign all three audio sliders.");
            return;
        }

        SettingsStorage.Save(
            masterSlider.value,
            musicSlider.value,
            sfxSlider.value
        );

        SettingsStorage.Apply(audioMixer);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Debug.Log("Settings saved successfully.");
    }
}
