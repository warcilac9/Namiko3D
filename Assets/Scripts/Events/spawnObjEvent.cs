using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Object Spawn Event", menuName = "Object Spawn Event", order = 56)]
public class spawnObjEvent : ScriptableObject
{
    private List<spawnObjListener> eventListeners = new List<spawnObjListener>();

    public void Register(spawnObjListener listener)
    {
        eventListeners.Add(listener);
    }

    public void Unregister(spawnObjListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void Occurred(Transform origin)
    {
        for (int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].InvokeEvent(origin);
        }
    }
}
