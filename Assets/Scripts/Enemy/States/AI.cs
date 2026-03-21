using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;
    public float minCooldown;
    public float maxCooldown;
    public float attackDuration = 0.4f;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player, minCooldown, maxCooldown, attackDuration);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
