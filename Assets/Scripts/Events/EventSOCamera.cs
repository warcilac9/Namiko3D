using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

[CreateAssetMenu(fileName = "New  Camera Event", menuName = "Camera Event" , order = 53)]

public class EventSOCamera : ScriptableObject
{

    private List<CameraEventListener> eventListeners = new List<CameraEventListener>();

    public void Register(CameraEventListener listener)
    {
        eventListeners.Add(listener);
    }

    public void Unregister(CameraEventListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void Occurred(CinemachineCamera mainCam, CinemachineCamera secondCam)
    {
        for(int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].invokeEvent(mainCam, secondCam);
        }
    }
}