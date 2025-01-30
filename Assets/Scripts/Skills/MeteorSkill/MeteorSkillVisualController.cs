using UnityEngine;
using UnityEngine.VFX;

public class MeteorSkillVisualController : MonoBehaviour
{
    private const string DIAMETER = "Diameter";
    
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

        VisualEffect effect = gameObject.GetComponent<VisualEffect>();
        effect.SetFloat(DIAMETER, size);
        effect.Play();
    }
}
