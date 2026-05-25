using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FollowPlayer : MonoBehaviour
{
    public float speed;

    public Transform target;
    public GameObject buny;
    
    
    void FixedUpdate()
    {
        if(target != null)
        {
            isFollowing();
        }
    }
   public void isFollowing()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        
    }
}
