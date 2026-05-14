using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    public Canvas canvas;
    public Image[] characterImg;
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;

    [SerializeField] private int index;


    void Start()
    {
        text.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        
    }
    private void StartDialogue()
    {
        index = 2;
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
    public void NextLine()
    {

    }
}
