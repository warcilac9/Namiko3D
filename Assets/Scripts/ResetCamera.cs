using UnityEngine;
using Unity.Cinemachine;

public class ResetCamera : MonoBehaviour
{
    [SerializeField] CinemachineCamera mainCam;
    [SerializeField] CinemachineCamera currentCam;
    [SerializeField] CinemachineBrain brain;

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

        ResetToMainCamera();
    }

    public void ResetToMainCamera()
    {
        if (brain == null || mainCam == null)
        {
            return;
        }

        currentCam = brain.ActiveVirtualCamera as CinemachineCamera;

        if (currentCam != null && currentCam != mainCam)
        {
            currentCam.Priority = 0;
        }

        mainCam.Priority = 10;
    }
}
