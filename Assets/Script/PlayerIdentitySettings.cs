
using TMPro;
using UnityEngine;

public class PlayerIdentitySettings : MonoBehaviour
{
    private const string PlayerNameKey = "GD_PlayerName";
    private const string NicknameKey = "GD_PlayerNickname";

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField nicknameInput;

    [Header("Profile Preview")]
    [SerializeField] private TMP_Text playerNamePreview;
    [SerializeField] private TMP_Text nicknamePreview;

    [Header("Panel")]
    [SerializeField] private GameObject playerSettingsPanel;

    private string savedPlayerName;
    private string savedNickname;

    private void Start()
    {
        LoadProfile();
    }

    // Called by Apply button.
    public void ApplyChanges()
    {
        SaveProfile();
        UpdatePreview();
        Debug.Log("Player identity applied.");
    }

    // Called by Save button.
    public void SaveChanges()
    {
        SaveProfile();
        UpdatePreview();

        // Save avatar selection too, if already stored by its selector.
        PlayerPrefs.Save();

        Debug.Log("Player profile saved successfully.");

        // Close the Player Settings panel.
        if (playerSettingsPanel != null)
        {
            playerSettingsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "Player Settings Panel is not assigned.");
        }
    }
    private void SaveProfile()
    {
        string playerName = playerNameInput != null
            ? playerNameInput.text.Trim()
            : savedPlayerName;

        string nickname = nicknameInput != null
            ? nicknameInput.text.Trim()
            : savedNickname;

        // Don't overwrite existing values with empty input.
        if (!string.IsNullOrWhiteSpace(playerName))
            savedPlayerName = playerName;

        if (!string.IsNullOrWhiteSpace(nickname))
            savedNickname = nickname;

        PlayerPrefs.SetString(
            PlayerNameKey, savedPlayerName);

        PlayerPrefs.SetString(
            NicknameKey, savedNickname);

        PlayerPrefs.Save();

        if (playerNameInput != null)
            playerNameInput.SetTextWithoutNotify(savedPlayerName);

        if (nicknameInput != null)
            nicknameInput.SetTextWithoutNotify(savedNickname);
    }

    public void LoadProfile()
    {
        savedPlayerName = PlayerPrefs.GetString(
            PlayerNameKey, "Player");

        savedNickname = PlayerPrefs.GetString(
            NicknameKey, "Rookie");

        if (playerNameInput != null)
            playerNameInput.SetTextWithoutNotify(savedPlayerName);

        if (nicknameInput != null)
            nicknameInput.SetTextWithoutNotify(savedNickname);

        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (playerNamePreview != null)
            playerNamePreview.text = savedPlayerName;

        if (nicknamePreview != null)
            nicknamePreview.text = savedNickname;
    }
}
