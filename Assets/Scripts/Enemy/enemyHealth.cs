using UnityEngine;
using System;

public class enemyHealth : MonoBehaviour, iDamageable
{
    private float health;
    public float MaxHealth = 30;
    public Collider hitBox;
    public event Action<enemyHealth> OnEnemyDefeated;
    private AI ai;
    

    void Awake()
    {
        health = MaxHealth;
        ai = GetComponent<AI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<damageDealer>(out var damageDealer))
        {
            receiveDamage(damageDealer.damage);
        }
    }
    public void receiveDamage(float amount)
    {
        if (health > 0)
        {
            health = health - amount;
            checkLife();
            if (health > 0 && ai != null)
            {
                ai.TriggerHurt();
            }
        } 
    }
    void checkLife()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        OnEnemyDefeated?.Invoke(this);
        health=MaxHealth;
        gameObject.SetActive(false);
    }


}
