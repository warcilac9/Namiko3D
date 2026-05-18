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


    [SerializeField] private int index;

    private void OnEnable()
    {
        inputHandler.interacting += GetDialogue;
        inputHandler.dialogue += NextLine;
    }
    private void OnDisable()
    {
        inputHandler.interacting -= GetDialogue;
        inputHandler.dialogue -= NextLine;
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
        if (dialoguePool != null)
        {
            dialogue = dialoguePool.Peek();
            StartDialogue(dialogue);
        }
    }
    private void StartDialogue(DialogueLines SOlines)
    {
        
        canvas.GetComponent<Canvas>().enabled = true;
        inputHandler.input.SwitchCurrentActionMap("UI");
        index = SOlines.index;
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
           
        }
    }
    public void EndLine()
    {
        //make sure lines end
        canvas.GetComponent<Canvas>().enabled = false;
    }
}
