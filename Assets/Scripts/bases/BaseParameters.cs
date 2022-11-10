using UnityEngine;

[CreateAssetMenu(fileName = "Base Parameters", menuName = "Stats/Create Parameters", order = 55)]
public class BaseParameters : ScriptableObject
{
    public int CountStatPointsPerLevel = 5;
    public float BaseHP = 20f;
    public float BaseStamina = 10f;
    public float BaseDamage = 2f;
    public DamageType BaseDamageType = DamageType.None;
    public float BaseAttackSpeed = 1f;
    public float BaseCastSpeed = 1f;
    public float BaseCritChance = 5f;
    public float BaseCritModifier = 1.5f;
    public float BaseSpeed = 1f;
}