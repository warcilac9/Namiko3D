using UnityEngine;

public class LineDistribution : MonoBehaviour
{
    public DialogueLines dialogueLines;
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        dialogueManager.dialogue = dialogueLines;
    }
    private void OnTriggerExit(Collider other)
    {
        dialogueManager.dialogue = null;
    }


}
