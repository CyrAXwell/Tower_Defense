public interface ISkill 
{
    public void Use();

    public void Upgrade();

    public void UpdateTimer(float deltaTime);
    
    public bool IsReady();

    public float GetTimer();

    public SkillSO GetSkillSO();

    public int GetLevel();

    public bool IsMaxLevel();
}
