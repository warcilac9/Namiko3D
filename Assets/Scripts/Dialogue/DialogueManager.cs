using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public Queue<DialogueLines> dialoguePool = new Queue<DialogueLines>();
    public DialogueLines dialogue;
    public GameObject canvas;
    public Image[] characterImg;
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;

    public InputHandler inputHandler;
    public bool canInteract;


    [SerializeField] private int index;

    private void OnEnable()
    {
        inputHandler.interacting += GetDialogue;
        inputHandler.dialogue += FastLine;
    }
    private void OnDisable()
    {
        inputHandler.interacting -= GetDialogue;
        inputHandler.dialogue -= FastLine;
    }


    void Start()
    {
        
        text.text = string.Empty;

    }

    void Update()
    {
        
        Debug.Log(dialoguePool.Count);
        if (dialogue == null)
        {
            Debug.Log("No Dialogue Lines Inserted!");
            return;
        }
     
        //NextLine(dialogue);

    }
    private void GetDialogue()
    {
        Debug.Log("Interacting..");
        if (canInteract)
        {
            if (dialoguePool != null)
            {
                dialogue = dialoguePool.Peek();
                StartDialogue(dialogue);
            }
        }
    }
    private void StartDialogue(DialogueLines SOlines)
    {
        text.text = string.Empty;
        canvas.GetComponent<Canvas>().enabled = true;
        inputHandler.input.SwitchCurrentActionMap("UI");
        if(dialogue.firstInteraction)
        {
            index = SOlines.index;
        } 
        else
        {
            index = dialogue.repeatFrom;
        }
        lines = SOlines.characterLines;
        StartCoroutine(TypeLines());

    }
     IEnumerator TypeLines()
    {
        foreach(char character in lines[index].ToCharArray() )
        {
            text.text += character;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    public void NextLine(/*DialogueLines SOlines*/)
    {
        //index = SOlines.index;
        //lines = SOlines.characterLines;
        if(index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLines());
        }
        else
        {

            EndLine();
        }
    }
    public void FastLine()
    {
        if(text.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            text.text = lines[index];
        }

    }
    public void EndLine()
    {
        dialogue.firstInteraction = false;
        inputHandler.input.SwitchCurrentActionMap("Player");
        dialogue = null;
        canvas.GetComponent<Canvas>().enabled = false;
    }

}
