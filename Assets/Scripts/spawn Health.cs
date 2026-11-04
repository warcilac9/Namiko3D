using UnityEngine;

public class spawnHealth : MonoBehaviour
{
    public GenericPoolManager healthSpawn;

    public void spawnHealthObj(Transform origin)
    {
        healthSpawn.RequestObject(origin);
    }
}
