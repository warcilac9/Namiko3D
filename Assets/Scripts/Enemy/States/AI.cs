using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    public Animator anim;
    public Transform player;
    State currentState;
    public float minCooldown;
    public float maxCooldown;
    public float attackDuration = 0.4f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TryResolvePlayer();

        if (player != null)
        {
            currentState = new Idle(gameObject, agent, anim, player, minCooldown, maxCooldown, attackDuration);
        }
        else
        {
            Debug.LogWarning("AI: Player Transform not found. Assign it in the inspector or tag player as 'Player'.", this);
        }
    }

    public void TriggerHurt()
    {
        currentState = new Hurt(gameObject, agent, anim, player, minCooldown, maxCooldown, attackDuration);
    }

    void Update()
    {
        if (currentState == null)
        {
            currentState = new Idle(gameObject, agent, anim, player, minCooldown, maxCooldown, attackDuration);
        }

        currentState = currentState.Process();
    }

    private void TryResolvePlayer()
    {
        if (player != null) return;

        GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (taggedPlayer != null)
        {
            player = taggedPlayer.transform;
            return;
        }

        GameObject namedPlayer = GameObject.Find("Player") ?? GameObject.Find("Namiko");
        if (namedPlayer != null)
        {
            player = namedPlayer.transform;
        }
    }
}
