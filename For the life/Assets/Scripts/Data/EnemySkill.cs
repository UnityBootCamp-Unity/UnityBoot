using UnityEngine;

public enum EnemySkillType
{
    None,
    SingleAttack,
    MultiAttack,
    Heal,
}

public enum EnemySkillReach
{
    None,
    CloseAttack,
    LongAttack,
}

[CreateAssetMenu(fileName = "SK", menuName = "Skill/SkillData", order = 0)]
public class EnemySkill : ScriptableObject
{
    public string skillName;
    public int hpDamage;
    public int mentalDamage;
    public float cooldownTime;
    public float castTime;
    public float durationTime;
    public bool isTrueActive = true;

    public EnemySkillType enemySkillType;
    public EnemySkillReach enemySkillReach;
}