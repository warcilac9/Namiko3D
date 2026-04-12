using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public bool isFacingRight = true;
    public GameObject playerGameObject;
    public InputHandler inputHandler;
    public AttackManager attackManager;
    public PlayerHealth playerHealth;

    public int counter = 0;
    private int lastAttackPhase = 0;

    void OnEnable()
    {
        inputHandler.moving += moveAnimation;
        playerHealth.onHurt += HurtAnimation;
    }

    void OnDisable()
    {
        inputHandler.moving -= moveAnimation;
        playerHealth.onHurt -= HurtAnimation;
    }

    private void moveAnimation()
    {
        if(inputHandler.moveValX > 0 || inputHandler.moveValY>0 ||inputHandler.moveValX < 0 || inputHandler.moveValY<0)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void HurtAnimation()
    {
        animator.SetTrigger("IsHurt");
    }

    void Update()
    {
        if (inputHandler.moveValX == 0 && inputHandler.moveValY == 0)
        {
            animator.SetBool("isWalking", false);
        }
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
