using UnityEngine;
using System.Collections.Generic;

public class RangedProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float flightDuration = 2f;
    [SerializeField] private float arcHeight = 2f;

    private readonly List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if(projectilePrefab == null)
        {
            return;
        }

        for(int i = 0; i < poolSize; i++)
        {
            CreateProjectile();
        }
    }

    private GameObject CreateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform);
        projectile.SetActive(false);
        pool.Add(projectile);
        return projectile;
    }

    public void RequestProjectile(Vector3 targetPosition)
    {
        GameObject projectile = GetAvailableProjectile();
        if(projectile == null)
        {
            projectile = CreateProjectile();
        }

        Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;

        RangedProjectile projectileScript = projectile.GetComponent<RangedProjectile>();
        if(projectileScript != null)
        {
            projectileScript.Launch(spawnPosition, targetPosition, flightDuration, arcHeight);
        }
        else
        {
            projectile.transform.position = spawnPosition;
            projectile.SetActive(true);
        }
    }

    private GameObject GetAvailableProjectile()
    {
        for (int i=0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                return pool[i];
            }
        }

        return null;
    }

}
