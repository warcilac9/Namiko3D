using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public PauseManager pauseManager;
    public InputHandlerBtn inputHandlerBtn;

    public Stack<Canvas> menus = new Stack<Canvas>();
    public Canvas[] canvasInScene;

    void OnEnable()
    {
        inputHandler.pauseGame += ActivateMenu;
        inputHandler.unPauseGame += ActivateMenu;
        
    }
    void OnDisable()
    {
        inputHandler.pauseGame -= ActivateMenu;
        inputHandler.unPauseGame -= ActivateMenu;
    }

    void Start()
    {
        menus.Push(canvasInScene[0]);
    }
    public void Update()
    {
        
    }
    public void ChangeMenu(int canvasToEnable)
    {
        for (int indexPanels = 0; indexPanels < canvasInScene.Length; indexPanels++)
        {
            menus.Push(canvasInScene[indexPanels]);
            canvasInScene[indexPanels].enabled = indexPanels == canvasToEnable;
        }
    }

    public void ActivateMenu()
    {
        Debug.Log(menus.Count + " menu count");
        if(pauseManager.isPaused == false)
        {
            pauseManager.GamePaused();
        }
        else{ 
            if (menus.Count > 1)
            {
                menus.Pop().enabled = false;
                menus.Peek().enabled = true;
            }
            if(menus.Count == 1 && pauseManager.isPaused)
            {
                pauseManager.GamePaused();
            }
            }
        
    }
}
