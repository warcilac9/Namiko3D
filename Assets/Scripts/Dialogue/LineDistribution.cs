using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDistribution : MonoBehaviour
{
    public DialogueLines[] dialogueLines;
    public DialogueManager dialogueManager;
    public Collider Player;

    private void OnTriggerStay(Collider other)
    {

        if(other == Player )
        { 
        foreach (DialogueLines dialogue in dialogueLines)
        {
                dialogueManager.canInteract = true;
                dialogueManager.dialoguePool.Enqueue(dialogue);
        }
        }
        else
        {
            dialogueManager.canInteract = false;
            dialogueManager.dialogue = null;
        }
        //dialogueManager.dialogue = dialogueLines;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == Player)
        {
            dialogueManager.canInteract = false;
            //dialogueManager.dialogue = null;
        }
    }


}
