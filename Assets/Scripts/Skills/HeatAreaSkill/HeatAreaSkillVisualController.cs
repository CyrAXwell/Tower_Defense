using UnityEngine;

public class HeatAreaSkillVisualController : MonoBehaviour
{
    private const float SIZE_MULTIPLIER = 0.25f;

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

        float localScale = size * SIZE_MULTIPLIER;
        transform.localScale = new Vector3(localScale, 1, localScale);
    }
}
