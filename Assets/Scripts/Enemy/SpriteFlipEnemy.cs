using UnityEngine;

public class SpriteFlipEnemy : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform player;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Transform[] flipTargets;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField] private float flipThreshold = 0.01f;
    [SerializeField] private bool useRotationFlip = true;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (player == null)
        {
            GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
            if (taggedPlayer != null)
            {
                player = taggedPlayer.transform;
            }
            if (player == null)
            {
                Debug.LogWarning("SpriteFlipEnemy: Player Transform not found. Assign it in the inspector.", this);
            }
        }
    }

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
