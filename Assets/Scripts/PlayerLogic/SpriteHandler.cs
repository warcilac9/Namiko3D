using Unity.Mathematics;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform playerSprite;
    [SerializeField] Camera mainCamera;
    public bool isRight;
    
    [SerializeField]private float flipThreshold = 0.1f;

    void Update()
    {
        Vector3 velocity = rb.linearVelocity;
        
        Vector3 cameraRight = mainCamera.transform.right;
        
        float horizontalSpeed = Vector3.Dot(velocity, cameraRight);
        
        if (Mathf.Abs(horizontalSpeed) > flipThreshold)
        {
            Vector3 currentScale = playerSprite.localScale;
            
            if (horizontalSpeed < 0)
            {
                currentScale.x = Mathf.Abs(currentScale.x);
                isRight = true;
            }
            else if (horizontalSpeed > 0)
            {
                currentScale.x = -Mathf.Abs(currentScale.x);
                isRight = false;
            }
            
            playerSprite.localScale = currentScale;
        }
    }
}
