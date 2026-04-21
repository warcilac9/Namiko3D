using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    public InputHandler inputHandler;
    public groundCheck groundCheck;
    private MovementHandler movementHandler;

    public float jumpForce = 5f;

    void Awake()
    {
        if (inputHandler == null)
            inputHandler = GetComponent<InputHandler>();

        movementHandler = GetComponent<MovementHandler>();
        if (movementHandler == null)
            movementHandler = GetComponentInParent<MovementHandler>();

        if (groundCheck == null)
            groundCheck = GetComponentInChildren<groundCheck>();
    }

    void OnEnable()
    {
        if (inputHandler != null)
            inputHandler.jumping += isJump;
    }

    void OnDisable()
    {
        if (inputHandler != null)
            inputHandler.jumping -= isJump;
    }

    public void isJump()
    {
        if (groundCheck != null && groundCheck.isGrounded && movementHandler != null)
        {
            movementHandler.ApplyJumpVelocity(jumpForce);
        }
    }
}
