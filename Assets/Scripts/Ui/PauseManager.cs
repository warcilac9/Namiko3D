using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject canvas;
    public bool isPaused = false;

    private CursorLockMode previousCursorLockState;
    private bool previousCursorVisibility;

    public void GamePaused()
    {
        Debug.Log("game paused");
        if (!isPaused)
        {
            isPaused = true;
            previousCursorLockState = Cursor.lockState;
            previousCursorVisibility = Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //canvas.SetActive(true);
            Time.timeScale = 0f;
            inputHandler.input.SwitchCurrentActionMap("UI");
        }
        else if (isPaused)
        {
            isPaused = false;
            //canvas.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = previousCursorLockState;
            Cursor.visible = previousCursorVisibility;
            inputHandler.input.SwitchCurrentActionMap("Player");
        }
    }
}
