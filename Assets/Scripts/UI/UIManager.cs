using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI _mainMenuUI;
    [SerializeField] private PauseMenuUI _pauseMenuUI;
    [SerializeField] private GameOverScreenUI _gameOverScreenUI;

    private void Start()
    {
        _mainMenuUI.Initialize();
        _pauseMenuUI.Initialize();
    }
}
