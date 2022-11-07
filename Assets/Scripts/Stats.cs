using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
  None,
  Physical,
  Magic
}

public enum TypeStats
{
  Level,
  HealthCurrent,
  HealthMax,
  StaminaCurrent,
  StaminaMax,
  ExperienceCurrent,
  ExperienceMax
}

public class Stats : MonoBehaviour
{
  /*
  1 strength = 2 Damage [Phys]
  1 dexterity = 0.1f attackSpeed, 1 critChance [Phys], 0.1f critModifier [Phys] 
  1 vitality = 5 HP, 5 Stamina, 0.2f speed 
  1 intelligence = 2 Damage [Magic]
  1 wisdom =  0.1f castSpeed, 1 critChance [Magic], 0.1f critModifier [Magic] 
  */
  private Dictionary<TypeStats, float> dictionaryStats = new Dictionary<TypeStats, float>();
  [SerializeField] private BaseStats baseStats;
  [SerializeField] private BaseParameters baseParameters;
  [SerializeField] private StatsUpgradePerLevel statsUpgradePerLevel;
  
  [Space, SerializeField, Header("Main")] private int level;
  [SerializeField] private float experienceCurrent;
  [SerializeField] private float experienceMax;
  [SerializeField] private float currentHP;
  [SerializeField] private float maxHP;
  [SerializeField] private float staminaMax;
  [SerializeField] private float staminaCurrent;
  [SerializeField, Header("Damage")] private DamageType damageType;
  [SerializeField] private float damagePhysical;
  [SerializeField] private float damageMagical;
  [SerializeField] private float attackSpeed;
  [SerializeField] private float castSpeed;
  [SerializeField] private float critChance;
  [SerializeField] private float critModifier;
  [SerializeField,Header("Others")] private float speed;

  private event Action OnExperienceGain;

  private void Update()
  {
    if (Input.GetKeyUp(KeyCode.B))
    {
      GetExperience(1);
    }
  }

  public void GetExperience(float value)
  {
    Debug.Log($"[Start] my current exp = {experienceCurrent}, max = {experienceMax}");
    experienceCurrent += value;
    if (experienceCurrent > experienceMax)
    {
      level++;
      experienceCurrent -= experienceMax;
    }
    Debug.Log($"[Finish] my current exp = {experienceCurrent}, max = {experienceMax}");
    OnExperienceGain();
  }

  private void AfterGetExperience()
  {
    
  }

  public float GetValueFromDictionary(TypeStats statsType)
  {
    return dictionaryStats[statsType];
  }
  
  private void SetDictionary()
  {
    Debug.Log($"$started, gameObject = {gameObject.name}");
    dictionaryStats.Add(TypeStats.Level, level);
    dictionaryStats.Add(TypeStats.HealthCurrent, currentHP);
    dictionaryStats.Add(TypeStats.HealthMax, maxHP);
    dictionaryStats.Add(TypeStats.StaminaCurrent, staminaCurrent);
    dictionaryStats.Add(TypeStats.StaminaMax, staminaMax);
    dictionaryStats.Add(TypeStats.ExperienceCurrent, experienceCurrent);
    dictionaryStats.Add(TypeStats.ExperienceMax, experienceMax);
    Debug.Log($"$finished, gameObject = {gameObject.name}");
  }

  private void OnEnable()
  {
    OnExperienceGain += AfterGetExperience;
    SetDictionary();
  }

  private void OnDisable()
  {
    OnExperienceGain -= AfterGetExperience;
  }
  
  [ContextMenu("SetStatsFromBaseStats")]
  public void SetStatsFromBaseStats()
  {
    if (level == 0)
    {
      level = 1;
    }
    maxHP = baseParameters.BaseHP + (statsUpgradePerLevel.healthPerLevel * baseStats.Vitality);
    currentHP = maxHP;
    staminaMax = baseParameters.BaseStamina + (statsUpgradePerLevel.staminaPerLevel * baseStats.Vitality);
    staminaCurrent = staminaMax;
    
    damagePhysical = baseParameters.BaseDamage + (statsUpgradePerLevel.physicalDamagePerLevel * baseStats.Strength);
    attackSpeed = baseParameters.BaseAttackSpeed + (statsUpgradePerLevel.attackSpeedPerLevel * baseStats.Dexterity);
    damageMagical = baseParameters.BaseDamage + (statsUpgradePerLevel.magicDamagePerLevel * baseStats.Intelligence);
    castSpeed = baseParameters.BaseCastSpeed + (statsUpgradePerLevel.castSpeedPerLevel * baseStats.Wisdom);
    speed = baseParameters.BaseSpeed + (statsUpgradePerLevel.speedPerLevel * baseStats.Vitality);
    experienceMax = statsUpgradePerLevel.experiencePerLevel * level;
    

    if (damageType == DamageType.Physical)
    {
      critChance = baseParameters.BaseCritChance + (statsUpgradePerLevel.physicalCritChancePerLevel * baseStats.Dexterity);
      critModifier = baseParameters.BaseCritModifier + (statsUpgradePerLevel.physicalCritModifierPerLevel * baseStats.Dexterity);
    }
    else if (damageType == DamageType.Magic)
    {
      critChance = baseParameters.BaseCritChance + (statsUpgradePerLevel.magicCritChancePerLevel * baseStats.Wisdom);
      critModifier = baseParameters.BaseCritModifier + (statsUpgradePerLevel.magicCritModifierPerLevel * baseStats.Wisdom);
    }
    else
    {
      critChance = baseParameters.BaseCritChance;
      critModifier = baseParameters.BaseCritModifier;
    }
  }
}
