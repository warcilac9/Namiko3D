using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public int attackPhase = 0;
    public float attackCooldown;
    public float comboTimeout = 1.5f;
    public float afterComboCD = 0.5f;
    public int attackCount = 0;
    float comboTimer = 0f;
    bool canAttack = true;
    bool inComboCooldown = false;
    [SerializeField] PoolManager poolManager;
    [SerializeField] Transform originTransform;

    public InputHandler inputHandler;

    void OnEnable()
    {
        inputHandler.typeAttack += addPunch;
    }
    void OnDisable()
    {
        inputHandler.typeAttack -= addPunch;
    }

    void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            
            if (comboTimer <= 0 && attackCount > 0)
            {
                attackCount = 0;
            }
        }
    }

    private void addPunch(int punchID)
    {
        if (inComboCooldown)
        {
            return;
        }
        
        if (canAttack && attackCount < 3)
        {
            attackCount++;
            
            if (attackCount == 1)
            {
                StartCoroutine(ExecuteAttack(punchID));
            }
            else if (attackCount == 2)
            {
                StartCoroutine(ExecuteAttack(punchID));
            }
            else if (attackCount == 3)
            {
                StartCoroutine(ExecuteAttackAndCooldown(punchID));
            }
        }
    }

    IEnumerator ExecuteAttack(int punchID)
    {
        canAttack = false;
        comboTimer = comboTimeout;
        SetAttackPhase(punchID);
        
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        attackPhase = 0;
    }

    IEnumerator ExecuteAttackAndCooldown(int punchID)
    {
        canAttack = false;
        comboTimer = comboTimeout;
        SetAttackPhase(punchID);
        
        yield return new WaitForSeconds(attackCooldown);

        attackPhase = 0;
        
        inComboCooldown = true;
        canAttack = false;
        
        yield return new WaitForSeconds(afterComboCD);
        
        canAttack = true;
        attackCount = 0;
        inComboCooldown = false;
    }

    void SetAttackPhase(int punchID)
    {
        switch(punchID)
        {
            case 0:
                attackPhase = 1;
                break;
            case 1:
                attackPhase = 2;
                break;
            case 2:
                attackPhase = 3;
                poolManager.RequesObject(originTransform);
                break;
            default:
                attackPhase = 0;
                break;
        }
    }
}
