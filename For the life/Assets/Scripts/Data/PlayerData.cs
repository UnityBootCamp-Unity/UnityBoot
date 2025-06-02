using UnityEngine;

public enum PlayerType
{
    None,
    Knight,
    Archer,
    Priest,
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public string unityName;
    public int maxHp;
    public int currentHp;
    public int maxMental;
    public int currentMental;
    public float moveSpeed;

    public PlayerType playerType;
    public PlayerSkill[] skills; //플레이어가 사용하는 스킬들

    public int EquipmentLevel;
}
