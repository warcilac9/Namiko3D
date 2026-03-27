using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    private bool isChangingState = false;
    private float changeTimer = 0f;
    private float attackTimer = 0f;
    private STATE nextStateType;
    private bool hasAttacked = false;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, float _minCooldown, float _maxCooldown, float _attackDuration) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();
        isChangingState = false;
        changeTimer = 0f;
        attackTimer = attackDuration;
        hasAttacked = false;
    }

    public override void Update()
    {
        if (!hasAttacked)
        {
            anim.SetTrigger("Attack");
            hasAttacked = true;
            return;
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        if (!isChangingState)
        {
            if (Vector3.Distance(npc.transform.position, player.position) > 3)
            {
                nextStateType = STATE.PURSUE;
            }
            else
            {
                nextStateType = STATE.IDLE;
            }

            isChangingState = true;
            changeTimer = Random.Range(minCooldown, maxCooldown);
            return;
        }

        if (isChangingState)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0f)
            {
                if (nextStateType == STATE.PURSUE)
                {
                    nextState = new Pursue(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                else if (nextStateType == STATE.IDLE)
                {
                    nextState = new Idle(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("Attack");
        base.Exit();
    }
}
