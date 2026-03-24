using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    public InputHandler inputHandler;
    [SerializeField] Rigidbody rb;
    public groundCheck groundCheck;

    public float jumpForce;
    

    void OnEnable()
    {
        inputHandler.jumping += isJump;
    }
    void OnDisable()
    {
        inputHandler.jumping -= isJump;
    }

    public void isJump()
    {
        if(groundCheck.isGrounded == true)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.y, jumpForce);
        }
        
        
    }
}
