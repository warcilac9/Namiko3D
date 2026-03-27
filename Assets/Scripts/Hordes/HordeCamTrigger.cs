using UnityEngine;
using Unity.Cinemachine;

public class HordeCamTrigger : MonoBehaviour
{
    public Collider hordeTrigger;
    public CinemachineCamera mainVcam;
    public CinemachineCamera vcam2;
    public EventSOCamera changeCamEvent;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            changeCamEvent.Occurred(mainVcam, vcam2);
        }
    }
}
