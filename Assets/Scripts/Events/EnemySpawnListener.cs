using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemySpawnPairEvent : UnityEvent<int, int, Transform> {}

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

    public void invokeEvent(int enemiesToSpawn, int maxEnemies, Transform origin)
    {
        response?.Invoke(enemiesToSpawn, maxEnemies, origin);
    }
}
