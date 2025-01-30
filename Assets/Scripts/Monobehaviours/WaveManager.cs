using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EventHandler OnAllWaveCompleted;

    [SerializeField] private List<Transform> _enemySpawnerPositiontList;
    [SerializeField] private List<EnemySpawnerSO> _enemyWaveList;
    [SerializeField] private WaveTimerUI _waveTimerUI;
    [SerializeField] private EnemySpawnerSO _endlessEnemyWaveSO;

    private List<Entity> _enemySpawnerEntityList;
    private List<EnemyWaveInfo> _currentEnemyWaveInfoList;
    private List<SpawnerTimerInfo> _spawnerTimerList;
    private int _waveCounter;
    private int _completedWaveCounter;
    private float _timer;
    private bool _isDelay;
    private bool _isStopTimer;

    public int GetWaveCounter() => _completedWaveCounter;
    public int GetMaxWave() => _enemyWaveList.Count;

    private void Update()
    {
        if (_waveCounter == 0)
        {
            SetSpawners();
            return;
        }
        WaveTimerCounterHandler();
        SpawnersTimersCounterHandler();
    }

    private void SetSpawners()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (!entityManager.CreateEntityQuery(typeof(EntitiesReferences)).TryGetSingleton<EntitiesReferences>(out EntitiesReferences entitiesReferences))
            return;
            
        _waveCounter = 1;
        _waveTimerUI.UpdateWaveCounter(_waveCounter, _enemyWaveList.Count);
        
        _timer = _enemyWaveList[0].Duration;

        InitializeSpawnersTimers();

        _enemySpawnerEntityList = new List<Entity>();
        
        for (int i = 0; i < _enemySpawnerPositiontList.Count; i++)
        {
            Entity enemySpawnerEntity = entityManager.Instantiate(entitiesReferences.enemySpawnerPrefabEntity);

            LocalTransform enemySpawnerLocalTransform = entityManager.GetComponentData<LocalTransform>(enemySpawnerEntity);
            enemySpawnerLocalTransform.Position = _enemySpawnerPositiontList[i].position;
            entityManager.SetComponentData<LocalTransform>(enemySpawnerEntity, enemySpawnerLocalTransform);

            EnemySpawner enemySpawner = entityManager.GetComponentData<EnemySpawner>(enemySpawnerEntity);
            float spawnerRadius = 5f;

            if (_currentEnemyWaveInfoList[i] != null && _currentEnemyWaveInfoList[i].EnemySpawnerInfoList.Count > _spawnerTimerList[i].Index)
            {
                float spawnRate = _currentEnemyWaveInfoList[i].EnemySpawnerInfoList[_spawnerTimerList[i].Index].spawnRate;
                if (spawnRate <= 0)
                    spawnRate = float.MaxValue;
                enemySpawner.timerMax = spawnRate;
                enemySpawner.timer = spawnRate;
            }
            else
            {
                float spawnRate = float.MaxValue;
                enemySpawner.timerMax = spawnRate;
                enemySpawner.timer = spawnRate;
            }
            enemySpawner.radius = spawnerRadius;
            enemySpawner.random = new Unity.Mathematics.Random((uint)enemySpawnerEntity.Index);
            entityManager.SetComponentData<EnemySpawner>(enemySpawnerEntity, enemySpawner);

            _enemySpawnerEntityList.Add(enemySpawnerEntity);
        }
    }

    private void InitializeSpawnersTimers()
    {
        _spawnerTimerList = new List<SpawnerTimerInfo>();
        _currentEnemyWaveInfoList = _enemyWaveList[_waveCounter - 1].EnemySpawnerList.ToList<EnemyWaveInfo>();
        for (int i = 0; i < _currentEnemyWaveInfoList.Count ; i++)
        {
            if (_currentEnemyWaveInfoList[i] != null && _currentEnemyWaveInfoList[i].EnemySpawnerInfoList.Count > 0)
            {
                _spawnerTimerList.Add(new SpawnerTimerInfo{Timer = _currentEnemyWaveInfoList[i].EnemySpawnerInfoList[0].duration, Index = 0});
            }
            else
            {
                _spawnerTimerList.Add(new SpawnerTimerInfo{Timer = 0, Index = 0, IsStoped = true});
            }
        }
    }

    private void WaveTimerCounterHandler()
    {
        if (_isStopTimer)
            return;

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = 0;

            if (!_isDelay && _enemyWaveList.Count > _waveCounter && _enemyWaveList[_waveCounter - 1].Delay != 0)
            {
                _timer = _enemyWaveList[_waveCounter - 1].Delay;
                _isDelay = true;
                _waveTimerUI.UpdateNextWaveTimer(_timer, _waveCounter + 1);
            }
            else
            {
                if (_enemyWaveList.Count > _waveCounter)
                {
                    _isDelay = false;

                    _waveCounter++;
                    _completedWaveCounter++;
                    _timer = _enemyWaveList[_waveCounter - 1].Duration;

                    InitializeSpawnersTimers();

                    for (int i = 0; i < _enemySpawnerEntityList.Count; i++)
                    {
                        UpdateSpawnerComponentData(i);
                    }

                    _waveTimerUI.UpdateWaveCounter(_waveCounter, _enemyWaveList.Count);
                }
                else
                {
                    _completedWaveCounter++;
                    _isDelay = false;
                    _isStopTimer = true;
                    OnAllWaveCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        _waveTimerUI.UpdateTimer(_timer);
    }

    private void SpawnersTimersCounterHandler()
    {
        for (int i = 0; i < _spawnerTimerList.Count; i++)
        {
            if (_spawnerTimerList[i].IsStoped)
                continue;

            if (_spawnerTimerList[i].Timer > 0)
            {
                _spawnerTimerList[i].Timer -= Time.deltaTime;
            }
            else
            {
                if (_currentEnemyWaveInfoList[i].EnemySpawnerInfoList.Count > _spawnerTimerList[i].Index + 1)
                {
                    _spawnerTimerList[i].Index++;
                    _spawnerTimerList[i].Timer = _currentEnemyWaveInfoList[i].EnemySpawnerInfoList[_spawnerTimerList[i].Index].duration;
                    UpdateSpawnerComponentData(i);
                }
                else
                {
                    _spawnerTimerList[i].IsStoped = true;

                    EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                    EnemySpawner enemySpawner = entityManager.GetComponentData<EnemySpawner>(_enemySpawnerEntityList[i]);
                    float spawnRate = float.MaxValue;
                    enemySpawner.timerMax = spawnRate;
                    enemySpawner.timer = spawnRate;
                    entityManager.SetComponentData<EnemySpawner>(_enemySpawnerEntityList[i], enemySpawner);
                }
            }
        }
    }

    private void UpdateSpawnerComponentData(int index)
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EnemySpawner enemySpawner = entityManager.GetComponentData<EnemySpawner>(_enemySpawnerEntityList[index]);
        
        if (_currentEnemyWaveInfoList[index] != null && _currentEnemyWaveInfoList[index].EnemySpawnerInfoList.Count > _spawnerTimerList[index].Index)
        {
            
            float spawnRate = _currentEnemyWaveInfoList[index].EnemySpawnerInfoList[_spawnerTimerList[index].Index].spawnRate;
            if (spawnRate <= 0)
                spawnRate = float.MaxValue;
            enemySpawner.timerMax = spawnRate;
            enemySpawner.timer = spawnRate;
        }
        else
        {
            float spawnRate = float.MaxValue;
            enemySpawner.timerMax = spawnRate;
            enemySpawner.timer = spawnRate;
        }

        entityManager.SetComponentData<EnemySpawner>(_enemySpawnerEntityList[index], enemySpawner);
    }

    public void StartEndlessWave()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        List<EnemyWaveInfo> enemySpawnerList = _endlessEnemyWaveSO.EnemySpawnerList.ToList<EnemyWaveInfo>();
        
        for (int i = 0; i < _enemySpawnerEntityList.Count; i++)
        {
            EnemySpawner enemySpawner = entityManager.GetComponentData<EnemySpawner>(_enemySpawnerEntityList[i]);
            
            float spawnRate = enemySpawnerList[i].EnemySpawnerInfoList[0].spawnRate;
            enemySpawner.timerMax = spawnRate;
            enemySpawner.timer = spawnRate;

            entityManager.SetComponentData<EnemySpawner>(_enemySpawnerEntityList[i], enemySpawner);
        }
    }
}

public class SpawnerTimerInfo
{
    public int Index;
    public float Timer;
    public bool IsStoped;
}
