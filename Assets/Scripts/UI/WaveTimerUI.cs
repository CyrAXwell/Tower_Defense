using TMPro;
using UnityEngine;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _waveCounter;
    [SerializeField] private TMP_Text _nextWaveTimer;

    private void Start()
    {
        _nextWaveTimer.text = "";
    }

    public void UpdateWaveCounter(int waveCounter, int maxWave)
    {
        _waveCounter.text = "WAVE " + waveCounter.ToString() + "/" + maxWave.ToString();
        _nextWaveTimer.text = "";
    }

    public void UpdateTimer(float time)
    {        
        _timer.text = GetTimeFormat(time);
    }

    public void UpdateNextWaveTimer(float time, int waveCounter)
    {
        _nextWaveTimer.text = "Wave " + waveCounter + " starts in " + GetTimeFormat(time);
    }

    private string GetTimeFormat(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
