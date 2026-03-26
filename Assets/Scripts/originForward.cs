using UnityEngine;

public class originForward : MonoBehaviour
{
    private SpriteHandler spriteHandler;

    void Start()
    {
        spriteHandler = transform.parent?.GetComponent<SpriteHandler>();
        if (spriteHandler == null)
        {
            Debug.LogWarning("originForward: Parent does not have SpriteHandler component!");
        }
    }

    void LateUpdate()
    {
        if (transform.parent == null) return;

        // Always match parent rotation
        transform.rotation = transform.parent.rotation;

        // Adjust based on sprite facing direction
        if (spriteHandler != null && !spriteHandler.isRight)
        {
            // If not facing right, flip 180 degrees on Y-axis so transform.right points left
            transform.Rotate(0, 180, 0, Space.Self);
        }
    }

    // Optional helper for bullet spawn direction
    public Vector3 GetFireDirection()
    {
        return transform.right;
    }
}
