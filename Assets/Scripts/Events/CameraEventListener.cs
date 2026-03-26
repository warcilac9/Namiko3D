using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

[System.Serializable]
public class CinemachineCameraPairEvent : UnityEvent<CinemachineCamera, CinemachineCamera> {}

public class CameraEventListener : MonoBehaviour
{
    public EventSOCamera gameEvent;
    public CinemachineCameraPairEvent response;

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

    public void invokeEvent(CinemachineCamera mainCam, CinemachineCamera secondCam)
    {
        response?.Invoke(mainCam, secondCam);
    }
}
