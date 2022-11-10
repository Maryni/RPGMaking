using System;
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
  ExperienceMax,
  Damage
}

public enum TypeParameter
{
  Strenght,
  Dexterity,
  Vitality,
  Intelligence,
  Wisdom
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

  #region Inspector variables

  [SerializeField] private BaseStats baseStats;
  [SerializeField] private BaseParameters baseParameters;
  [SerializeField] private StatsUpgradePerLevel statsUpgradePerLevel;
  
  [Space, SerializeField, Header("Main")] private int level;
  [SerializeField] private int countFreeStatPoints;
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

  #endregion Inspector variables

  #region properties

  public BaseStats BaseStats => baseStats;

  #endregion properties

  #region private variables

  private Dictionary<TypeStats, float> dictionaryStats = new Dictionary<TypeStats, float>();
  private event Action OnExperienceGain;
  private event Action OnLevelUp;

  #endregion private variables

  #region Unity functions

  private void Update()
  {
    if (Input.GetKeyUp(KeyCode.B))
    {
      GetExperience(1);
    }
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


  #endregion Unity functions
  
  #region public functions

  public void SetActionsOnLevelUp(params Action[] actions)
  {
    foreach (var action in actions)
    {
      OnLevelUp += action;
    }
  }
  public void SetValueByType(TypeStats typeStats,float value)
  {
    switch (typeStats)
    {
      case TypeStats.Damage: GetDamage(value); break;
      case TypeStats.StaminaCurrent: GetStamina(value); break;
      case TypeStats.ExperienceCurrent: GetExperience(value); break;
      default: break;
    }
  }

  public bool IsPlayerWillBeDeadAfterDamage(float damage)
  {
    if (currentHP - damage <= 0)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  
  /// <summary>
  /// Gets static value, which set while OnEnable
  /// </summary>
  /// <param name="typeStats"></param>
  /// <returns></returns>
  public float GetValueFromDictionary(TypeStats typeStats)
  {
    return dictionaryStats[typeStats];
  }

  public float GetValue(TypeStats typeStats)
  {
    float value = 0f;
    switch (typeStats)
    {
      case TypeStats.Level : value = level; break;
      case TypeStats.HealthCurrent : value = currentHP; break;
      case TypeStats.HealthMax : value = maxHP; break;
      case TypeStats.StaminaCurrent : value = staminaCurrent; break;
      case TypeStats.StaminaMax : value = staminaMax; break;
      case TypeStats.ExperienceCurrent : value = experienceCurrent; break;
      case TypeStats.ExperienceMax : value = experienceMax; break; 
      default: value = 0f; break;
    }
    return value;
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

  public void IncreaseStat(TypeParameter typeParameter)
  {
      if (countFreeStatPoints > 0)
      {
          switch (typeParameter)
          {
            case TypeParameter.Strenght : baseStats.Strength++; break;
            case TypeParameter.Dexterity : baseStats.Dexterity++;break;
            case TypeParameter.Vitality : baseStats.Vitality++; break;
            case TypeParameter.Intelligence : baseStats.Intelligence++; break;
            case TypeParameter.Wisdom : baseStats.Wisdom++; break;
            default: break;
          }

          countFreeStatPoints--;
      }
      else
      {
        Debug.Log($"countFreeStatPoints = {countFreeStatPoints}");
      }
  }

  #endregion public functions
  
  #region private functions

  private void GetExperience(float value)
  {
    experienceCurrent += value;
    if (experienceCurrent >= experienceMax)
    {
      level++;
      experienceCurrent -= experienceMax;
      countFreeStatPoints += baseParameters.CountStatPointsPerLevel;
      OnLevelUp();
    }
    OnExperienceGain();
  }
  
  private void GetDamage(float value)
  {
    currentHP -= value;
    if (currentHP <= 0)
    {
      currentHP = 0;
      Debug.Log($"Player are dead");
    }
  }
  
  private void GetStamina(float value)
  {
    staminaCurrent -= value;
    if (staminaCurrent < 0)
    {
      staminaCurrent = 0;
    }
  }
  
  private void SetDictionary()
  {
    dictionaryStats.Add(TypeStats.Level, level);
    dictionaryStats.Add(TypeStats.HealthCurrent, currentHP);
    dictionaryStats.Add(TypeStats.HealthMax, maxHP);
    dictionaryStats.Add(TypeStats.StaminaCurrent, staminaCurrent);
    dictionaryStats.Add(TypeStats.StaminaMax, staminaMax);
    dictionaryStats.Add(TypeStats.ExperienceCurrent, experienceCurrent);
    dictionaryStats.Add(TypeStats.ExperienceMax, experienceMax);
  }
  
  private void AfterGetExperience()
  {
    
  }

  #endregion private functions
}
