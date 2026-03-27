using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] Button playAgainButton;
    [SerializeField] EventSystem eventSystem;

    void OnEnable()
    {
        playerHealth.onDeath += Death;
    }

    void OnDisable()
    {
        playerHealth.onDeath -= Death;
    }

    void Death()
    {
        inputHandler.moveValX = 0;
        inputHandler.moveValY = 0;
        inputHandler.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        eventSystem.firstSelectedGameObject = playAgainButton.gameObject;
    }
}
