using UnityEngine;
using Unity.Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    public CinemachineCamera mainCamera;
    public CinemachineCamera nextCamera;
    public EventSOCamera eventSOCamera;
    [SerializeField] private CinemachineBrain brain;

    void Awake()
    {
        if (brain == null)
        {
            Camera sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                brain = sceneCamera.GetComponent<CinemachineBrain>();
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            if (eventSOCamera == null || mainCamera == null || nextCamera == null)
            {
                return;
            }

            ICinemachineCamera activeCamera = brain != null ? brain.ActiveVirtualCamera : null;
            if (ReferenceEquals(activeCamera, nextCamera))
            {
                eventSOCamera.Occurred(nextCamera, mainCamera);
            }
            else
            {
                eventSOCamera.Occurred(mainCamera, nextCamera);
            }
        }
    }
}
