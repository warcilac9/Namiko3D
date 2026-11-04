using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericPoolManager : MonoBehaviour
{
    //This is the prefab that will be spawned and used in the pool
    [SerializeField]
    private GameObject prefab;

    //The amount of objects that will be instanced when the pool is initialized
    [SerializeField]
    private int poolSize = 3;

    //The list of objects spawned in the game
    private List<GameObject> objList;

    //This is where the objects will be instanced in the world
    private Transform poolOrigin;

    //This bool tell the code if the pure has been initial;ized
    private bool isPoolInitialized;

    //This function checks if the pool is initialized, if it has not been initialized then it will check if the object list exists, if not, then it will create one, and add the established number of objects to the list.
    private void EnsurePoolIsInitialized()
    {
        if (!isPoolInitialized)
        {
            poolOrigin = transform;
            if (objList == null)
            {
                objList = new List<GameObject>();
            }
            AddObjectsToPool(poolSize);
            isPoolInitialized = true;
        }
    }

    //This function adds objects to the object list, it requiers an amount of objects. When an object is inbstanced, it deactivates itself, and moves to the origin.
    private void AddObjectsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject Object = Instantiate(prefab, poolOrigin);
            Object.SetActive(false);
            objList.Add(Object);
            Object.transform.parent = transform;
        }
    }

    //This function searches the object list for a deactivated object, if it finds one, then it send this object to the function that spawns it where needed, if it doesnt find any deactivated object, then it will call the AddObjectsToPool function to create a new game object, and use this game object
    public GameObject RequestObject(Transform position)
    {
        EnsurePoolIsInitialized();
        for (int i = 0; i < objList.Count; i++)
        {
            if (!objList[i].activeSelf)
            {
                SetObjectSpawn(objList[i], position);
                return objList[i];
            }
        }
        AddObjectsToPool(1);
        GameObject newObject = objList[objList.Count - 1];
        SetObjectSpawn(newObject, position);
        return newObject;
    }

    // This function first moves the object that will be spawned to the location it will be spawned, then it activates the object
    private void SetObjectSpawn(GameObject objectToSpawn, Transform spawnPoint)
    {
        objectToSpawn.transform.position = spawnPoint.position;
        objectToSpawn.SetActive(true);
    }
}
