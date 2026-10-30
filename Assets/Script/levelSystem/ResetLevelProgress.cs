using UnityEngine;

public class ResetLevelProgress : MonoBehaviour
{
    private void Start()
    {
        LevelProgress.ResetAllProgress();

        Debug.Log("Level progression successfully reset!");

        Destroy(this);
    }
}