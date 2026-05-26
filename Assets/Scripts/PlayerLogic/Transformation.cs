using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Transformation : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject namikoSprite;
    public GameObject HeartParticles;
    public bool isTransforming;
    public bool isTransformed;
    private bool isTransformedBuffer;
    [Min(0f)] public float transformDuration = 6f;
    [Min(0f)] public float transformDurationTimer;
    [Min(0f)] public float transformCooldown = 8f;
    [Min(0f)] public float transformCooldownTimer = 8f;
    [Min(0f)] public float initialTransformDelay = 3f;
    [Min(0f)] public float detransformAnimationDelay = 1f;

    public RuntimeAnimatorController magicalGirlNamiko;
    public Image MagicBar;

    [SerializeField] Animator animator;
    [SerializeField] Animator animatorNamiko;

    bool canTransform;
    float currentCooldownDuration;
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
        transformDurationTimer = transformDuration;
        BeginCooldown(initialTransformDelay > 0f ? initialTransformDelay : transformCooldown);
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
    private void Update()
    {
        if (isTransformedBuffer)
        {
            if (transformDuration > 0f)
            {
                transformDurationTimer = Mathf.Max(0f, transformDurationTimer - Time.deltaTime);
                MagicBar.fillAmount = transformDurationTimer / transformDuration;
            }
            else
            {
                MagicBar.fillAmount = 0f;
            }
        }

        if (!isTransformedBuffer)
        {
            if (!canTransform)
            {
                transformCooldownTimer += Time.deltaTime;

                if (currentCooldownDuration > 0f)
                {
                    transformCooldownTimer = Mathf.Min(transformCooldownTimer, currentCooldownDuration);
                    MagicBar.fillAmount = transformCooldownTimer / currentCooldownDuration;
                }
                else
                {
                    transformCooldownTimer = 0f;
                    MagicBar.fillAmount = 1f;
                }

                if (transformCooldownTimer >= currentCooldownDuration)
                {
                    canTransform = true;
                    transformDurationTimer = transformDuration;
                }
            }
            else
            {
                MagicBar.fillAmount = 1f;
            }
        }
            
    }

    public void NamikoTransform()
    {
        if (!canTransform || isTransforming)
            return;

        StartCoroutine(TransformationFlow());
    }

    void BeginCooldown(float cooldownDuration)
    {
        currentCooldownDuration = Mathf.Max(0f, cooldownDuration);
        transformCooldownTimer = 0f;
        canTransform = currentCooldownDuration <= 0f;

        if (canTransform)
        {
            transformDurationTimer = transformDuration;
        }
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
        isTransformedBuffer = true;
        transformDurationTimer = transformDuration;


        if (transformDuration > 0f)
        {
            
            HeartParticles.gameObject.SetActive(true);
            yield return new WaitForSeconds(transformDuration);
            
        }

        
        animator.SetTrigger("Transform");
        isTransformed = false;

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
        isTransformedBuffer = false;
        BeginCooldown(transformCooldown);
    }

}