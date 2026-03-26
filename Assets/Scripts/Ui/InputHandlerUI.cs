using UnityEngine;
using UnityEngine.UI;

public class InputHandlerBtn : MonoBehaviour
{
    [Header("Buttons")]
    public Button newGameBtn;
    public Button bckToMenu;

    public delegate void ButtonClick();
    public ButtonClick sendNewGame;
    public ButtonClick backToMenu;


    void Start()
    {
        newGameBtn.onClick.AddListener(() => sendNewGame?.Invoke());

        if (bckToMenu != null)
        {
            bckToMenu.onClick.AddListener(() => backToMenu?.Invoke());
        }
    }

}
