using UnityEngine;

//이 게임에서 사용할 타입에 대한 데이터입니다.
public enum Type
{
    노말, 불, 물, 풀, 전기, 얼음, 격투, 독, 땅, 비행, 에스퍼, 벌레, 바위, 고스트, 드래곤, 악, 강철, 페어리, 없음
}

[CreateAssetMenu(fileName = "", menuName = "Battle/Unity")]
public class Unit : ScriptableObject
{
    public string unitName;
    public int maxHP;
    public int currentHP;

    //포켓몬 게임 기준 가질 수 있는 타입의 최대 수는 2개
    public Type type1;
    public Type type2;

    public Skill[] skills;
}
