using System;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    private int poolSize = 3;
    [SerializeField] private List<GameObject> bulletList;
    [SerializeField] Transform origin;
    [SerializeField] private SpriteHandler spriteHandler;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (spriteHandler == null)
        {
            spriteHandler = origin.parent?.GetComponent<SpriteHandler>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
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

    public GameObject RequesObject(Transform position)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeSelf)
            {
                SetBulletDirectionAndSpawn(bulletList[i], position);
                return bulletList[i];
            }
        }
        AddObjectsToPool(1);
        SetBulletDirectionAndSpawn(bulletList[bulletList.Count - 1], position);
        return bulletList[bulletList.Count - 1];
    }

    private void SetBulletDirectionAndSpawn(GameObject bullet, Transform spawnPoint)
    {
        bullet.transform.position = spawnPoint.position;
        
        // Calculate bullet direction based on camera orientation and sprite facing direction
        NamikoMagic bulletScript = bullet.GetComponent<NamikoMagic>();
        if (bulletScript != null && spriteHandler != null && mainCamera != null)
        {
            // Aiming direction is based on camera's right direction and sprite flip
            Vector3 bulletDirection = spriteHandler.isRight 
                ? -mainCamera.transform.right 
                : mainCamera.transform.right;
            
            bulletScript.SetDirection(bulletDirection);
        }
        
        bullet.SetActive(true);
    }
}
