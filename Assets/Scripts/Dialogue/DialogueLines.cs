using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueLines", menuName = "Scriptable Objects/DialogueLines", order = 55)]
public class DialogueLines : ScriptableObject
{
    public Image[] characterImages;
    public string[] characterLines;
    public int index;
    public int repeatFrom;
}
