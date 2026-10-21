using UnityEngine;
using UnityEngine.UI;

public class InputHandlerBtn : MonoBehaviour
{
    [Header("Buttons")]
    public Button newGameBtn;
    public Button bckToMenu;
    public Button options;


    public delegate void ButtonClick();
    public ButtonClick sendNewGame;
    public ButtonClick backToMenu;

    public delegate void ButtonClickValue(int value);
    public ButtonClickValue openMenu;

    void Start()
    {
        newGameBtn.onClick.AddListener(() => sendNewGame?.Invoke());
        options.onClick.AddListener(() => openMenu?.Invoke(1));

        if (bckToMenu != null)
        {
            bckToMenu.onClick.AddListener(() => backToMenu?.Invoke());
        }
        
    }

}
