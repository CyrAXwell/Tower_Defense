using UnityEngine;

public class FreezeAreaSkillVisualController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _circle;
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
    
    public void Initialize(float size, float duration)
    {
        _timer = duration;

        _circle.Stop();
        _snowflakes.Stop();

        var circleMain = _circle.main;
        circleMain.startSize = size;
        circleMain.duration = duration;
        circleMain.startLifetime = duration;

        var snowflakesShape = _snowflakes.shape;
        snowflakesShape.radius = size / 2;

        var snowflakesMain = _snowflakes.main;
        snowflakesMain.duration = duration;

        var snowflakesEmission = _snowflakes.emission;
        var burst = snowflakesEmission.GetBurst(0);
        burst.cycleCount = 1000;
        snowflakesEmission.SetBurst(0, burst);

        _circle.Play();
        _snowflakes.Play();

    }
}
