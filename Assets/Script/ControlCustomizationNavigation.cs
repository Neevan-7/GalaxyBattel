
using UnityEngine;

public class ControlCustomizationNavigation : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject customizationPanel;

    public void OpenCustomization()
    {
        // Close previous panels.
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Open customization.
        if (customizationPanel != null)
            customizationPanel.SetActive(true);
    }

    public void CloseCustomization()
    {
        if (customizationPanel != null)
            customizationPanel.SetActive(false);

        // Return to Settings only.
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
}
