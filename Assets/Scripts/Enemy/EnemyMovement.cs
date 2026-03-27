using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    private NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        moveEnemy();
    }

    public void moveEnemy()
    {
        List<GameObject> checkpoints = EnemyDestSingleton.Singleton.Checkpoints;

        GameObject closestCheckpoint = null;
        float closestDistance = float.MaxValue;

        for (int i = 1; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, checkpoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoints[i];
            }
        }

        if (checkpoints.Count > 0 && checkpoints[0] != null)
        {
            float distance = Vector3.Distance(transform.position, checkpoints[0].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoints[0];
            }
        }

        if (closestCheckpoint == null) return;

        navAgent.SetDestination(closestCheckpoint.transform.position);
    }
}
