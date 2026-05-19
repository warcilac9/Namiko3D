using System.Collections.Generic;
using UnityEngine;
// ========== TEMPORARY: Remove when feature is no longer needed ==========
using UnityEngine.SceneManagement;
// =========================================================================


public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private GameObject rangedPrefab;
    private int poolSize = 10;
    [SerializeField] private List<GameObject> bulletList;
    [SerializeField] private List<GameObject> rangedEnemyList;
    [SerializeField] Transform origin;
    [SerializeField] EventSO eventSO;

    private int pendingEnemies;
    private int pendingRangedEnemies;
    private int maxBasicActiveEnemies;
    private int maxRangedActiveEnemies;
    private bool allEnemiesDefeatedLogged;
    private Transform currentWaveSpawnPoint;
    private bool isPoolInitialized = false;

    [Header("[ TEMPORARY: Easy to Remove Later ]")]
    [SerializeField] private bool isLastHorde = false;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // ========== TEMPORARY: Remove when feature is no longer needed ==========
    public void SetIsLastHorde(bool value)
    {
        isLastHorde = value;
    }
    // =========================================================================

    void Start()
    {
        if (bulletList == null)
        {
            bulletList = new List<GameObject>();
        }

        if (rangedEnemyList == null)
        {
            rangedEnemyList = new List<GameObject>();
        }

        if (origin == null)
        {
            origin = transform;
        }

        // Don't initialize pool at startup - do it lazily when needed
    }

    private void AddObjectsToPool(int amount, GameObject prefab, List<GameObject> pool)
    {
        if (prefab == null || pool == null)
        {
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject Object = Instantiate(prefab, origin.transform);
            Object.SetActive(false);
            pool.Add(Object);
            Object.transform.parent = transform;
        }
    }

    public void StartWave(
        int enemiesToSpawn,
        int rangedEnemiesToSpawn,
        int maxBasicEnemies,
        int maxRangedEnemies,
        Transform spawnPoint = null)
    {
        EnsurePoolInitialized();
        
        allEnemiesDefeatedLogged = false;
        pendingEnemies = Mathf.Max(0, enemiesToSpawn);
        pendingRangedEnemies = Mathf.Max(0, rangedEnemiesToSpawn);
        maxBasicActiveEnemies = Mathf.Max(0, maxBasicEnemies);
        maxRangedActiveEnemies = Mathf.Max(0, maxRangedEnemies);
        currentWaveSpawnPoint = spawnPoint != null ? spawnPoint : origin;

        TrySpawnPending(currentWaveSpawnPoint);
        CheckWaveCompletion();
    }

    private void EnsurePoolInitialized()
    {
        if (!isPoolInitialized)
        {
            AddObjectsToPool(poolSize, Prefab, bulletList);
            AddObjectsToPool(poolSize, rangedPrefab, rangedEnemyList);
            isPoolInitialized = true;
        }
    }

    public GameObject RequesObject(Transform position)
    {
        return RequestObjectFromPool(position, bulletList, Prefab);
    }

    public GameObject RequestRangedObject(Transform position)
    {
        return RequestObjectFromPool(position, rangedEnemyList, rangedPrefab);
    }

    private GameObject RequestObjectFromPool(Transform position, List<GameObject> pool, GameObject prefab)
    {
        if (pool == null || prefab == null)
        {
            return null;
        }

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                SetEnemySpawn(pool[i], position);
                return pool[i];
            }
        }

        AddObjectsToPool(1, prefab, pool);
        SetEnemySpawn(pool[pool.Count - 1], position);
        return pool[pool.Count - 1];
    }

    private void SetEnemySpawn(GameObject enemy, Transform spawnPoint)
    {
        enemy.transform.position = spawnPoint.position;

        enemyHealth health = enemy.GetComponent<enemyHealth>();
        if (health != null)
        {
            health.OnEnemyDefeated -= HandleEnemyDefeated;
            health.OnEnemyDefeated += HandleEnemyDefeated;
        }

        enemy.SetActive(true);
    }

    private void HandleEnemyDefeated(enemyHealth _) 
    {
        TrySpawnPending(currentWaveSpawnPoint != null ? currentWaveSpawnPoint : origin);
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        int activeBasicEnemies = CountActiveEnemiesInPool(bulletList);
        int activeRangedEnemies = CountActiveEnemiesInPool(rangedEnemyList);
        int totalActiveEnemies = activeBasicEnemies + activeRangedEnemies;

        if (pendingEnemies <= 0 && pendingRangedEnemies <= 0 && totalActiveEnemies <= 0 && !allEnemiesDefeatedLogged)
        {
            allEnemiesDefeatedLogged = true;
            AllEnemiesDefeated();
        }
    }

    public void AllEnemiesDefeated()
    {
        eventSO.Occurred();

        // ========== TEMPORARY: Remove this entire if block when feature is no longer needed ==========
        // To remove: Delete this entire if statement and the mainMenuSceneName field above
        if (isLastHorde)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        // ==============================================================================================
    }

    private void TrySpawnPending(Transform spawnPoint)
    {
        while (pendingEnemies > 0 || pendingRangedEnemies > 0)
        {
            int activeBasicEnemies = CountActiveEnemiesInPool(bulletList);
            int activeRangedEnemies = CountActiveEnemiesInPool(rangedEnemyList);

            bool canSpawnBasic = pendingEnemies > 0 && activeBasicEnemies < maxBasicActiveEnemies;
            bool canSpawnRanged = pendingRangedEnemies > 0 && activeRangedEnemies < maxRangedActiveEnemies;

            if (!canSpawnBasic && !canSpawnRanged)
            {
                break;
            }

            if (canSpawnBasic)
            {
                if (RequesObject(spawnPoint) != null)
                {
                    pendingEnemies--;
                }
            }

            if (canSpawnRanged)
            {
                if (RequestRangedObject(spawnPoint) != null)
                {
                    pendingRangedEnemies--;
                }
            }
        }
    }

    private int CountActiveEnemiesInPool(List<GameObject> pool)
    {
        if (pool == null)
        {
            return 0;
        }

        int count = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null && pool[i].activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
