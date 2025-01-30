using UnityEngine;

public class HealingAreaSkillVisualController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _circle;
    [SerializeField] private ParticleSystem _healing;

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
        _healing.Stop();

        var circleMain = _circle.main;
        circleMain.duration = duration;
        circleMain.startLifetime = duration;
        circleMain.startSize = size;

        var healingMain = _healing.main;
        healingMain.duration = duration;

        var healingShape = _healing.shape;
        healingShape.radius = size / 2;

        _circle.Play();
        _healing.Play();
    }
}
