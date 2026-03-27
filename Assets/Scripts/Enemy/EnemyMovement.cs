using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    private NavMeshAgent navAgent;
    private float recalculatePathTimer = 0.5f;
    private float timeSinceLastRecalculation = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        // Only recalculate path periodically (every 0.5 seconds) instead of every frame
        timeSinceLastRecalculation -= Time.fixedDeltaTime;
        if (timeSinceLastRecalculation <= 0f)
        {
            moveEnemy();
            timeSinceLastRecalculation = recalculatePathTimer;
        }
    }

    public void moveEnemy()
    {
        List<GameObject> checkpoints = EnemyDestSingleton.Singleton.Checkpoints;
        if (checkpoints == null || checkpoints.Count == 0)
            return;

        GameObject closestCheckpoint = null;
        float closestDistance = float.MaxValue;

        // Only check checkpoints, not every frame
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] == null)
                continue;

            float distance = Vector3.Distance(transform.position, checkpoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoints[i];
            }
        }

        if (closestCheckpoint != null)
        {
            navAgent.SetDestination(closestCheckpoint.transform.position);
        }
    }
}
