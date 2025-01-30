using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public const int MAX_SKILL_AMOUNT = 5;

    public EventHandler<bool> OnAddNewSkill;
    public EventHandler<int> OnUseSkill;
    public EventHandler OnUpdateTimer;
    public EventHandler<int> OnSelectSkill;
    public EventHandler<UpgradeSkillEventArgs> OnUpgradeSkill;
    public class UpgradeSkillEventArgs
    {
        public int Index;
        public int Level;
        public bool IsMaxLevel;
    }

    [SerializeField] private List<ISkill> _skillList;

    public static SkillManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private int _selectedSkillIndex;

    private void Start()
    {
        _skillList = new List<ISkill>();

        _selectedSkillIndex = 0;
        OnSelectSkill?.Invoke(this, _selectedSkillIndex);
    }

    public void AddNewSkill(ISkill skill)
    {
        _skillList.Add(skill);

        OnAddNewSkill?.Invoke(this, skill.IsReady());
        OnSelectSkill?.Invoke(this, _selectedSkillIndex);
    }

    public void UpgradeSkill(ISkill skill)
    {
        skill.Upgrade();
        int i = 0;
        while (skill != _skillList[i])
        {
            i++;
        }

        OnUpgradeSkill?.Invoke(this, new UpgradeSkillEventArgs{ Index = i, Level = skill.GetLevel() , IsMaxLevel = skill.IsMaxLevel()});
    }

    private void Update()
    {
        if (_skillList.Count <= 0)
            return;
        
        foreach (ISkill skill in _skillList)
        {
            skill.UpdateTimer(Time.deltaTime);
        }
        
        OnUpdateTimer?.Invoke(this, EventArgs.Empty);

        int mouseScrollWheel = (int)(Input.GetAxisRaw("Mouse ScrollWheel") * 10);
        if (mouseScrollWheel!= 0)
        {
            int newSelectedSkillIndex = _selectedSkillIndex + mouseScrollWheel;
            if (newSelectedSkillIndex >= 0 && newSelectedSkillIndex < _skillList.Count)
            {
                _selectedSkillIndex = (int)newSelectedSkillIndex;
                OnSelectSkill?.Invoke(this, _selectedSkillIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSkill(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSkill(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSkill(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSkill(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSkill(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSkill(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSkill(6);

        if (Input.GetMouseButtonDown(1))
        {
            if (_skillList[_selectedSkillIndex].IsReady())
            {
                _skillList[_selectedSkillIndex].Use();   
                OnUseSkill?.Invoke(this, _selectedSkillIndex);
            }
        }
    }

    private void SelectSkill(int index)
    {
        if (index <= _skillList.Count - 1)
        {
            _selectedSkillIndex = index;
            OnSelectSkill?.Invoke(this, _selectedSkillIndex);
        }
    }

    public float GetSkillTimer(int skillIndex)
    {
        return _skillList[skillIndex].GetTimer();
    }

    public bool IsSkillReady(int skillIndex)
    {
        return _skillList[skillIndex].IsReady();
    }

    public SkillSO GetSkillSO(int skillIndex)
    {
        return _skillList[skillIndex].GetSkillSO();
    }

    private void ChangeEntityDataTest()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Entity playerEntity = entityManager.CreateEntityQuery(typeof(Friendly)).GetSingletonEntity();
            // ShootAttack shootAttackNew = entityManager.GetComponentData<ShootAttack>(playerEntity);
            // shootAttackNew.damageAmount += 20;
            // entityManager.SetComponentData<ShootAttack>(playerEntity, shootAttackNew);

            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<ShootAttack>().Build(entityManager);
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<ShootAttack> shootAttackarray = entityQuery.ToComponentDataArray<ShootAttack>(Allocator.Temp);

            for (int i = 0; i < shootAttackarray.Length; i++)
            {
                ShootAttack shootAttack = shootAttackarray[i];
                shootAttack.damageAmount += 10;
                //entityManager.SetComponentData(entityArray[i], shootAttack);
                shootAttackarray[i] = shootAttack;
            }
            entityQuery.CopyFromComponentDataArray(shootAttackarray);

            
        }
    }
}
