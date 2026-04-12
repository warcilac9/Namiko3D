using UnityEngine;

[RequireComponent(typeof(Collider))]
public class groundCheck : MonoBehaviour
{
    [Tooltip("True when grounded.")]
    public bool isGrounded;

    [Tooltip("Layers considered ground for this check.")]
    public LayerMask layerMask;

    [Tooltip("Ray origin is lifted by this amount to avoid starting inside geometry.")]
    [SerializeField] private float rayOriginOffset = 0.05f;

    [Tooltip("How far below the origin we search for ground.")]
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Tooltip("Minimum surface normal Y to count as floor (1 = flat floor, 0 = vertical wall).")]
    [Range(0f, 1f)]
    [SerializeField] private float minGroundNormalY = 0.55f;

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * rayOriginOffset;
        float distance = groundCheckDistance + rayOriginOffset;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, distance, layerMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = hit.normal.y >= minGroundNormalY;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 origin = transform.position + Vector3.up * rayOriginOffset;
        Vector3 end = origin + Vector3.down * (groundCheckDistance + rayOriginOffset);

        Gizmos.DrawLine(origin, end);
        Gizmos.DrawWireSphere(end, 0.02f);
    }
}
