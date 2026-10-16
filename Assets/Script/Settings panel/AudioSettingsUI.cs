
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Value Text")]
    [SerializeField] private TMP_Text masterValueText;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;

    private void Start()
    {
        // Update the displayed values when the scene starts
        UpdateMasterValue(masterSlider.value);
        UpdateMusicValue(musicSlider.value);
        UpdateSfxValue(sfxSlider.value);

        // Listen for slider changes
        masterSlider.onValueChanged.AddListener(UpdateMasterValue);
        musicSlider.onValueChanged.AddListener(UpdateMusicValue);
        sfxSlider.onValueChanged.AddListener(UpdateSfxValue);
    }

    private void UpdateMasterValue(float value)
    {
        masterValueText.text = Mathf.RoundToInt(value).ToString();
    }

    private void UpdateMusicValue(float value)
    {
        musicValueText.text = Mathf.RoundToInt(value).ToString();
    }

    private void UpdateSfxValue(float value)
    {
        sfxValueText.text = Mathf.RoundToInt(value).ToString();
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveListener(UpdateMasterValue);
        musicSlider.onValueChanged.RemoveListener(UpdateMusicValue);
        sfxSlider.onValueChanged.RemoveListener(UpdateSfxValue);
    }
}