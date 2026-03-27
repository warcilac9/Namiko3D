using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Pursue : State
{
    private bool isChangingState = false;
    private float changeTimer = 0f;
    private STATE nextStateType;

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
            return; // Don't execute the rest while changing
        }

        if (Vector3.Distance(npc.transform.position, player.position) > 3)
        {
            Debug.Log("Following player");

            List<GameObject> checkpoints = EnemyDestSingleton.Singleton.Checkpoints;

            GameObject closestCheckpoint = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (checkpoints[i] == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(npc.transform.position, checkpoints[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCheckpoint = checkpoints[i];
                }
            }

            if (closestCheckpoint == null) return;

            agent.SetDestination(closestCheckpoint.transform.position);
        }
        else
        {
            if (Random.Range(0, 100) < 20)
            {
                Debug.Log("Player in range, attacking after cooldown");
                nextStateType = STATE.ATTACK;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
            else
            {
                Debug.Log("Player in range, waiting after cooldown");
                nextStateType = STATE.IDLE;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
            }
        }

        if (Random.Range(0, 100) < 1)
        {
            Debug.Log("Waiting a min after cooldown");
            nextStateType = STATE.IDLE;
            isChangingState = true;
            changeTimer = Random.Range(minCooldown, maxCooldown);
        }
    }
    public override void Exit()
    {
        anim.SetBool("isWalking", false);
        base.Exit();
    }


}
