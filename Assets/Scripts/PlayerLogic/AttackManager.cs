using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public int attackPhase = 0;
    public float attackCooldown;
    public float comboTimeout = 1.5f;
    public float afterComboCD = 0.5f;
    public int attackCount = 0;
    [SerializeField] GameObject punchHitbox;
    [SerializeField] GameObject kickHitbox;
    [SerializeField] float punchActiveTime = 0.06f;
    [SerializeField] float kickActiveTime = 0.06f;
    float comboTimer = 0f;
    bool canAttack = true;
    bool inComboCooldown = false;
    Coroutine punchHitboxCoroutine;
    Coroutine kickHitboxCoroutine;
    [SerializeField] PoolManager poolManager;
    [SerializeField] Transform originTransform;

    public InputHandler inputHandler;

    void Awake()
    {
        if (punchHitbox == null)
        {
            punchHitbox = FindChildByName(transform, "punchHitbox");
        }

        if (kickHitbox == null)
        {
            kickHitbox = FindChildByName(transform, "kickHItbox");
        }
    }

    void OnEnable()
    {
        if (inputHandler == null)
        {
            inputHandler = FindFirstObjectByType<InputHandler>();
        }
        
        if (inputHandler != null)
        {
            inputHandler.typeAttack += addPunch;
        }
    }
    void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.typeAttack -= addPunch;
        }

        if (punchHitboxCoroutine != null)
        {
            StopCoroutine(punchHitboxCoroutine);
            punchHitboxCoroutine = null;
        }

        if (kickHitboxCoroutine != null)
        {
            StopCoroutine(kickHitboxCoroutine);
            kickHitboxCoroutine = null;
        }

        if (punchHitbox != null)
        {
            punchHitbox.SetActive(false);
        }

        if (kickHitbox != null)
        {
            kickHitbox.SetActive(false);
        }
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
                TriggerPunchHitbox();
                break;
            case 1:
                attackPhase = 2;
                TriggerKickHitbox();
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

    void TriggerPunchHitbox()
    {
        if (punchHitbox == null)
        {
            return;
        }

        if (punchHitboxCoroutine != null)
        {
            StopCoroutine(punchHitboxCoroutine);
        }

        punchHitboxCoroutine = StartCoroutine(EnableHitboxTemporarily(punchHitbox, punchActiveTime, true));
    }

    void TriggerKickHitbox()
    {
        if (kickHitbox == null)
        {
            return;
        }

        if (kickHitboxCoroutine != null)
        {
            StopCoroutine(kickHitboxCoroutine);
        }

        kickHitboxCoroutine = StartCoroutine(EnableHitboxTemporarily(kickHitbox, kickActiveTime, false));
    }

    IEnumerator EnableHitboxTemporarily(GameObject hitbox, float activeTime, bool isPunch)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        hitbox.SetActive(false);

        if (isPunch)
        {
            punchHitboxCoroutine = null;
        }
        else
        {
            kickHitboxCoroutine = null;
        }
    }

    static GameObject FindChildByName(Transform root, string childName)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
