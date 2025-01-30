public class AbstructSkill : ISkill
{
    private SkillSO _skillSO;
    private float _timer;
    private int _level;

    public virtual void Use()
    {
        
    }

    public virtual void UpdateTimer(float deltaTime)
    {
        if (_timer > 0)
        {
            _timer -= deltaTime;
        }
    }
    
    public virtual bool IsReady()
    {
        return _timer <= 0;
    }

    public float GetTimer()
    {
        return _timer;
    }

    public SkillSO GetSkillSO()
    {
        return _skillSO;
    }

    public void Upgrade()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }

    public bool IsMaxLevel()
    {
        return _level == _skillSO.GetUpgradeAmount() + 1;
    }
}
