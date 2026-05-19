using UnityEngine;
using UnityEngine.AI;

public class RangedAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private enemyHealth health;

    public Animator anim;
    public Transform player;

    private State currentState;

    [Header("Timing")]
    public float minCooldown = 0.1f;
    public float maxCooldown = 0.5f;
    public float attackDuration = 0.4f;

    [Header("Ranged Spacing")]
    public float retreatDistance = 3.5f;
    public float preferredMinDistance = 5f;
    public float preferredMaxDistance = 8f;

    [Header("Attack Rhythm")]
    public float minAttackInterval = 1.1f;
    public float maxAttackInterval = 2.2f;

    private float nextAttackAllowedTime;

    private EnemyDamagePayload pendingDamage;
    private bool hasPendingDamage;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<enemyHealth>();
    }

    void OnEnable()
    {
        if(health != null)
        {
            health.OnDamageReceived -= HandleDamageReceived;
            health.OnDamageReceived += HandleDamageReceived;
        }

        hasPendingDamage = false;
        pendingDamage = null;

        if(agent != null)
        {
            agent.ResetPath();
            agent.isStopped = false;
        }

        nextAttackAllowedTime = 0f;
        currentState = null;
        TryResolvePlayer();
        EnsureStartingState();
    }

    void OnDisable()
    {
        if (health != null)
        {
            health.OnDamageReceived -= HandleDamageReceived;
        }
    }

    void Start()
    {
        TryResolvePlayer();
        EnsureStartingState();
    }

    void Update()
    {
        if(currentState == null)
        {
            EnsureStartingState();
        }
        if(currentState != null)
        {
            currentState = currentState.Process();
        }

        TryConsumePendingDamage();
    }

    private void HandleDamageReceived(EnemyDamagePayload payload)
    {
        pendingDamage = payload;
        hasPendingDamage = true;
    }

    private void TryConsumePendingDamage()
    {
        if(!hasPendingDamage || currentState == null)
        {
            return;
        }

        if (!CanConsumePendingDamage())
        {
            return;
        }

        currentState = new RangedHurt(
            gameObject,
            agent,
            anim,
            player,
            minCooldown,
            maxCooldown,
            attackDuration,
            pendingDamage.HurtDuration,
            pendingDamage.IsLethal,
            retreatDistance,
            preferredMinDistance,
            preferredMaxDistance
        );

        hasPendingDamage = false;
        pendingDamage = null;

    }

    private bool CanConsumePendingDamage()
    {
        if(currentState == null)
        {
            return true;
        }

        if (currentState.name == State.STATE.ATTACK)
        {
            return false;
        }

        if (currentState.name == State.STATE.HURT)
        {
            return false;
        }

        return currentState.name == State.STATE.IDLE || currentState.name == State.STATE.PURSUE;
    }

    public void HandleHurtFinished(bool wasLethal)
    {
        if (!wasLethal)
        {
            return;
        }

        if(health != null)
        {
            health.FinalizeDeath();
        }
    }

    private void EnsureStartingState()
    {
        if(player == null)
        {
            TryResolvePlayer();
        }
        if(player != null)
        {
            currentState = new RangedIdle(
                gameObject,
                agent,
                anim,
                player,
                minCooldown,
                maxCooldown,
                attackDuration,
                retreatDistance,
                preferredMinDistance,
                preferredMaxDistance
            );
        } 
    }

    private void TryResolvePlayer()
    {
        if(player != null)
        {
            return;
        }

        GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (taggedPlayer != null)
        {
        player = taggedPlayer.transform;
        return;
        }
    }

    public bool CanStartAttack()
    {
        return Time.time >= nextAttackAllowedTime;
    }

    public void ConsumeAttackWindow()
    {
        float interval = Random.Range(minAttackInterval,maxAttackInterval);
        nextAttackAllowedTime = Time.time + interval;
    }
}
