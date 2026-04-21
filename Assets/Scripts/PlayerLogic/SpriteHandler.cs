using Unity.Mathematics;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    private MovementHandler movementHandler;
    [SerializeField] Transform playerSprite;
    [SerializeField] Camera mainCamera;
    public bool isRight;
    
    [SerializeField] private float flipThreshold = 0.1f;

    void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
        if (movementHandler == null)
        {
            movementHandler = GetComponentInParent<MovementHandler>();
        }
    }

    void Update()
    {
        if (movementHandler == null) return;

        Vector3 velocity = movementHandler.GetCurrentVelocity();
        
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
