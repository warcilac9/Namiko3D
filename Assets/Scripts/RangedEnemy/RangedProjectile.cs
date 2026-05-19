using System;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{
    [SerializeField] private float arcHeight = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float duration;
    private float elapsedTime;
    private bool isFlying;

    public void Launch(Vector3 start, Vector3 target, float flightDuration, float height)
    {
        startPosition = start;
        targetPosition = target;
        duration = Mathf.Max(0.01f, flightDuration);
        arcHeight = height;
        elapsedTime = 0f;
        isFlying = true;

        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isFlying)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        Vector3 position =  Vector3.Lerp(startPosition, targetPosition, t);
        position.y += Mathf.Sin(t*Mathf.PI) * arcHeight;
        transform.position = position;

        if( t >= 1f)
        {
            DisableProjectile();
        }
    }

    private void DisableProjectile()
    {
        isFlying = false;
        gameObject.SetActive(false);
    }
}
