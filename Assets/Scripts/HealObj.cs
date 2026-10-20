using UnityEngine;

public class HealObj : MonoBehaviour
{
    public float amountToHeal;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.healPlayer(amountToHeal);
            this.gameObject.SetActive(false);
        }
    }
}
