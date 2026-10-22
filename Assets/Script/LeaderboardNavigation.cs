
using UnityEngine;

public class LeaderboardNavigation : MonoBehaviour
{
    [Header("Leaderboard Panels")]
    [SerializeField] private GameObject regionRankPanel;
    [SerializeField] private GameObject globalRankPanel;

    [Header("Optional Additional Pages")]
    [Tooltip("Add leaderboard pages in order, for example Friends or Top Players.")]
    [SerializeField] private GameObject[] leaderboardPages;

    private int currentPageIndex = 0;

    private void Start()
    {
        ShowRegionRank();
    }

    // Global button
    public void ShowGlobalRank()
    {
        SetActivePanel(globalRankPanel);
    }

    // Back arrow: return to Region Rank
    public void ShowRegionRank()
    {
        currentPageIndex = 0;
        SetActivePanel(regionRankPanel);
    }

    // Right arrow: move to the next leaderboard page
    public void NextPage()
    {
        if (leaderboardPages == null || leaderboardPages.Length == 0)
        {
            Debug.LogWarning("No additional leaderboard pages are assigned.");
            return;
        }

        currentPageIndex++;

        if (currentPageIndex >= leaderboardPages.Length)
            currentPageIndex = 0;

        SetActivePanel(leaderboardPages[currentPageIndex]);
    }

    // Optional left arrow: move to the previous page
    public void PreviousPage()
    {
        if (leaderboardPages == null || leaderboardPages.Length == 0)
            return;

        currentPageIndex--;

        if (currentPageIndex < 0)
            currentPageIndex = leaderboardPages.Length - 1;

        SetActivePanel(leaderboardPages[currentPageIndex]);
    }

    private void SetActivePanel(GameObject targetPanel)
    {
        if (targetPanel == null)
        {
            Debug.LogError("Leaderboard panel is not assigned in the Inspector.");
            return;
        }

        if (regionRankPanel != null)
            regionRankPanel.SetActive(false);

        if (globalRankPanel != null)
            globalRankPanel.SetActive(false);

        if (leaderboardPages != null)
        {
            foreach (GameObject page in leaderboardPages)
            {
                if (page != null)
                    page.SetActive(false);
            }
        }

        targetPanel.SetActive(true);
    }
}
