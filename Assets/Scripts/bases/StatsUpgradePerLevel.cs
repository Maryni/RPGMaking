using UnityEngine;

[CreateAssetMenu(fileName = "Stats Upgrade Per Level", menuName = "Stats/Create Stats Upgrade Per Level", order = 56)]
public class StatsUpgradePerLevel : ScriptableObject
{
    [Header("Level")] public float experiencePerLevel;
    [Header("Strength")] public float physicalDamagePerLevel;
    [Header("Dexterity")] public float attackSpeedPerLevel;
    public float physicalCritChancePerLevel;
    public float physicalCritModifierPerLevel;
    [Header("Vitality")] public float healthPerLevel;
    public float staminaPerLevel;
    public float speedPerLevel;
    [Header("Intelligence")] public float magicDamagePerLevel;
    [Header("Wisdom")] public float castSpeedPerLevel;
    public float magicCritChancePerLevel;
    public float magicCritModifierPerLevel;
}