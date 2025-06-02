using UnityEngine;

// 타겟이 대미지를 입었으면 여기서 충돌 확인 안하게 하기(Enable 시 다시 false로 초기화)
public class PlayerAttackBox : MonoBehaviour
{
    private Player player; //플레이어 스크립트 참조
    public bool isSingleAttack = false; // 싱글 공격 여부

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //부모 오브젝트에서 Player 스크립트를 찾음
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isSingleAttack)
        {
            if (other.gameObject == player.target)
            {
                Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //디버그 메시지 출력
                /*Enemy enemy = other.GetComponent<Enemy>(); //적 스크립트를 찾음
                if (enemy != null) //적이 존재하면
                {
                    enemy.TakeDamage(player.playerData.attackPower); //적에게 플레이어의 공격력만큼 피해를 줌
                    Debug.Log($"Player {player.playerData.unityName} attacked {enemy.name} for {player.playerData.attackPower} damage.");
                }*/
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Enemy")) // 적 태그를 가진 오브젝트와 충돌했을 때
            {
                Debug.Log("PlayerAttackBox: Enemy in multi-attack range!"); // 디버그 메시지 출력
                /*Enemy enemy = other.GetComponent<Enemy>(); // 적 스크립트를 찾음
                if (enemy != null) // 적이 존재하면
                {
                    enemy.TakeDamage(player.playerData.attackPower); // 적에게 플레이어의 공격력만큼 피해를 줌
                    Debug.Log($"Player {player.playerData.unityName} multi-attacked {enemy.name} for {player.playerData.attackPower} damage.");
                }*/
            }
        }
    }

    public void SingleCheck(PlayerSkill skill)
    {
        if (skill.playerSkillType == PlayerSkillType.SingleAttack)
        {
            isSingleAttack = true; // 싱글 공격 여부를 true로 설정
        }
        else if (skill.playerSkillType == PlayerSkillType.MultiAttack)
        {
            isSingleAttack = false; // 멀티 공격 여부를 false로 설정
        }
    }
}
