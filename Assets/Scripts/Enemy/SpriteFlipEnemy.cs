using UnityEngine;

public class SpriteFlipEnemy : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform player;

    [Tooltip("The transform that will be flipped (usually the sprite or model root). If empty, falls back to this GameObject's transform.")]
    [SerializeField] private Transform spriteTransform;

    [Tooltip("Additional transforms to flip/rotate along with the sprite (e.g., hitboxes, child visuals). If empty, only the sprite transform will be modified.")]
    [SerializeField] private Transform[] flipTargets;

    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [Tooltip("Minimum horizontal difference (in camera-space units) required to change the flip state.")]
    [SerializeField] private float flipThreshold = 0.01f;

    [Tooltip("If enabled, flipping is done via local Y rotation (+180 / 0) instead of inverting localScale.x. This avoids issues with NavMeshAgent and physics caused by negative scaling.")]
    [SerializeField] private bool useRotationFlip = true;

    private void Reset()
    {
        spriteTransform = transform;
        flipTargets = new[] { transform };
    }

    private void FixedUpdate()
    {
        if (player == null || spriteTransform == null)
            return;

        Camera cam = mainCamera != null ? mainCamera : Camera.main;
        if (cam == null)
            return;

        Vector3 toPlayer = player.position - transform.position;
        Vector3 cameraRight = cam.transform.right;
        float horizontalOffset = Vector3.Dot(toPlayer, cameraRight);

        if (Mathf.Abs(horizontalOffset) < flipThreshold)
            return;

        bool faceLeft = horizontalOffset < 0;

        if (useRotationFlip)
        {
            float targetY = faceLeft ? 180f : 0f;

            foreach (Transform t in GetFlipTargets())
            {
                Vector3 rot = t.localEulerAngles;
                rot.y = targetY;
                t.localEulerAngles = rot;
            }
        }
        else
        {
            foreach (Transform t in GetFlipTargets())
            {
                Vector3 currentScale = t.localScale;
                float absX = Mathf.Abs(currentScale.x);
                currentScale.x = faceLeft ? -absX : absX;
                t.localScale = currentScale;
            }
        }
    }

    private Transform[] GetFlipTargets()
    {
        if (flipTargets != null && flipTargets.Length > 0)
            return flipTargets;

        return new[] { spriteTransform ?? transform };
    }
}
