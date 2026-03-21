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
        
        if (checkpoints.Count == 0) return;

        GameObject closestCheckpoint = checkpoints[0];
        float closestDistance = Vector3.Distance(transform.position, checkpoints[0].transform.position);

        for (int i = 1; i < checkpoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, checkpoints[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoints[i];
            }
        }

        navAgent.SetDestination(closestCheckpoint.transform.position);
    }
}
