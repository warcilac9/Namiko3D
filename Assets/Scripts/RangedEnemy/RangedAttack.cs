using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : State
{
    private readonly float retreatDistance;
    private readonly float preferredMinDistance;
    private readonly float preferredMaxDistance;

    private readonly RangedProjectilePool projectilePool;
    private bool hasFired;
    private float attackTimer;
    private float shotDelayTimer;

    public RangedAttack(
        GameObject _npc,
        NavMeshAgent _agent,
        Animator _anim,
        Transform _player,
        float _minCooldown,
        float _maxCooldown,
        float _attackDuration,
        float _retreatDistance,
        float _preferredMinDistance,
        float _preferredMaxDistance,
        RangedProjectilePool _projectilePool
    ) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.ATTACK;
        retreatDistance = _retreatDistance;
        preferredMinDistance = _preferredMinDistance;
        preferredMaxDistance = _preferredMaxDistance;
        projectilePool = _projectilePool;
    }

    public override void Enter()
    {
        base.Enter();

        if(agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if(anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("Attack");
        }

        hasFired = false;
        attackTimer = attackDuration;

        RangedAI rangedAI = npc.GetComponent<RangedAI>();
        shotDelayTimer = rangedAI != null ? rangedAI.shotDelay : 0f;
    }

    public override void Update()
    {
        if (!hasFired)
        {
            shotDelayTimer -= Time.deltaTime;
            if (shotDelayTimer > 0f) return;

            if(projectilePool != null && player != null)
            {
                RangedAI rangedAI = npc.GetComponent<RangedAI>();
                if (rangedAI != null)
                {
                    rangedAI.ConsumeAttackWindow();
                }

                Vector3 targetPosition = player.position + Vector3.up * 1f;
                projectilePool.RequestProjectile(targetPosition);
            }

            hasFired = true;
            return;
        }

        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0f)
        {
            nextState = new RangedIdle(
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
    }

    public override void Exit()
    {
        if (anim != null)
        {
            anim.ResetTrigger("Attack");
        }

        if (agent != null)
        {
            agent.isStopped = false;
        }
        base.Exit();
    }
}