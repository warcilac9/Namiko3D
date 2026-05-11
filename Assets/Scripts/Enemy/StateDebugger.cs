using UnityEngine;
using TMPro;

public class StateDebugger : MonoBehaviour
{
    private AI aiScript;
    public TMP_Text text;

    void Start()
    {
        aiScript = GetComponent<AI>();
    }

    void Update()
    {
        text.text = aiScript.cState;
    }
}
