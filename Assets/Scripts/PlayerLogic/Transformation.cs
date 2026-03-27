using UnityEngine;
using System.Collections;
public class Transformation : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject namikoSprite;
    public GameObject HeartParticles;
    public bool isTransforming;
    public bool isTransformed;
    [Min(0f)] public float transformDuration = 6f;
    [Min(0f)] public float transformCooldown = 8f;
    [Min(0f)] public float initialTransformDelay = 3f;
    [Min(0f)] public float detransformAnimationDelay = 1f;
    public RuntimeAnimatorController magicalGirlNamiko;

    [SerializeField] Animator animator;
    [SerializeField] Animator animatorNamiko;

    bool canTransform;
    RuntimeAnimatorController defaultNamikoController;



    void Awake()
    {
        if (animatorNamiko == null && namikoSprite != null)
        {
            animatorNamiko = namikoSprite.GetComponent<Animator>();
        }

        if (animatorNamiko != null)
        {
            defaultNamikoController = animatorNamiko.runtimeAnimatorController;
        }
    }

    void Start()
    {
        canTransform = false;
        StartCoroutine(InitialTransformLockout());
    }

    void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.transforming += NamikoTransform;
        }
    }

    void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.transforming -= NamikoTransform;
        }
    }

    public void NamikoTransform()
    {
        if (!canTransform || isTransforming)
            return;

        StartCoroutine(TransformationFlow());
    }

    IEnumerator InitialTransformLockout()
    {
        if (initialTransformDelay > 0f)
        {
            yield return new WaitForSeconds(initialTransformDelay);
        }

        canTransform = true;
    }

    IEnumerator TransformationFlow()
    {
        canTransform = false;
        isTransforming = true;

        if (animatorNamiko == null && namikoSprite != null)
        {
            animatorNamiko = namikoSprite.GetComponent<Animator>();
        }

        animator.SetTrigger("Transformation");
        yield return new WaitForSeconds(1f);

        if (animatorNamiko != null && magicalGirlNamiko != null)
        {
            animatorNamiko.runtimeAnimatorController = magicalGirlNamiko;
            
        }

        isTransformed = true;

        if (transformDuration > 0f)
        {
            HeartParticles.gameObject.SetActive(true);
            yield return new WaitForSeconds(transformDuration);
            
        }

        isTransformed = false;
        animator.SetTrigger("Transform");

        if (detransformAnimationDelay > 0f)
        {
            yield return new WaitForSeconds(detransformAnimationDelay);
        }

        if (animatorNamiko != null && defaultNamikoController != null)
        {
            animatorNamiko.runtimeAnimatorController = defaultNamikoController;
            HeartParticles.gameObject.SetActive(false);
        }

        isTransforming = false;

        if (transformCooldown > 0f)
        {
            yield return new WaitForSeconds(transformCooldown);
        }

        canTransform = true;
    }

}