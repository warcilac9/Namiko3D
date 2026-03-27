using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;

    public void spawnEnemies(int enemiesToSpawn, int maxEnemies, Transform origin)
    {
        if (enemyPoolManager == null)
        {
            enemyPoolManager = FindFirstObjectByType<EnemyPoolManager>();
        }

        if (enemyPoolManager == null)
        {
            return;
        }

        enemyPoolManager.StartWave(enemiesToSpawn, maxEnemies, origin);
    }
}
