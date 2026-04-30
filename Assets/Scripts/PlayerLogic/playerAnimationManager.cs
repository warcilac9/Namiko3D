using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public InputHandler inputHandler;
    public AttackManager attackManager;
    public PlayerHealth playerHealth;
    public MovementHandler movementHandler;

    private int lastAttackPhase = 0;
    private float jumpValue = 0;

    void OnEnable()
    {
        playerHealth.onHurt += HurtAnimation;
    }

    void OnDisable()
    {
        playerHealth.onHurt -= HurtAnimation;
    }

    private void HurtAnimation()
    {
        animator.SetTrigger("IsHurt");
    }

    void Update()
    {
        bool isMoving = inputHandler.movementValue.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        jumpValue = movementHandler._verticalVelocity;
        animator.SetFloat("Jump", jumpValue);
        animator.SetFloat("Fall", jumpValue);

        bool isGrounded = movementHandler.groundCheck.isGrounded;
        animator.SetBool("isGrounded", isGrounded);
        Debug.Log(isGrounded);
        

        int currentPhase = attackManager.attackPhase;
        if (currentPhase != lastAttackPhase)
        {
            attackAnimation(currentPhase);
            lastAttackPhase = currentPhase;
        }
    }

    public void attackAnimation(int phase)
    {
        switch (phase)
        {
            case 1:
                animator.SetTrigger("Punch");
                break;
            case 2:
                animator.SetTrigger("Kick");
                break;
            case 3:
                animator.SetTrigger("Magic");
                break;
            default:
                break;
        }
    }
}
