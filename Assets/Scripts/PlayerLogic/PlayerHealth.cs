using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
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

    public delegate void Shake(CinemachineImpulseSource impulseSource);
    public Shake onCameraShake;

    public event Action onDeath;

    public CinemachineImpulseSource impulseSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<damagePlayer>(out var damagePlayer))
        {
            receiveDamage(damagePlayer.damage);
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    public void receiveDamage(float amount)
    {
        if (health > 0 && canRecieveDmg)
        {
            Debug.Log("Received " + amount + " damage");
            health = health - amount;
            healthBar.fillAmount = health / 100f;
            StartCoroutine(iFrame());
            StartCoroutine(CanMove());
            onHurt?.Invoke();
        }
    }

    public void healPlayer(float amount)
    {
        health = Mathf.Min(health + amount, 100f);
        healthBar.fillAmount = health / 100f;
    }

    void Death()
    {
        onDeath?.Invoke();
    }

    IEnumerator iFrame()
    {
        onCameraShake?.Invoke(impulseSource);
        canRecieveDmg = false;
        yield return new WaitForSeconds(iFrameSec);
        canRecieveDmg = true;
    }

    IEnumerator CanMove()
    {
        inputHandler.movementValue = Vector2.zero;
        inputHandler.gameObject.SetActive(false);
        yield return new WaitForSeconds(stun);
        inputHandler.gameObject.SetActive(true);
    }
}
