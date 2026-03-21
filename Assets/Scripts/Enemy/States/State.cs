using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
            IDLE, PURSUE, ATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    protected float minCooldown;
    protected float maxCooldown;
    protected float attackDuration;
    
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, float _minCooldown, float _maxCooldown, float _attackDuration)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
        minCooldown = _minCooldown;
        maxCooldown = _maxCooldown;
        attackDuration = _attackDuration;
    }

    public virtual void Enter() {stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() {stage = EVENT.EXIT; }

    public State Process()
    {
        if(stage == EVENT.ENTER) Enter();
        if(stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}
