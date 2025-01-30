using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreenUI : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _endlessGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private TMP_Text _gameResult;
    [SerializeField] private TMP_Text _titleText;

    public void Initialize()
    {
        GameManager.Instance.OnGameLose += OnGameLose;
        GameManager.Instance.OnGameWin += OnGameWin;

        _playAgainButton.onClick.AddListener(() => { OnMainmenuButton(); });  
        _quitGameButton.onClick.AddListener(() => { Application.Quit(); });  
        _endlessGameButton.onClick.AddListener(() => { GameManager.Instance.OnPressEndlessWaveButton(); Hide(); }); 
    }

    private void OnGameLose(object sender, EventArgs e)
    {
        Show(true);
    }

    private void OnGameWin(object sender, EventArgs e)
    {
        Show(false);
    }

    private void OnMainmenuButton()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void Show(bool isGameLose)
    {
        gameObject.SetActive(true);
        _endlessGameButton.gameObject.SetActive(!isGameLose);

        _titleText.text = isGameLose ? "GAME OVER" : "VICTORY";

        string waveCompletedText = "Wave completed " + GameManager.Instance.GetWaveCounter() + "/" + GameManager.Instance.GetMaxWave();
        string enemyDestroedAmountText = "Destroed enemies: " +  GameManager.Instance.GetDestroyedEnemiesAmount();
        string enemySpawnedAmountText = "Spawned enemies: " + GameManager.Instance.GetSpawnedEnemiesAmount();
        _gameResult.text = waveCompletedText + "\n" + enemyDestroedAmountText + "\n" + enemySpawnedAmountText;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameLose -= OnGameLose;
        GameManager.Instance.OnGameWin -= OnGameWin; 
    }
}
