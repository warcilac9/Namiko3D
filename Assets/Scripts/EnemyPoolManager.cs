using System;
using System.Collections.Generic;
using UnityEngine;


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

        AddObjectsToPool(poolSize);
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
        allEnemiesDefeatedLogged = false;
        pendingEnemies = Mathf.Max(0, enemiesToSpawn);
        activeEnemies = CountActiveEnemies();
        maxActiveEnemies = Mathf.Max(1, maxEnemies);
        currentWaveSpawnPoint = spawnPoint != null ? spawnPoint : origin;

        TrySpawnPending(currentWaveSpawnPoint);
        CheckWaveCompletion();
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
