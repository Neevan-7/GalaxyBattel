
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatarSelector : MonoBehaviour
{
    private const string AvatarKey = "GD_AvatarIndex";

    [Header("Profile Preview")]
    [SerializeField] private Image profileAvatar;

    [Header("Avatar Selection Buttons")]
    [SerializeField] private Button[] avatarButtons;

    [Header("Optional Save Button")]
    [SerializeField] private Button saveButton;

    private int selectedAvatarIndex;

    private void Awake()
    {
        // Connect every avatar button automatically.
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;

            if (avatarButtons[index] != null)
            {
                avatarButtons[index].onClick.AddListener(
                    () => SelectAvatar(index));
            }
        }

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveAvatar);
    }

    private void Start()
    {
        selectedAvatarIndex = PlayerPrefs.GetInt(
            AvatarKey, 0);

        // Restore the saved avatar.
        ApplyAvatar(selectedAvatarIndex);
    }

    public void SelectAvatar(int index)
    {
        if (index < 0 || index >= avatarButtons.Length)
            return;

        Button selectedButton = avatarButtons[index];

        if (selectedButton == null)
            return;

        Image avatarImage =
            selectedButton.targetGraphic as Image;

        if (avatarImage == null || avatarImage.sprite == null)
        {
            Debug.LogWarning(
                "Avatar button " + index +
                " has no sprite assigned.");
            return;
        }

        selectedAvatarIndex = index;

        // Update the large profile preview immediately.
        profileAvatar.sprite = avatarImage.sprite;
        profileAvatar.enabled = true;

        // Save automatically when an avatar is selected.
        SaveAvatar();
    }

    private void ApplyAvatar(int index)
    {
        if (avatarButtons == null ||
            index < 0 ||
            index >= avatarButtons.Length)
        {
            Debug.LogWarning("Saved avatar index is invalid.");
            return;
        }

        Button button = avatarButtons[index];

        if (button == null)
            return;

        Image avatarImage = button.targetGraphic as Image;

        if (avatarImage == null || avatarImage.sprite == null)
            return;

        profileAvatar.sprite = avatarImage.sprite;
        profileAvatar.enabled = true;
    }

    public void SaveAvatar()
    {
        PlayerPrefs.SetInt(AvatarKey, selectedAvatarIndex);
        PlayerPrefs.Save();

        Debug.Log(
            "Avatar saved. Index: " + selectedAvatarIndex);
    }

    private void OnDestroy()
    {
        // Prevent duplicate listeners if this component is reused.
        if (avatarButtons != null)
        {
            foreach (Button button in avatarButtons)
            {
                if (button != null)
                    button.onClick.RemoveAllListeners();
            }
        }

        if (saveButton != null)
            saveButton.onClick.RemoveListener(SaveAvatar);
    }
}
