using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Pursue : State
{
    private bool isChangingState = false;
    private float changeTimer = 0f;
    private STATE nextStateType;
    private float recalculatePathTimer = 0.5f;
    private float timeSinceLastRecalculation = 0f;

    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, float _minCooldown, float _maxCooldown, float _attackDuration) : base(_npc, _agent, _anim, _player, _minCooldown, _maxCooldown, _attackDuration)
    {
        name = STATE.PURSUE;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();
        isChangingState = false;
        changeTimer = 0f;
        timeSinceLastRecalculation = 0f;
    }
    public override void Update()
    {
        anim.SetBool("isWalking", true);
        if (isChangingState)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0f)
            {
                if (nextStateType == STATE.ATTACK)
                {
                    nextState = new Attack(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                else if (nextStateType == STATE.IDLE)
                {
                    nextState = new Idle(npc, agent, anim, player, minCooldown, maxCooldown, attackDuration);
                }
                stage = EVENT.EXIT;
            }
            return;
        }

        if (Vector3.Distance(npc.transform.position, player.position) > 3)
        {
            // Only recalculate checkpoint path every 0.5 seconds instead of every frame
            timeSinceLastRecalculation -= Time.deltaTime;
            if (timeSinceLastRecalculation <= 0f)
            {
                RecalculateCheckpointPath();
                timeSinceLastRecalculation = recalculatePathTimer;
            }
        }
        else
        {
            if (Random.Range(0, 100) < 20)
            {
                nextStateType = STATE.ATTACK;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
            else
            {
                nextStateType = STATE.IDLE;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
        }

        if (Random.Range(0, 100) < 1)
        {
            nextStateType = STATE.IDLE;
            isChangingState = true;
            changeTimer = Random.Range(minCooldown, maxCooldown);
        }
    }

    private void RecalculateCheckpointPath()
    {
        List<GameObject> checkpoints = EnemyDestSingleton.Singleton.Checkpoints;
        if (checkpoints == null || checkpoints.Count == 0)
            return;

        GameObject closestCheckpoint = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] == null)
                continue;

            float distance = Vector3.Distance(npc.transform.position, checkpoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoints[i];
            }
        }

        if (closestCheckpoint != null)
        {
            agent.SetDestination(closestCheckpoint.transform.position);
        }
    }
    public override void Exit()
    {
        anim.SetBool("isWalking", false);
        base.Exit();
    }


}
