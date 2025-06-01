using UnityEngine;

public enum EnemyType
{
    None,
    Skeleton,
    Fielder,
    Ghost,
}

[CreateAssetMenu(fileName = "", menuName = "Enemy/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    public string unityName;
    public int maxHp;
    public int currentHp;

    public float moveSpeed;

    //Àû Á¾Á·
    public EnemyType enemyType;

    public EnemySkill[] skills;
}
