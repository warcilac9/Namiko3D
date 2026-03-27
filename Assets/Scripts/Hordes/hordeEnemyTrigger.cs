using UnityEngine;

public class hordeEnemyTrigger : MonoBehaviour
{
    public Collider hordeTrigger;
    public EventSOEnemySpawn triggerEffect;
    public int enemiesToSpawn;
    public int maxEnemies = 3;
    public GameObject triggerObj;
    public Transform origin;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth) && triggerEffect != null)
        {
            triggerEffect.Occurred(enemiesToSpawn, maxEnemies, origin);
        }
    }


}
