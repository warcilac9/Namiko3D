using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject canvas;
    private bool isPaused = false;


    void OnEnable()
    {
        inputHandler.pauseGame += GamePaused;
        inputHandler.unPauseGame += GamePaused;
    }
    void OnDisable()
    {
        inputHandler.pauseGame -= GamePaused;
        inputHandler.unPauseGame -= GamePaused;
    }
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
