using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private EnemyPoolManager enemyPoolManager;

    private void Awake()
    {
        if (enemyPoolManager == null)
        {
            enemyPoolManager = FindFirstObjectByType<EnemyPoolManager>();
        }
    }

    public void spawnEnemies(int enemiesToSpawn,int maxEnemies, Transform origin)
    {
        if (enemyPoolManager == null)
        {
            return;
        }

        enemyPoolManager.StartWave(enemiesToSpawn, maxEnemies, origin);
    }
}
