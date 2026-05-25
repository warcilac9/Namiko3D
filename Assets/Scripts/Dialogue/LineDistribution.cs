using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDistribution : MonoBehaviour
{
    public DialogueLines[] dialogueLines;
    public DialogueManager dialogueManager;
    public Collider Player;

    private void OnTriggerEnter(Collider other)
    {

        if(other == Player )
        { 
          
        foreach (DialogueLines dialogue in dialogueLines)
        {
                dialogueManager.dialoguePool.Enqueue(dialogue);
                dialogueManager.canInteract = true;
                
        }
        }
        else
        {
            dialogueManager.canInteract = false;
            dialogueManager.dialogue = null;
            dialogueManager.dialoguePool.Clear();
        }
        //dialogueManager.dialogue = dialogueLines;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == Player)
        {
            dialogueManager.canInteract = false;
            dialogueManager.dialoguePool.Clear();
            //dialogueManager.dialogue = null;
        }
    }
    private void Update()
    {
        Debug.Log(dialogueLines[0]);
    }

}
