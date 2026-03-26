using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public InputHandlerBtn inputHandlerBtn;

    [Header("Select Next Scene")]
    [SerializeField] private sceneName sceneToLoad;

    void OnEnable()
    {
        inputHandlerBtn.backToMenu += returnToMenu;
    }
    void OnDisable()
    {
        inputHandlerBtn.backToMenu -= returnToMenu;
    }

    private void returnToMenu()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}
