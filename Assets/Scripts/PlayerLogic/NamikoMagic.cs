using UnityEngine;

public class NamikoMagic : MonoBehaviour
{
    public float movementSpeed;
    private Vector3 bulletDirection = Vector3.right; // Default direction

    void Update()
    {
        // Move in the stored direction, not transform.right
        transform.position += bulletDirection * movementSpeed * Time.deltaTime;
    }

    /// <summary>Sets the bullet direction as a vector. Call this from PoolManager after spawning.</summary>
    public void SetDirection(Vector3 direction)
    {
        bulletDirection = direction.normalized;
    }

    void OnBecameInvisible()
    {
        resetObj();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<enemyHealth>(out var enemyHealth))
        {
            resetObj();
        }
    }

    void resetObj()
    {
        gameObject.SetActive(false);
    }
}

