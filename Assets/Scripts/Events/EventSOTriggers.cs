using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Trigger Event", menuName = "Trigger Event" , order = 55)]

public class EventSOTriggers : ScriptableObject
{

    private List<TriggersListener> eventListeners = new List<TriggersListener>();

    public void Register(TriggersListener listener)
    {
        eventListeners.Add(listener);
    }

    public void Unregister(TriggersListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void Occurred(GameObject triggerObj, GameObject limit1, GameObject limit2)
    {
        for(int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].invokeEvent(triggerObj, limit1, limit2);
        }
    }
}
