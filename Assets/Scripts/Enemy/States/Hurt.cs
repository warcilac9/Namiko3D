using UnityEngine;
using UnityEngine.AI;

public class Hurt : State
{
    private readonly float hurtDuration;
    private readonly float deathAnimationDuration;
    private readonly bool endsInDeathFinalization;

    private bool hasTriggeredHurtAnimation;
    private bool hasTriggeredDeathAnimation;
    private bool hasHandledCompletion;
    private float hurtTimer;
    private float deathTimer;

    private AI ai;
    private enemyHealth health;

    public Hurt(
        GameObject _npc,
        NavMeshAgent _agent,
        Animator _anim,
        Transform _player,
        float _minCooldown,
        float _maxCooldown,
        float _attackDuration,
        float _hurtDuration,
        bool _endsInDeathFinalization
    ) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.HURT;
        hurtDuration = _hurtDuration;
        endsInDeathFinalization = _endsInDeathFinalization;
        ai = _npc.GetComponent<AI>();
        health = _npc.GetComponent<enemyHealth>();
        deathAnimationDuration = health != null ? health.DeathAnimationDuration : 1f;
    }

    public override void Enter()
    {
        base.Enter();

        if (agent != null)
        {
            agent.isStopped = true;
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
                anim.SetTrigger("IsHurt");
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
                    anim.SetTrigger("Death");
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
            ai?.HandleHurtFinished(true);

            if (!npc.activeInHierarchy)
            {
                return;
            }

            nextState = new Idle(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
            stage = EVENT.EXIT;
            return;
        }

        if (hasHandledCompletion)
        {
            return;
        }

        hasHandledCompletion = true;

        nextState = new Idle(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        health?.SetCanReceiveDamage(true);

        if (anim != null)
        {
            anim.ResetTrigger("IsHurt");
            anim.ResetTrigger("Death");
        }

        base.Exit();
    }
}