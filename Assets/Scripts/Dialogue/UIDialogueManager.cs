using UnityEngine;

public class UIDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject ButtonUi;

    void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            ButtonUi.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        ButtonUi.SetActive(false);
    }
}
