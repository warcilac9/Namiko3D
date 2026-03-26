using UnityEngine;
using Unity.Cinemachine;

public class hordeManager : MonoBehaviour
{
    public void changeToHordeCamera(CinemachineCamera mainCam, CinemachineCamera hordeCam)
    {
        if (mainCam != null && hordeCam != null)
        {
            mainCam.Priority = 0;
            hordeCam.Priority = 10;
        }
    }
}
