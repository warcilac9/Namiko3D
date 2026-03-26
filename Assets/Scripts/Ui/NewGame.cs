using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public InputHandlerBtn inputHandlerBtn;

    [Header("Select Next Scene")]
    [SerializeField] private sceneName sceneToLoad;

    void OnEnable()
    {
        inputHandlerBtn.sendNewGame += startNewGame;
    }
    void OnDisable()
    {
        inputHandlerBtn.sendNewGame -= startNewGame;
    }

    private void startNewGame()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}