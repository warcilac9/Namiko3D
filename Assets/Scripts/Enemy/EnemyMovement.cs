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
        navAgent.SetDestination(playerTarget.position);
    }
}
