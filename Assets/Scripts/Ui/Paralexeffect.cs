using UnityEngine;
using UnityEngine.UI;

public class Paralexeffect : MonoBehaviour
{
    private Material material;
    private float distance;

    [Range(0f,0.5f)]
    public float speed;
    
    
    void Start()
    {
        
        material = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime * speed;
        material.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}
