using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _retryGameButton;
    [SerializeField] private Button _quiteGameButton;

    private void Start()
    {
        Hide();

        GameManager.Instance.OnPauseGame += OnPauseGame;
        GameManager.Instance.OnUnpauseGame += OnUnpauseGame;

        
        _resumeGameButton.onClick.AddListener(() => { GameManager.Instance.TogglePauseGame(); });
        _retryGameButton.onClick.AddListener(() => { SceneManager.LoadSceneAsync(1, LoadSceneMode.Single); });
        _quiteGameButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void OnPauseGame(object sender, EventArgs e)
    {
        Show();
    }

    private void OnUnpauseGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

