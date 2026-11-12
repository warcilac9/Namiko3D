using UnityEngine;

public class UIDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject ButtonUi;

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out _))
        {
            ButtonUi.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out _))
        {
            ButtonUi.SetActive(false);
        }
    }
}
