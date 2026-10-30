
using UnityEngine;

public class LevelSpawnedEnemy : MonoBehaviour
{
    private LevelWaveRunner owner;
    private bool reported;

    public void Initialize(LevelWaveRunner runner)
    {
        owner = runner;
    }

    public void ReportDefeated()
    {
        if (reported)
            return;

        reported = true;

        if (owner != null)
            owner.NotifyEnemyDefeated(this);
    }

    private void OnDestroy()
    {
        // Safety for enemies removed by other gameplay logic.
        // Normal kills should call ReportDefeated from Die().
        if (!reported && owner != null)
            owner.NotifyEnemyDefeated(this);

        reported = true;
    }
}
