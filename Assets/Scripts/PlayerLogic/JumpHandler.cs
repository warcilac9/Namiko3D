using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    public InputHandler inputHandler;
    [SerializeField] Rigidbody rb;

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
        Debug.Log("is jumping");
        rb.linearVelocity = new Vector3(rb.linearVelocity.y, jumpForce);
        
    }
}
