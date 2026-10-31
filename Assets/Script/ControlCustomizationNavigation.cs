
//using UnityEngine;

//public class ControlCustomizationNavigation : MonoBehaviour
//{
//    [Header("Panels")]
//    [SerializeField] private GameObject pausePanel;
//    [SerializeField] private GameObject settingsPanel;
//    [SerializeField] private GameObject customizationPanel;

//    public void OpenCustomization()
//    {
//        // Close previous panels.
//        if (pausePanel != null)
//            pausePanel.SetActive(false);

//        if (settingsPanel != null)
//            settingsPanel.SetActive(false);

//        // Open customization.
//        if (customizationPanel != null)
//            customizationPanel.SetActive(true);
//    }

//    public void CloseCustomization()
//    {
//        if (customizationPanel != null)
//            customizationPanel.SetActive(false);

//        // Return to Settings only.
//        if (settingsPanel != null)
//            settingsPanel.SetActive(true);
//    }
//}

using UnityEngine;

public class ControlCustomizationNavigation : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseOverlay;     // the dark full-screen overlay used by PauseManager
    [SerializeField] private GameObject settingsPanel;

    [Header("Customizer")]
    [SerializeField] private ControlLayoutCustomizer customizer;

    // "Custom Controller" button -> OnClick
    public void OpenCustomization()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // The overlay is a full-screen raycast target: if it stays on it can swallow every
        // touch meant for the draggable controls.
        if (pauseOverlay != null) pauseOverlay.SetActive(false);

        if (customizer != null)
            customizer.EnterEditMode();      // <- this is what turns editing ON (IsEditing = true)
    }

    // "SAVE LAYOUT" button -> OnClick (use this instead of calling customizer.SaveLayout directly)
    public void SaveAndClose()
    {
        if (customizer != null) customizer.SaveLayout();
        ReturnToSettings();
    }

    // "CANCEL" buttons -> OnClick
    public void CancelAndClose()
    {
        if (customizer != null) customizer.CancelEditing();
        ReturnToSettings();
    }

    private void ReturnToSettings()
    {
        if (pauseOverlay != null) pauseOverlay.SetActive(true);   // game is still paused
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
}