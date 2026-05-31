using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDistribution : MonoBehaviour
{
    public List<DialogueLines> dialogueLines;
    public DialogueManager dialogueManager;
    public Collider Player;
    public int maxLineIndex;
    public DialogueLines currentDialogue;
    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other == Player )
        { 
          
        foreach (DialogueLines dialogue in dialogueLines)
        {
                dialogueManager.dialoguePool.Enqueue(dialogue);
                dialogueManager.canInteract = true;
                currentDialogue = dialogueManager.dialoguePool.Peek();
                
        }
        }
        else
        {
            dialogueManager.canInteract = false;
            dialogueManager.dialogue = null;
            dialogueManager.dialoguePool.Clear();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == Player)
        {
            if(currentDialogue.CanswitchLines)
            {
                Debug.Log("Si es true");
                dialogueLines.Remove(dialogueLines[maxLineIndex]);
                dialogueManager.canInteract = false;
                dialogueManager.dialoguePool.Clear();
            }
            else
            {
                dialogueManager.canInteract = false;
                dialogueManager.dialoguePool.Clear();
            }
            //dialogueManager.dialogue = null;
        }
    }
    private void Update()
    {
        
    }

}
