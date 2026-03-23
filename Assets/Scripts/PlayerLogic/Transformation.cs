using UnityEngine;
using System.Collections;
public class Transformation : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject namikoSprite;
    public bool isTransforming;
    public float transformCooldown;
    public RuntimeAnimatorController magicalGirlNamiko;
    [SerializeField] Animator animator;
    [SerializeField] Animator animatorNamiko;
    void OnEnable()
    {
        inputHandler.transforming += NamikoTransform;
    }
    void OnDisable()
    {
        inputHandler.transforming -= NamikoTransform;
    }
    public void NamikoTransform()
    {
        animatorNamiko = namikoSprite.GetComponent<Animator>();
        isTransforming = false;
        if (!isTransforming)
        {
            StartCoroutine(ChangeAnimator());
        }
        isTransforming = false;
        
    }
    IEnumerator ChangeAnimator()
    {
        isTransforming = true;
        animator.SetTrigger("Transformation");
        yield return new WaitForSeconds(1f);
        animatorNamiko.runtimeAnimatorController = magicalGirlNamiko;
        
    }
    
   
}