using UnityEngine;

public class SpriteToCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private bool isBullet = false;

    [Header("Directional Facing")]
    [SerializeField, Min(2)]
    private int directionCount = 8;

    [SerializeField, Min(0f)]
    private float hysteresisDegrees = 6f;

    [SerializeField, Min(0f)]
    private float turnSpeed = 360f;

    private float targetYaw;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null || isBullet)
        {
            return;
        }

        float cameraYaw = GetCameraYaw();
        float sectorSize = 360f / Mathf.Max(2, directionCount);

        targetYaw = Mathf.Round(cameraYaw / sectorSize) * sectorSize;
        transform.rotation = Quaternion.Euler(0f, targetYaw, 0f);
    }

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            return;
        }

        if (isBullet)
        {
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;

            if (directionToCamera.sqrMagnitude < 0.0001f)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(directionToCamera);
            return;
        }

        float sectorSize = 360f / Mathf.Max(2, directionCount);
        float maxHysteresis = sectorSize * 0.49f;
        float hysteresis = Mathf.Clamp(hysteresisDegrees, 0f, maxHysteresis);
        float switchThreshold = sectorSize * 0.5f + hysteresis;

        float cameraYaw = GetCameraYaw();
        float angleFromTarget = Mathf.DeltaAngle(targetYaw, cameraYaw);

        while (Mathf.Abs(angleFromTarget) > switchThreshold)
        {
            targetYaw += Mathf.Sign(angleFromTarget) * sectorSize;
            angleFromTarget = Mathf.DeltaAngle(targetYaw, cameraYaw);
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    private float GetCameraYaw()
    {
        Vector3 directionToCamera = -mainCamera.transform.forward;
        directionToCamera.y = 0f;

        if (directionToCamera.sqrMagnitude < 0.0001f)
        {
            return targetYaw;
        }

        return Quaternion.LookRotation(directionToCamera).eulerAngles.y;
    }
}
