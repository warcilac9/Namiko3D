using UnityEngine;
using UnityEngine.AI;

public class RangedMove : State
{
    private readonly float retreatDistance;
    private readonly float preferredMinDistance;
    private readonly float preferredMaxDistance;

    private bool isChangingState;
    private float changeTimer;
    private STATE nextStateType;
    private bool isRetreating;
    private readonly float retreatExitBuffer = 0.8f;

    public RangedMove(
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
        name = STATE.PURSUE;
        retreatDistance = _retreatDistance;
        preferredMinDistance = _preferredMinDistance;
        preferredMaxDistance = _preferredMaxDistance;
    }

    public override void Enter()
    {
        base.Enter();
        isChangingState = false;
        changeTimer = 0f;
        isRetreating = false;

        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    public override void Update()
    {
        RangedAI rangedAi = npc.GetComponent<RangedAI>();
        bool canAttack = rangedAi == null || rangedAi.CanStartAttack();
        if (player == null || agent == null)
        {
            return;
        }

        anim.SetBool("isWalking", true);

        if (isChangingState)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0f)
            {
                if (nextStateType == STATE.ATTACK)
                {
                    nextState = new RangedAttack(
                        npc,
                        agent,
                        anim,
                        player,
                        minCooldown,
                        maxCooldown,
                        attackDuration,
                        retreatDistance,
                        preferredMinDistance,
                        preferredMaxDistance,
                        npc.GetComponentInChildren<RangedProjectilePool>()
                    );
                }
                else
                {
                    nextState = new RangedMove(npc,
                        agent,
                        anim,
                        player,
                        minCooldown,
                        maxCooldown,
                        attackDuration,
                        retreatDistance,
                        preferredMinDistance,
                        preferredMaxDistance);
                }

                stage = EVENT.EXIT;
            }

            return;
        }

        float planarDistance = GetPlanarDistance(npc.transform.position, player.position);
        agent.isStopped = false;

        if (!isRetreating && planarDistance < retreatDistance)
        {
            isRetreating = true;
        }
        else if (isRetreating && planarDistance > retreatDistance + retreatExitBuffer)
        {
            isRetreating = false;
        }

        if (isRetreating)
        {
            Vector3 awayDir = (npc.transform.position - player.position);
            awayDir.y = 0f;

            if (awayDir.sqrMagnitude < 0.001f)
            {
                awayDir = -npc.transform.forward;
            }

            awayDir.Normalize();

            float retreatStep = (retreatDistance + retreatExitBuffer - planarDistance) + 1.5f;
            Vector3 desiredTarget = npc.transform.position + awayDir * retreatStep;

            MoveToNearestNavMeshPoint(desiredTarget);
            return;
        }

        if (planarDistance > preferredMaxDistance)
        {
            MoveToNearestNavMeshPoint(player.position);
            return;
        }

        if (planarDistance >= retreatDistance && planarDistance < preferredMinDistance)
        {
            Vector3 fromPlayer = npc.transform.position - player.position;
            fromPlayer.y = 0f;

            if (fromPlayer.sqrMagnitude < 0.001f)
            {
                fromPlayer = -npc.transform.forward;
            }

            fromPlayer.Normalize();
            Vector3 ringTarget = player.position + fromPlayer * preferredMinDistance;
            ringTarget.y = npc.transform.position.y;

            MoveToNearestNavMeshPoint(ringTarget);
            return;
        }

        if (planarDistance >= preferredMinDistance && planarDistance <= preferredMaxDistance)
        {
            if (canAttack)
            {
                nextStateType = STATE.ATTACK;
                isChangingState = true;
                changeTimer = Random.Range(minCooldown, maxCooldown);
                return;
            }

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
            return;
        }
    }

    public override void Exit()
    {
        anim.SetBool("isWalking", false);
        if (agent != null)
        {
            agent.isStopped = false;
        }
        base.Exit();
    }

    private float GetPlanarDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }

    private void MoveToNearestNavMeshPoint(Vector3 desiredTarget)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredTarget, out hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(desiredTarget);
        }
    }
}