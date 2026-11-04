using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpawnObjTransformEvent : UnityEvent<Transform> { }

public class spawnObjListener : MonoBehaviour
{
    public spawnObjEvent gameEvent;
    public SpawnObjTransformEvent response;

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

    public void InvokeEvent(Transform origin)
    {
        response?.Invoke(origin);
    }
}
