using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    
    private Enemy enemy; //적 스크립트 참조
    public bool isSingleAttack = false; // 싱글 공격 여부

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); //부모 오브젝트에서 Enemy 스크립트를 찾음
    }

    /// <summary>
    /// 적이 공격 범위에 들어온 플레이어와 충돌했을 때 호출되는 함수
    /// 명중 시 플레이어의 HP 감소
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isSingleAttack)
        {
            if (other.gameObject == enemy.target)
            {
                Debug.Log($"EnemyAttackBox: Player in attack range!"); //디버그 메시지 출력
                /*Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
                if (player != null) //플레이어가 존재하면
                {
                    player.TakeDamage(enemy.enemyData.attackPower); //플레이어에게 적의 공격력만큼 피해를 줌
                    Debug.Log($"Enemy {enemy.enemyData.unityName} attacked {player.name} for {enemy.enemyData.attackPower} damage.");
                }*/
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Player")) // 플레이어 태그를 가진 오브젝트와 충돌했을 때
            {
                Debug.Log("EnemyAttackBox: Player in multi-attack range!"); // 디버그 메시지 출력
                /*Player player = other.GetComponent<Player>(); // 플레이어 스크립트를 찾음
                if (player != null) // 플레이어가 존재하면
                {
                    player.TakeDamage(enemy.enemyData.attackPower); // 플레이어에게 적의 공격력만큼 피해를 줌
                    Debug.Log($"Enemy {enemy.enemyData.unityName} multi-attacked {player.name} for {enemy.enemyData.attackPower} damage.");
                }*/
            }
        }
    }

    /// <summary>
    ///  적의 공격이 단일인지 멀티인지 확인하고 해당 로직을 실행합니다.
    /// </summary>
    /// <param name="skill"></param>
    public void SingleCheck(EnemySkill skill)
    {
        if(skill.enemySkillType == EnemySkillType.SingleAttack)
        {
            isSingleAttack = true; // 싱글 공격 여부를 true로 설정
        }
        else if(skill.enemySkillType == EnemySkillType.MultiAttack)
        {
            isSingleAttack = false; // 멀티 공격 여부를 false로 설정
        }
    }
}
