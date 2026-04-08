using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] private float shakeForce = 1;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        if(instance = null)
        {
            instance = this;
        }
    }
    void OnEnable()
    {
        playerHealth.onCameraShake += CameraShake;
    }
    void OnDisable()
    {
        playerHealth.onCameraShake -= CameraShake;
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
        
    }
}
