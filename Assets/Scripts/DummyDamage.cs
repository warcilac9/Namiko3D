using UnityEngine;
using TMPro;

public class DummyDamage : MonoBehaviour
{
    public TMP_Text text;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<damageDealer>(out var damageDealer))
        {
            receiveDamage(damageDealer.damage);
        }
    }

    void receiveDamage(float damage)
    {
        text.text = damage.ToString();
    }

}
