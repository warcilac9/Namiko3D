using UnityEngine;

public class LASTHORDE : MonoBehaviour
{
    private EnemyPoolManager enemyPoolManager;
    private bool hasTriggered = false;

    void Start()
    {
        enemyPoolManager = FindFirstObjectByType<EnemyPoolManager>();
        if (enemyPoolManager == null)
        {
            Debug.LogWarning("LASTHORDE: EnemyPoolManager not found in scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (enemyPoolManager != null)
        {
            hasTriggered = true;
            enemyPoolManager.SetIsLastHorde(true);
            Debug.Log("LASTHORDE: Last horde flag activated!");
        }
    }
}
