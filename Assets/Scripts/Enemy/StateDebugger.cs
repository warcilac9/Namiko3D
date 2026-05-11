using UnityEngine;
using TMPro;

public class StateDebugger : MonoBehaviour
{
    private AI aiScript;
    public TMP_Text text;
    public TMP_Text cooldown;

    void Start()
    {
        aiScript = GetComponent<AI>();
    }

    void Update()
    {
        text.text = aiScript.cState;
        cooldown.text = 
    }
}
