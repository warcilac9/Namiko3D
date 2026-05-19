using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemySpawnData
{
    public int EnemiesToSpawn;
    public int RangedEnemiesToSpawn;
    public int MaxBasicEnemies;
    public int MaxRangedEnemies;
    public Transform Origin;
    
}

[System.Serializable]
public class EnemySpawnPairEvent : UnityEvent<EnemySpawnData> {}

public class EnemySpawnListener : MonoBehaviour
{
    public EventSOEnemySpawn gameEvent;
    public EnemySpawnPairEvent response;

    void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.Register(this);
        }
    }

    void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.Unregister(this);
        }
    }

    public void InvokeEvent(int enemiesToSpawn, int rangedEnemiesToSpawn, int maxBasicEnemies,int maxRangedEnemies, Transform origin)
    {
        var data = new EnemySpawnData
        {
            EnemiesToSpawn = enemiesToSpawn,
            RangedEnemiesToSpawn = rangedEnemiesToSpawn,
            MaxBasicEnemies = maxBasicEnemies,
            MaxRangedEnemies = maxRangedEnemies,
            Origin = origin
        };

        response?.Invoke(data);
    }
}
