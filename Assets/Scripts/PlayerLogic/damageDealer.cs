using UnityEngine;

public class damageDealer : MonoBehaviour
{
    [SerializeField] private Transformation transformation;
    private float dmgMultiplier = 2;
    public float damage;
    [SerializeField] private float baseDmg;


    void Start()
    {
        if (transformation == null)
        {
            transformation=FindFirstObjectByType<Transformation>();
        }
    }
    void Update()
    {
        if(transformation.isTransformed == true)
        {
            damage = baseDmg * dmgMultiplier;
        }
        else
        {
            damage = baseDmg;
        }
    }

    
}
