using UnityEngine;
using UnityEngine.AI;

public class Hurt : State
{
    private bool isChangingState = false;
    private bool hasReceivedDamage = false;
    private float changeTimer = 0f;
    private float stunDuration;


    public Hurt(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, float _minCooldown, float _maxCooldown, float _attackDuration) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.HURT;
    }

    public override void Enter()
    {
        agent.isStopped = true;
        base.Enter();
        Debug.Log(name);
        isChangingState = false;
        hasReceivedDamage = false;
        changeTimer = 0f;
        stunDuration = attackDuration;
    }

    public override void Update()
    {
        if (!hasReceivedDamage)
        {
            anim.SetTrigger("IsHurt");
            hasReceivedDamage = true;
            return;
        }

        if(stunDuration > 0f)
        {
            stunDuration-=Time.deltaTime;
            return;
        }

        if (!isChangingState)
        {
            isChangingState = true;
            changeTimer = Random.Range(minCooldown, maxCooldown);
            return;
        }

        if (isChangingState)
        {
            changeTimer -= Time.deltaTime;
            if(changeTimer <= 0f)
            {
                nextState = new Idle(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                stage = EVENT.EXIT;
            }
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("IsHurt");
        agent.isStopped = false;
        base.Exit();
    }
}
