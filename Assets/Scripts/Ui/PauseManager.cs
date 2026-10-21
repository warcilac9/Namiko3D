using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject canvas;
   public bool isPaused = false;

    public void GamePaused()
    {
        Debug.Log("game paused");
        if(!isPaused)
        {
            isPaused = true;
            canvas.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0f;
            inputHandler.input.SwitchCurrentActionMap("UI");
            
        }
        else if(isPaused)
        {
            isPaused = false;
            canvas.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1f;
            inputHandler.input.SwitchCurrentActionMap("Player");
        }
            

    }

}
