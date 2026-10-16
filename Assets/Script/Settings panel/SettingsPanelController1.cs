using UnityEngine;

public class SettingsPanelController1 : MonoBehaviour
{
    [Header("Settings Panels")]
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject accountPanel;
    [SerializeField] private GameObject graphicsPanel;

    private void Start()
    {
        ShowAudio();
    }

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        accountPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }

    public void ShowAccount()
    {
        audioPanel.SetActive(false);
        accountPanel.SetActive(true);
        graphicsPanel.SetActive(false);
    }

    public void ShowGraphics()
    {
        audioPanel.SetActive(false);
        accountPanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }

}