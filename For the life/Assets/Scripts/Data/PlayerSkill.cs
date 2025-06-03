using UnityEngine;

public enum PlayerSkillType
{
    None,
    SingleAttack,
    MultiAttack,
    Heal,
}

public enum PlayerSkillReach
{
    None,
    CloseAttack,
    LongAttack,
}

[CreateAssetMenu(fileName = "PS", menuName = "Skill/PlayerSkillData", order = 0)]
public class PlayerSkill : ScriptableObject
{
    public string skillName;
    public int hpDamage;
    public int mentalDamage;
    public float cooldownTime;
    public float castTime;
    public float durationTime;
    public bool isTrueActive = true;

    public PlayerSkillType playerSkillType;
    public PlayerSkillReach playerSkillReach;
}
