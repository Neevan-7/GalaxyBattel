
using UnityEngine;

public class ResetSettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject confirmPopup;

    public void OpenResetConfirmation()
    {
        if (confirmPopup != null)
            confirmPopup.SetActive(true);
        else
            Debug.LogError("Assign the confirmation popup.");
    }
}
