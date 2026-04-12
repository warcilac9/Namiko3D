using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, iDamageable
{
    public InputHandler inputHandler;
    public Image healthBar;

    public float health = 100;
    public float iFrameSec;
    public float stun;
    public bool canRecieveDmg = true;
    public Collider hitBox;
    public event Action onHurt;

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

    public void receiveDamage(float amount)
    {
        if(health > 0 && canRecieveDmg)
        {
            health = health - amount;
            healthBar.fillAmount = health / 100f;
            StartCoroutine(iFrame());
            StartCoroutine(CanMove());
            onHurt?.Invoke();
            

        }
    }

    void Death()
    {
        onDeath?.Invoke();
    }
    IEnumerator iFrame()
    {
        canRecieveDmg = false;
        yield return new WaitForSeconds(iFrameSec);
        canRecieveDmg = true;
    }
    IEnumerator CanMove()
    {
        inputHandler.moveValX = 0;
        inputHandler.moveValY = 0;
        inputHandler.gameObject.SetActive(false);
        yield return new WaitForSeconds(stun);
        inputHandler.gameObject.SetActive(true);
    }
    
}
