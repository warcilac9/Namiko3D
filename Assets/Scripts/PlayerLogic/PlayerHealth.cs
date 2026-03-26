using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, iDamageable
{
    public int health = 100;
    public Collider hitBox;

    public event Action onDeath;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<damagePlayer>(out var damagePlayer))
        {
            receiveDamage(damagePlayer.damage);
        }
    }

    void Update()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    public void receiveDamage(int amount)
    {
        if(health > 0)
        {
            Debug.Log("Received "+amount+" damage");
            health = health - amount;
        }
    }

    void Death()
    {
        onDeath?.Invoke();
    }
}
