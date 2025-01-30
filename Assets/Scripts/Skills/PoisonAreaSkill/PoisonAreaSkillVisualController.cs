using UnityEngine;

public class PoisonAreaSkillVisualController : MonoBehaviour
{
    private const float WORLD_START_SIZE = 8.7f;

    [SerializeField] private ParticleSystem _circle;
    [SerializeField] private ParticleSystem _skulls;
    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private ParticleSystem _points;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float size, float duration)
    {
        timer = duration;

        _circle.Stop();
        _skulls.Stop();
        _sparks.Stop();
        _points.Stop();

        var circleMain = _circle.main;
        float textureSize = circleMain.startSize.constantMax;
        float sizeMultiplyer =  textureSize / WORLD_START_SIZE;
        circleMain.startSize = size * sizeMultiplyer;
        circleMain.duration = duration;
        circleMain.startLifetime = duration;

        float radius = size / 2;
        var skullsMain = _skulls.main;
        skullsMain.duration = duration;
        var skullsShape = _skulls.shape;
        skullsShape.radius = radius;

        var sparksMain = _sparks.main;
        sparksMain.duration = duration;
        var sparksShape = _sparks.shape;
        sparksShape.radius = radius;
        
        var pointsMain = _points.main;
        pointsMain.duration = duration;
        var pointsShape = _points.shape;
        pointsShape.radius = radius;

        _circle.Play();
        _skulls.Play();
        _sparks.Play();
        _points.Play();
 
    }
}
