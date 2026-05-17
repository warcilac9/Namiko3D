using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public DialogueLines dialogue;
    public Canvas canvas;
    public Image[] characterImg;
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;


    [SerializeField] private int index;


    void Start()
    {
        
        text.text = string.Empty;
        StartDialogue(dialogue);
    }

    void Update()
    {
       
            //NextLine(dialogue);
     
    }
    private void StartDialogue(DialogueLines SOlines)
    {
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
    public void NextLine(DialogueLines SOlines)
    {
        index = SOlines.index;
        lines = SOlines.characterLines;
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
    }
}
