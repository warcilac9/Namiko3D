using UnityEngine;

public class SpriteToCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] private bool isBullet = false;
    [SerializeField] private string bulletTag = "Bullet"; // Tag to auto-detect bullets

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    
    void LateUpdate()
    {
        if (mainCamera == null)
            return;

        Vector3 directionToCamera = mainCamera.transform.position - transform.position;
        
        if (directionToCamera != Vector3.zero)
        {
            if (isBullet)
            {
                // Bullet: Face camera directly on all axes
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                transform.rotation = targetRotation;
            }
            else
            {
                // Player: Only rotate on Y axis
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                float targetYRotation = targetRotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
            }
        }
    }
}
