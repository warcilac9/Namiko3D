using UnityEngine;
using System;

public class enemyHealth : MonoBehaviour, iDamageable
{
    [Header("Health")]
    public float MaxHealth = 30;
    public Collider hitBox;

    [Header("Damage Reaction")]
    [SerializeField] private float defaultHurtDuration = 0.4f;

    private float health;
    private bool pendingDeathFinalization;
    
    public event Action<enemyHealth> OnEnemyDefeated;
    public event Action<EnemyDamagePayload> OnDamageReceived;

    void Awake()
    {
        health = MaxHealth;
    }

    void OnEnable()
    {
        ResetHealthState();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<damageDealer>(out var dealer))
        {
            receiveDamage(dealer.damage);
        }
    }
    public void receiveDamage(float dmgAmount)
    {
        if (health <= 0f)
        {
            return;
        } 

        health -= dmgAmount;
        bool isLethal = health <= 0f;
        Debug.Log($"[{gameObject.name}] health remaining: {health}/{MaxHealth}", this);

        EnemyDamagePayload payload = new EnemyDamagePayload(amount: dmgAmount, hurtDuration: defaultHurtDuration, isLethal: isLethal);

        OnDamageReceived?.Invoke(payload);

        if (isLethal)
        {
            pendingDeathFinalization = true;
        }
    }

    public void FinalizeDeath()
    {
        if (!pendingDeathFinalization)
        {
            return;
        }

        pendingDeathFinalization = false;
        OnEnemyDefeated?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void ResetHealthState()
    {
        health = MaxHealth;
        pendingDeathFinalization = false;
    }


}
