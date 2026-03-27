using System;
using System.Collections.Generic;
using UnityEngine;
// ========== TEMPORARY: Remove when feature is no longer needed ==========
using UnityEngine.SceneManagement;
// =========================================================================


public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    private int poolSize = 10;
    [SerializeField] private List<GameObject> bulletList;
    [SerializeField] Transform origin;
    [SerializeField] EventSO eventSO;

    private int pendingEnemies;
    private int activeEnemies;
    private int maxActiveEnemies;
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

        if (origin == null)
        {
            origin = transform;
        }

        // Don't initialize pool at startup - do it lazily when needed
    }

    private void AddObjectsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject Object = Instantiate(Prefab, origin.transform);
            Object.SetActive(false);
            bulletList.Add(Object);
            Object.transform.parent = transform;
        }
    }

    public void StartWave(int enemiesToSpawn, int maxEnemies, Transform spawnPoint = null)
    {
        EnsurePoolInitialized();
        
        allEnemiesDefeatedLogged = false;
        pendingEnemies = Mathf.Max(0, enemiesToSpawn);
        activeEnemies = CountActiveEnemies();
        maxActiveEnemies = Mathf.Max(1, maxEnemies);
        currentWaveSpawnPoint = spawnPoint != null ? spawnPoint : origin;

        TrySpawnPending(currentWaveSpawnPoint);
        CheckWaveCompletion();
    }

    private void EnsurePoolInitialized()
    {
        if (!isPoolInitialized)
        {
            AddObjectsToPool(poolSize);
            isPoolInitialized = true;
        }
    }

    public GameObject RequesObject(Transform position)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeSelf)
            {
                SetEnemySpawn(bulletList[i], position);
                return bulletList[i];
            }
        }
        AddObjectsToPool(1);
        SetEnemySpawn(bulletList[bulletList.Count - 1], position);
        return bulletList[bulletList.Count - 1];
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
        activeEnemies = Mathf.Max(0, activeEnemies - 1);
        TrySpawnPending(currentWaveSpawnPoint != null ? currentWaveSpawnPoint : origin);
        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if (pendingEnemies <= 0 && activeEnemies <= 0 && !allEnemiesDefeatedLogged)
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
        while (pendingEnemies > 0 && activeEnemies < maxActiveEnemies)
        {
            RequesObject(spawnPoint);
            pendingEnemies--;
            activeEnemies++;
        }
    }

    private int CountActiveEnemies()
    {
        int count = 0;
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] != null && bulletList[i].activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
