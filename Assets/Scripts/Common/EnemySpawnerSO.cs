using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerSO", menuName = "ScriptableObject/Enemies")]
public class EnemySpawnerSO : ScriptableObject
{
    [SerializeField] private int _waveindex;
    [SerializeField] private float _duration;
    [SerializeField] private float _delay;
    [SerializeField] private List<EnemyWaveInfo> _enemySpawnerList;

    public int Waveindex => _waveindex;
    public float Duration => _duration;
    public float Delay => _delay;

    public IEnumerable<EnemyWaveInfo> EnemySpawnerList => _enemySpawnerList;
}
