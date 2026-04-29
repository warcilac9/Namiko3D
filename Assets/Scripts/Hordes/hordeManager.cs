using UnityEngine;
using Unity.Cinemachine;

public class hordeManager : MonoBehaviour
{
    CinemachineCamera mainCame, hordeCame;

    public void changeToHordeCamera(CinemachineCamera mainCam, CinemachineCamera hordeCam)
    {
        if (mainCam != null && hordeCam != null)
        {
            mainCame = mainCam;
            hordeCame = hordeCam;
            mainCame.Priority = 0;
            hordeCame.Priority = 10;
        }
    }

    public void changeToMainCamera()
    {
        mainCame.Priority = 10;
        hordeCame.Priority = 0;
    }
}
