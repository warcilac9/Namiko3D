using UnityEngine;

public class limitTrigger : MonoBehaviour
{
    public Collider hordeTrigger;
    public EventSOTriggers eventSOTriggers;
    public GameObject trigger;
    public GameObject limit1;
    public GameObject limit2;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            eventSOTriggers.Occurred(trigger, limit1, limit2);
        }
    }
}
