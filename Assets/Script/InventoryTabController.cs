
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabController : MonoBehaviour
{
    [System.Serializable]
    public class InventoryTab
    {
        public string tabName;
        public Button tabButton;
        public GameObject contentPanel;

        [Header("Optional tab sprite swap")]
        public Image tabImage;
        public Sprite normalSprite;
        public Sprite selectedSprite;
    }

    [Header("Inventory Tabs")]
    [SerializeField] private InventoryTab[] tabs;

    [Header("Default Selected Tab")]
    [SerializeField] private int defaultTabIndex = 0;

    private int currentTabIndex = -1;

    private void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;

            if (tabs[i].tabButton != null)
            {
                tabs[i].tabButton.onClick.AddListener(
                    () => SelectTab(index)
                );
            }
        }

        SelectTab(defaultTabIndex);
    }

    public void SelectTab(int index)
    {
        if (tabs == null || index < 0 || index >= tabs.Length)
            return;

        currentTabIndex = index;

        for (int i = 0; i < tabs.Length; i++)
        {
            bool isSelected = (i == currentTabIndex);

            if (tabs[i].contentPanel != null)
                tabs[i].contentPanel.SetActive(isSelected);

            if (tabs[i].tabImage != null)
            {
                Sprite sprite = isSelected
                    ? tabs[i].selectedSprite
                    : tabs[i].normalSprite;

                if (sprite != null)
                    tabs[i].tabImage.sprite = sprite;
            }

            if (tabs[i].tabButton != null)
                tabs[i].tabButton.interactable = !isSelected;
        }
    }
}
