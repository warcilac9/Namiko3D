using UnityEngine;
using System;

public class enemyHealth : MonoBehaviour, iDamageable
{
    private int health;
    [SerializeField] private Animator animator;
    public int MaxHealth = 30;
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
    public void receiveDamage(int amount)
    {
        if (health > 0)
        {
            health = health - amount;
            checkLife();
            animator.SetTrigger("IsTrigger");

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
