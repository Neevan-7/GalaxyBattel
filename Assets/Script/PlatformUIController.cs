
using UnityEngine;
using TMPro;

public class PlatformUIController : MonoBehaviour
{
    [Header("Platform Dropdown")]
    [SerializeField] private TMP_Dropdown platformDropdown;

    [Header("Platform UI")]
    [SerializeField] private GameObject canvasHandle;

    private void Awake()
    {
        if (platformDropdown != null)
        {
            platformDropdown.onValueChanged.AddListener(
                OnPlatformChanged);
        }
    }

    private void Start()
    {
        if (platformDropdown != null)
        {
            OnPlatformChanged(platformDropdown.value);
        }
        else
        {
            SetMobileUI(Application.isMobilePlatform);
        }
    }

    public void OnPlatformChanged(int index)
    {
        if (platformDropdown == null ||
            index < 0 ||
            index >= platformDropdown.options.Count)
            return;

        string selectedPlatform =
            platformDropdown.options[index].text.Trim();

        if (selectedPlatform.Equals(
            "Android",
            System.StringComparison.OrdinalIgnoreCase))
        {
            SetMobileUI(true);
        }
        else if (selectedPlatform.Equals(
            "Windows",
            System.StringComparison.OrdinalIgnoreCase))
        {
            SetMobileUI(false);
        }
    }

    private void SetMobileUI(bool mobile)
    {
        if (canvasHandle != null)
        {
            canvasHandle.SetActive(mobile);
        }
    }

    private void OnDestroy()
    {
        if (platformDropdown != null)
        {
            platformDropdown.onValueChanged.RemoveListener(
                OnPlatformChanged);
        }
    }
}
