using UnityEngine;
using UnityEngine.AI;

public class RangedHurt : State
{
    private readonly float hurtDuration;
    private readonly float deathAnimationDuration;
    private readonly bool endsInDeathFinalization;

    private readonly float retreatDistance;
    private readonly float preferredMinDistance;
    private readonly float preferredMaxDistance;

    private bool hasTriggeredHurtAnimation;
    private bool hasTriggeredDeathAnimation;
    private bool hasHandledCompletion;
    private float hurtTimer;
    private float deathTimer;

    private RangedAI rangedAi;
    private enemyHealth health;

    public RangedHurt(
        GameObject _npc,
        NavMeshAgent _agent,
        Animator _anim,
        Transform _player,
        float _minCooldown,
        float _maxCooldown,
        float _attackDuration,
        float _hurtDuration,
        bool _endsInDeathFinalization,
        float _retreatDistance,
        float _preferredMinDistance,
        float _preferredMaxDistance
    ) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.HURT;
        hurtDuration = _hurtDuration;
        endsInDeathFinalization = _endsInDeathFinalization;

        retreatDistance = _retreatDistance;
        preferredMinDistance = _preferredMinDistance;
        preferredMaxDistance = _preferredMaxDistance;

        rangedAi = _npc.GetComponent<RangedAI>();
        health = _npc.GetComponent<enemyHealth>();
        deathAnimationDuration = health != null ? health.DeathAnimationDuration : 1f;
    }

    public override void Enter()
    {
        base.Enter();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }

        health?.SetCanReceiveDamage(false);

        hasTriggeredHurtAnimation = false;
        hasTriggeredDeathAnimation = false;
        hasHandledCompletion = false;
        hurtTimer = hurtDuration;
        deathTimer = 0f;
    }

    public override void Update()
    {
        if (!hasTriggeredHurtAnimation)
        {
            if (anim != null)
            {
                anim.SetTrigger("isHurt");
            }

            hasTriggeredHurtAnimation = true;
            return;
        }

        if (hurtTimer > 0f)
        {
            hurtTimer -= Time.deltaTime;
            return;
        }

        if (endsInDeathFinalization)
        {
            if (!hasTriggeredDeathAnimation)
            {
                if (anim != null)
                {
                    anim.SetTrigger("isDead");
                }

                hasTriggeredDeathAnimation = true;
                deathTimer = deathAnimationDuration;
                return;
            }

            if (deathTimer > 0f)
            {
                deathTimer -= Time.deltaTime;
                return;
            }

            if (hasHandledCompletion)
            {
                return;
            }

            hasHandledCompletion = true;
            rangedAi?.HandleHurtFinished(true);

            if (!npc.activeInHierarchy)
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
            return;
        }

        if (hasHandledCompletion)
        {
            return;
        }

        hasHandledCompletion = true;

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
        health?.SetCanReceiveDamage(true);

        if (anim != null)
        {
            anim.ResetTrigger("isHurt");
            anim.ResetTrigger("isDead");
        }

        base.Exit();
    }
}