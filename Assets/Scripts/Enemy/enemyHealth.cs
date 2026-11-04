using System;
using UnityEngine;

public class enemyHealth : MonoBehaviour, iDamageable
{
    [Header("Health")]
    public float MaxHealth = 30;

    //public Collider hitBox;

    [Header("Health Drop")]
    [SerializeField, Range(0f, 1f)]
    private float healthDropChance = 0.25f;

    [Header("Damage Reaction")]
    [SerializeField]
    private float defaultHurtDuration = 0.4f;

    [SerializeField]
    private float deathAnimationDuration = 1f;

    [SerializeField]
    private float duplicateHitGraceSeconds = 0.08f;

    [SerializeField]
    private ParticleSystem pSHurt;

    private float health;
    public spawnObjEvent spawnObjEvent;
    private bool pendingDeathFinalization;
    private bool canReceiveDamage;
    private damageDealer lastDamageDealer;
    private float lastDamageTime;

    public event Action<enemyHealth> OnEnemyDefeated;
    public event Action<EnemyDamagePayload> OnDamageReceived;

    public float DeathAnimationDuration => deathAnimationDuration;

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
        if (other.TryGetComponent<damageDealer>(out var dealer))
        {
            if (dealer == lastDamageDealer && Time.time - lastDamageTime < duplicateHitGraceSeconds)
            {
                return;
            }

            lastDamageDealer = dealer;
            lastDamageTime = Time.time;
            receiveDamage(dealer.damage);
        }
    }

    public void receiveDamage(float dmgAmount)
    {
        if (!canReceiveDamage || health <= 0f)
        {
            return;
        }

        health -= dmgAmount;
        pSHurt.Play();
        bool isLethal = health <= 0f;
        Debug.Log($"[{gameObject.name}] health remaining: {health}/{MaxHealth}", this);

        EnemyDamagePayload payload = new EnemyDamagePayload(
            amount: dmgAmount,
            hurtDuration: defaultHurtDuration,
            isLethal: isLethal
        );

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
        TrySpawnHealthObject();
        OnEnemyDefeated?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void TrySpawnHealthObject()
    {
        float chance = Mathf.Clamp01(healthDropChance);
        if (chance <= 0f || (chance < 1f && UnityEngine.Random.value >= chance))
        {
            return;
        }

        spawnObjEvent.Occurred(transform);
    }

    public void SetCanReceiveDamage(bool value)
    {
        canReceiveDamage = value;
    }

    private void ResetHealthState()
    {
        health = MaxHealth;
        pendingDeathFinalization = false;
        canReceiveDamage = true;
        lastDamageDealer = null;
        lastDamageTime = -999f;
    }
}
