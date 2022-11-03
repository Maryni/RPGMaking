using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
  None,
  Physical,
  Magic
}

public class Stats : BaseStats
{
  /*
  1 strengts = 2 Damage [Phys]
  1 dexterity = 0.1f speed, 0.1f attackSpeed, 1 critChance [Phys], 0.1f critModifier [Phys] 
  1 vitality = 5 HP, 5 Stamina, 0.2f speed 
  1 intelligence = 2 Damage [Magic]
  1 wisdom =  0.1f castSpeed, 1 critChance [Magic], 0.1f critModifier [Magic] 
  */
  [SerializeField, Header("Main")] private float currentHP;
  [SerializeField] private float maxHP;
  [SerializeField] private float stamina;
  [SerializeField, Header("Damage")] private float damage;
  [SerializeField] private DamageType damageType;
  [SerializeField] private float attackSpeed;
  [SerializeField] private float castSpeed;
  [SerializeField] private float critChance;
  [SerializeField] private float critModifier;
  [SerializeField,Header("Others")] private float speed;

  [ContextMenu("SetStatsFromBaseStats")]
  public void SetStatsFromBaseStats()
  {
    maxHP = BaseHP + (5 * Vitality);
    currentHP = maxHP;
    stamina = BaseStamina + (5 * Vitality);
    
    if (damageType == DamageType.Physical)
    {
      damage = BaseDamage + (2 * Strength);
      attackSpeed = BaseAttackSpeed + (0.1f * Dexterity);
      critChance = BaseCritChance + (1 * Dexterity);
      critModifier = BaseCritModifier + (0.1f * Dexterity);
    }
    else if (damageType == DamageType.Magic)
    {
      damage = BaseDamage + (2 * Intelligence);
      castSpeed = BaseCastSpeed + (0.1f * Wisdom);
      critChance = BaseCritChance + (1 * Wisdom);
      critModifier = BaseCritModifier + (0.1f * Wisdom);
    }

    speed = BaseSpeed + (0.1f * Dexterity) + (0.2f * Vitality);
  }
  
}
