using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStats
{
   protected int Strength = 10;
   protected int Dexterity = 10;
   protected int Vitality = 10;
   protected int Intelligence = 10;
   protected int Wisdom = 10;
   
   protected float BaseHP = 10f;
   protected float BaseStamina = 10f; 
   protected float BaseDamage = 1f;
   protected DamageType BaseDamageType = DamageType.None;
   protected float BaseAttackSpeed = 1f;
   protected float BaseCastSpeed = 1f;
   protected float BaseCritChance = 5f;
   protected float BaseCritModifier = 1f;
   protected float BaseSpeed = 1f;
}
