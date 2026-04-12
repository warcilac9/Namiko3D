using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] InputHandler input;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Camera mainCamera;
    [SerializeField] groundCheck groundCheck;

    public float moveSpeed;

    [Range(0,1.0f)]
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if(groundCheck.isGrounded){
            Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementDirection = (cameraRight * input.moveValX + cameraForward * input.moveValY).normalized;
        Vector3 moveVelocity = movementDirection * moveSpeed;

        _rb.linearVelocity = new Vector3(moveVelocity.x, _rb.linearVelocity.y, moveVelocity.z);
        }
        
    }

}
