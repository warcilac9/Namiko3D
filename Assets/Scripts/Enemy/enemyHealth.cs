using UnityEngine;

public class enemyHealth : MonoBehaviour, iDamageable
{
    int health;
    public int MaxHealth = 30;
    public Collider hitBox;

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
        health=MaxHealth;
        gameObject.SetActive(false);
    }


}
