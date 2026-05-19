using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;

    public void spawnEnemies(EnemySpawnData data)
    {
        if (data == null)
        {
            return;
        }

        if (enemyPoolManager == null)
        {
            enemyPoolManager = FindFirstObjectByType<EnemyPoolManager>();
        }

        if (enemyPoolManager == null)
        {
            return;
        }

        enemyPoolManager.StartWave(data.EnemiesToSpawn, data.RangedEnemiesToSpawn, data.MaxBasicEnemies, data.MaxRangedEnemies, data.Origin);
    }
}
