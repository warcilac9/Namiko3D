using UnityEngine;

public class LineDistribution : MonoBehaviour
{
    public DialogueLines dialogueLines;
    public DialogueManager dialogueManager;

    private void OnCollisionStay(Collision collision)
    {
        dialogueManager.dialogue = dialogueLines;
    }
}
