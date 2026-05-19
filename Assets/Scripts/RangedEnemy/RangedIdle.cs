using UnityEngine;
using UnityEngine.AI;

public class RangedIdle : State
{
    private readonly float retreatDistance;
    private readonly float preferredMinDistance;
    private readonly float preferredMaxDistance;

    private float idleTimer;
    private bool hasTriggeredIdle;

    public RangedIdle(
        GameObject _npc,
        NavMeshAgent _agent,
        Animator _anim,
        Transform _player,
        float _minCooldown,
        float _maxCooldown,
        float _attackDuration,
        float _retreatDistance,
        float _preferredMinDistance,
        float _preferredMaxDistance
    ) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.IDLE;
        retreatDistance = _retreatDistance;
        preferredMinDistance = _preferredMinDistance;
        preferredMaxDistance = _preferredMaxDistance;
    }

    public override void Enter()
    {
        base.Enter();

        idleTimer = Random.Range(minCooldown, maxCooldown);
        hasTriggeredIdle = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }
    }

    public override void Update()
    {
        if (!hasTriggeredIdle && anim != null)
        {
            anim.SetTrigger("Idle");
            hasTriggeredIdle = true;
        }

        if (player == null)
        {
            return;
        }

        idleTimer -= Time.deltaTime;
        if (idleTimer > 0f)
        {
            return;
        }

        nextState = new RangedMove(
            npc,
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

        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        if (anim != null)
        {
            anim.ResetTrigger("Idle");
        }

        if (agent != null)
        {
            agent.isStopped = false;
        }

        base.Exit();
    }
}
