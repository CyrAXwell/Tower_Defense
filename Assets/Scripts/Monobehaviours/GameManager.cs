using System;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EventHandler OnGameLose;
    public EventHandler OnGameWin;
    public EventHandler OnPauseGame;
    public EventHandler OnUnpauseGame;

    public static GameManager Instance;

    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private EnemyAmountCounterUI _enemyAmountCounterUI;
    [SerializeField] private SubScene _subScene;

    private bool _isFinaleWave;
    private bool _isManualPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PasueGame();

        PlayerDeadTestSystem playerDeadTestSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerDeadTestSystem>();
        playerDeadTestSystem.OnPlayerDeath += OnPlayerDeath;
        _waveManager.OnAllWaveCompleted += OnAllWaveCompleted;
    }

    public int GetMaxWave() => _waveManager.GetMaxWave();
    public int GetWaveCounter() => _waveManager.GetWaveCounter();
    public int GetSpawnedEnemiesAmount() => _enemyAmountCounterUI.GetSpawnedEnemiesAmount();
    public int GetDestroyedEnemiesAmount() => _enemyAmountCounterUI.GetDestroyedEnemiesAmount();

    public void PasueGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpasueGame()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (_isFinaleWave)
        {
            if(_enemyAmountCounterUI.GetSpawnedEnemiesAmount() == _enemyAmountCounterUI.GetDestroyedEnemiesAmount())
            {
                PasueGame();  
                OnGameWin?.Invoke(this, EventArgs.Empty);
                _isFinaleWave = false;
            }
        }
    }

    public void TogglePauseGame()
    {
        if (_isManualPaused)
        {
            UnpasueGame();
            OnUnpauseGame?.Invoke(this, EventArgs.Empty);
            _isManualPaused = false;
        }
        else if (Time.timeScale != 0)
        {
            PasueGame();
            OnPauseGame?.Invoke(this, EventArgs.Empty);
            _isManualPaused = true;
        }
    }

    public void OnPressEndlessWaveButton()
    {
        _waveManager.StartEndlessWave();
        UnpasueGame();
    }   
    
    private void OnPlayerDeath(object sender, EventArgs e)
    {
        PasueGame();  
        
        OnGameLose?.Invoke(this, EventArgs.Empty);
    }

    private void OnAllWaveCompleted(object sender, EventArgs e)
    {
        _isFinaleWave = true;
    }
}
