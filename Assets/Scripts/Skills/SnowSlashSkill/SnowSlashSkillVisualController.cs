using UnityEngine;

public class SnowSlashSkillVisualController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _slash;
    [SerializeField] private ParticleSystem _snowflakes;

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float size, float duration, float frequency)
    {
        _timer = duration;

        _slash.Stop();
        _snowflakes.Stop();

        var slashMain = _slash.main;
        slashMain.duration = frequency + 0.01f;
        // slashMain.startLifetime = frequency / 3;
        slashMain.startSize = size;

        var snowflakesMain = _snowflakes.main;
        snowflakesMain.duration = frequency;

        var snowflakesMainShape = _snowflakes.shape;
        snowflakesMainShape.radius = size / 2;

        _slash.Play();
        _snowflakes.Play();
    }
}
