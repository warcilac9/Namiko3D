using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    private bool isChangingState = false;
    private float changeTimer = 0f;
    private STATE nextStateType;

    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, float _minCooldown, float _maxCooldown, float _attackDuration) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
        isChangingState = false;
        changeTimer = 0f;
    }

    public override void Update()
    {
        anim.SetTrigger("Idle");
        if (isChangingState)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0f)
            {
                if (nextStateType == STATE.PURSUE)
                {
                    nextState = new Pursue(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                else if (nextStateType == STATE.ATTACK)
                {
                    nextState = new Attack(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                stage = EVENT.EXIT;
            }
        }
        else
        {
            if (Random.Range(0, 100) < 1 && Vector3.Distance(npc.transform.position, player.position) > 2)
            {
                Debug.Log("Starting to follow player after cooldown");
                nextStateType = STATE.PURSUE;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
            else if (Random.Range(0, 100) < 1 && Vector3.Distance(npc.transform.position, player.position) < 2)
            {
                Debug.Log("Starting to attack player after cooldown");
                nextStateType = STATE.ATTACK;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("Idle");
        base.Exit();
    }
}
