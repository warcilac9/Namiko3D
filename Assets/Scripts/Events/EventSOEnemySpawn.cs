using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New  Enemy Spawn Event", menuName = "Enemy Spawn Event" , order = 54)]

public class EventSOEnemySpawn : ScriptableObject
{

    private List<EnemySpawnListener> eventListeners = new List<EnemySpawnListener>();

    public void Register(EnemySpawnListener listener)
    {
        eventListeners.Add(listener);
    }

    public void Unregister(EnemySpawnListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void Occurred(int enemiesToSpawn, int rangedEnemiesToSpawn, int maxBasicEnemies,int maxRangedEnemies, Transform origin)
    {
        for(int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].InvokeEvent(enemiesToSpawn, rangedEnemiesToSpawn, maxBasicEnemies, maxRangedEnemies, origin);
        }
    }
}
