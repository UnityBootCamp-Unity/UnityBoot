using UnityEngine;

public class PlayerRangeBox : MonoBehaviour
{
    private Player player; //플레이어 스크립트 참조
    public bool isRange = false; // 타겟 범위 존재
    public bool isLongReach = true; // false : 단거리, true : 원거리

    public bool healBuffer; // 힐 버퍼 여부

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //부모 오브젝트에서 Player 스크립트를 찾음
    }

    private void OnEnable()
    {
        isRange = false; //범위 초기화
        isLongReach = true; //원거리 여부 초기화
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (healBuffer)
        {
            Debug.Log($"[RangeBox] 충돌 대상 태그: {other.tag}");
            if (other.gameObject == player.target) //플레이어 태그를 가진 오브젝트와 충돌했을 때
            {
                Player targetPlayer = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
                if (targetPlayer.currentHp <= 0) // 플레이어의 HP가 0보다 큰지 확인
                {
                    isRange = false;
                    return;
                }
                isRange = true; //타겟 범위 존재 여부를 true로 설정
                return;
            }
            else if (player.target == null && other.gameObject.CompareTag("Player")) //플레이어 태그를 가진 오브젝트와 충돌했을 때
            {
                Player targetPlayer = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
                if (targetPlayer.currentHp <= 0) // 플레이어의 HP가 0보다 큰지 확인
                {
                    isRange = false;
                    return;
                }
                isRange = true; //타겟 범위 존재 여부를 true로 설정
                return;
            }
        }
        
        if (healBuffer) { return; } // 힐 버퍼가 활성화된 상태에서 다른 충돌은 무시

        if (other.gameObject == player.target) //Enemy 태그를 가진 오브젝트와 충돌했을 때
        {
            Enemy enemy = other.GetComponent<Enemy>(); //적 스크립트를 찾음
            if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
            {
                isRange = false;
                return;
            }

            isRange = true; //타겟 범위 존재 여부를 true로 설정
            return;
        }
        else if (player.target == null)
        {
            if (other.gameObject.CompareTag("Enemy")) //Enemy 태그를 가진 오브젝트와 충돌했을 때
            {
                Enemy enemy = other.GetComponent<Enemy>(); //적 스크립트를 찾음
                if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                {
                    isRange = false;
                    return;
                }

                isRange = true; //타겟 범위 존재 여부를 true로 설정
                //여기 자체로 타겟을 정하는 것도 좋았을 듯
                return;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (healBuffer)
        {
            if (other.gameObject == player.target) //Player 태그를 가진 오브젝트와 충돌이 끝났을 때
            {
                isRange = false; //타겟 범위 존재 여부를 false로 설정
            }
            else if (other.gameObject.CompareTag("Player")) //Player 태그를 가진 오브젝트와 충돌이 끝났을 때
            {
                isRange = false; //타겟 범위 존재 여부를 false로 설정
            }
        }

        if(healBuffer) { return; } // 힐 버퍼가 활성화된 상태에서 다른 충돌은 무시

        if (other.gameObject == player.target) //Enemy 태그를 가진 오브젝트와 충돌이 끝났을 때
        {
            isRange = false; //타겟 범위 존재 여부를 false로 설정
        }
        else if (other.gameObject.CompareTag("Enemy")) //Enemy 태그를 가진 오브젝트와 충돌이 끝났을 때
        {
            isRange = false; //타겟 범위 존재 여부를 false로 설정
        }
    }

    public void CheckHealBuffer(PlayerSkill skill)
    {
        if (skill.playerSkillType == PlayerSkillType.Heal)
        {
            healBuffer = true; // 힐 버퍼 활성화
        }
        else
        {
            healBuffer = false; // 힐 버퍼 비활성화
        }
    }
}
