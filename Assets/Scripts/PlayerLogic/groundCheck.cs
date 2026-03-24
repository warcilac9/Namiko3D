using UnityEngine;

[RequireComponent(typeof(Collider))]
public class groundCheck : MonoBehaviour
{
    [Tooltip("Optional reference to the trigger collider used for ground checks.")]
    public Collider groundTriggerCollider;

    [Tooltip("True when grounded.")]
    public bool isGrounded;

    [Tooltip("Layers considered ground for this check.")]
    public LayerMask layerMask;

    private int groundContactCount;

    void Reset()
    {
        groundTriggerCollider = GetComponent<Collider>();
        if (groundTriggerCollider != null)
            groundTriggerCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            groundContactCount++;
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            groundContactCount = Mathf.Max(groundContactCount - 1, 0);
            isGrounded = (groundContactCount > 0);
        }
    }
}
