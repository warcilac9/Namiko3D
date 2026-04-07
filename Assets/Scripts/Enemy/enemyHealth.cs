using UnityEngine;
using System;

public class enemyHealth : MonoBehaviour, iDamageable
{
    private float health;
    public float MaxHealth = 30;
    public Collider hitBox;
    public event Action<enemyHealth> OnEnemyDefeated;
    

    void Awake()
    {
        health = MaxHealth;
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
