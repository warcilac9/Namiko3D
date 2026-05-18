using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDistribution : MonoBehaviour
{
    public DialogueLines[] dialogueLines;
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {

        foreach (DialogueLines dialogue in dialogueLines)
        {
            dialogueManager.dialoguePool.Enqueue(dialogue);
        }

        //dialogueManager.dialogue = dialogueLines;
    }
    private void OnTriggerExit(Collider other)
    {
        //dialogueManager.dialogue = null;
    }


}
