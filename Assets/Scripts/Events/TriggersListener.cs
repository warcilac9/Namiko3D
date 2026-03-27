using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggersPairEvent : UnityEvent<GameObject, GameObject, GameObject> {}

public class TriggersListener : MonoBehaviour
{
    public EventSOTriggers gameEvent;
    public TriggersPairEvent response;

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

    public void invokeEvent(GameObject triggerObj, GameObject limit1, GameObject limit2)
    {
        response?.Invoke(triggerObj, limit1, limit2);
    }
}
