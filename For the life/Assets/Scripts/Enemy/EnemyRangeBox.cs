using UnityEngine;


public class EnemyRangeBox : MonoBehaviour
{
    private Enemy enemy; //적 스크립트 참조
    public bool isRange = false; // 타겟 범위 존재
    public bool isLongReach = true; // false : 단거리, true : 원거리

    [Header("스킬 상태")]
    public int hpDamage; // 적의 스킬이 주는 피해량
    public int mentalDamage; // 적의 스킬이 주는 정신력 피해량
    public bool durationSkill; // 적의 스킬이 지속되는지 여부
    public EnemySkillType skillType; //적의 스킬 타입 (단일 공격, 다중 공격 등)

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); //부모 오브젝트에서 Enemy 스크립트를 찾음
    }

    private void OnEnable()
    {
        isRange = false; //범위 초기화
        isLongReach = true; //원거리 여부 초기화
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (enemy.target != null)
        {
            if (other.gameObject == enemy.target) //플레이어 태그를 가진 오브젝트와 충돌했을 때
            {
                if (isLongReach == false) // 단거리 공격 범위일 때
                {
                    float distance = Vector2.Distance(transform.position, other.transform.position); //적과 플레이어 사이의 거리 계산
                    float threshold = 0.8f; //원하는 안쪽 범위 반경 예: 1유닛
                    if (distance <= threshold) //거리가 임계값 이하일 때
                    {
                        Debug.Log("EnemyRangeBox: Player in range!"); //디버그 메시지 출력
                        isRange = true; //타겟 범위 존재 여부를 true로 설정
                    }
                }
                else if (isLongReach == true) // 원거리 공격 범위일 때
                {
                    //원거리 공격 범위는 항상 true로 설정
                    //여기서는 거리 계산 없이 바로 범위 존재로 간주

                    Debug.Log("EnemyRangeBox: Player in long range!"); //디버그 메시지 출력
                    isRange = true; //타겟 범위 존재 여부를 true로 설정
                }
            }
        }
        else if (enemy.target == null && other.gameObject.CompareTag("Player")) //플레이어 태그를 가진 오브젝트와 충돌했을 때
        {
            Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
            if (player.currentHp <= 0) // 플레이어의 HP가 0보다 큰지 확인
            {
                isRange = false;
                return;
            }

            if (isLongReach == false) // 단거리 공격 범위일 때
            {
                float distance = Vector2.Distance(transform.position, other.transform.position); //적과 플레이어 사이의 거리 계산
                float threshold = 0.8f; //원하는 안쪽 범위 반경 예: 1유닛
                if (distance <= threshold) //거리가 임계값 이하일 때
                {
                    Debug.Log("EnemyRangeBox: Player in range!"); //디버그 메시지 출력
                    isRange = true; //타겟 범위 존재 여부를 true로 설정
                }
            }
            else if (isLongReach == true) // 원거리 공격 범위일 때
            {
                //원거리 공격 범위는 항상 true로 설정
                //여기서는 거리 계산 없이 바로 범위 존재로 간주

                Debug.Log("EnemyRangeBox: Player in long range!"); //디버그 메시지 출력
                isRange = true; //타겟 범위 존재 여부를 true로 설정
            }
            return;
        }

        if (other.gameObject.activeSelf == false)
        {
            isRange = false; //타겟이 비활성화되면 범위 존재 여부를 false로 설정
            enemy.target = null; //타겟을 null로 설정
            Debug.Log("PlayerRangeBox: Target is inactive, resetting target to null.");
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == enemy.target) //플레이어 태그를 가진 오브젝트와 충돌이 끝났을 때
        {
            Debug.Log("EnemyRangeBox: Player exited range!"); //디버그 메시지 출력
            isRange = false; //타겟 범위 존재 여부를 false로 설정
            enemy.target = null; //타겟을 null로 설정
        }
        if(other.gameObject.CompareTag("Player")) //플레이어 태그를 가진 오브젝트와 충돌이 끝났을 때
        {
            Debug.Log("EnemyRangeBox: Player exited range!"); //디버그 메시지 출력
            isRange = false; //타겟 범위 존재 여부를 false로 설정
            enemy.target = null; //타겟을 null로 설정
        }
    }

    public void ReachCheck(EnemySkill skill)
    {
        if(skill.enemySkillReach == EnemySkillReach.LongAttack)
        {
            isLongReach = true; // 원거리 공격 여부를 true로 설정
        }
        else if (skill.enemySkillReach == EnemySkillReach.CloseAttack)
        {
            isLongReach = false; // 단거리 공격 여부를 false로 설정
        }
    }

}