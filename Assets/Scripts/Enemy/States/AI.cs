using System;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private enemyHealth health;

    public Animator anim;
    public Transform player;

    private State currentState;

    public float minCooldown;
    public float maxCooldown;
    public float attackDuration = 0.4f;

    private EnemyDamagePayload pendingDamage;
    private bool hasPendingDamage;
    private bool hasPendingLethalDamage;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<enemyHealth>();
    }

    void OnEnable()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if(health == null)
        {
            health = GetComponent<enemyHealth>();
        }

        if(health != null)
        {
            health.OnDamageReceived -= HandleDamageReceived;
            health.OnDamageReceived += HandleDamageReceived;
        }

        hasPendingDamage = false;
        hasPendingLethalDamage = false;
        pendingDamage = null;

        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = false;
        }

        currentState = null;
        TryResolvePlayer();
        EnsureStartingState();
    }

    void OnDisable()
    {
        if(health != null)
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
        if (currentState == null)
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
        hasPendingLethalDamage = payload.IsLethal;
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
        currentState = new Hurt(gameObject,
            agent,
            anim,
            player,
            minCooldown,
            maxCooldown,
            attackDuration,
            pendingDamage.HurtDuration,
            pendingDamage.IsLethal);

            
            hasPendingDamage = false;
            pendingDamage = null;
    }

    private bool CanConsumePendingDamage()
    {
        if(currentState == null)
        {
            return true;
        }
        if(currentState.name == State.STATE.ATTACK)
        {
            return false;
        }

        if(currentState.name == State.STATE.HURT)
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

        if (health != null)
        {
            health.FinalizeDeath();
        }

        hasPendingLethalDamage = false;
    }
    private void EnsureStartingState()
    {
        if (player == null)
        {
            TryResolvePlayer();
        }

        if (player != null)
        {
            currentState = new Idle(gameObject, agent, anim, player, minCooldown, maxCooldown, attackDuration);
        }
        else
        {
            Debug.LogWarning("AI: Player Transform not found. Assign it in the inspector or tag player as 'Player'.", this);
        }
    }

    private void TryResolvePlayer()
    {
        if (player != null)
        {
            return;
        }

        GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (taggedPlayer != null)
        {
            player = taggedPlayer.transform;
            return;
        }

        GameObject namedPlayer = GameObject.Find("Player") ?? GameObject.Find("Namiko");
        if (namedPlayer != null)
        {
            player = namedPlayer.transform;
        }
    }
}
