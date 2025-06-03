using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// 타겟이 대미지를 입었으면 여기서 충돌 확인 안하게 하기(Enable 시 다시 false로 초기화)
public class EnemyAttackBox : MonoBehaviour
{
    
    private Enemy enemy; //적 스크립트 참조
    public bool isSingleAttack = false; // 싱글 공격 여부

    [Header("스킬 상태")]
    public int hpDamage; // 적의 스킬이 주는 피해량
    public int mentalDamage; // 적의 스킬이 주는 정신력 피해량
    public bool durationSkill; // 적의 스킬이 지속되는지 여부
    public EnemySkillType skillType; //적의 스킬 타입 (단일 공격, 다중 공격 등)

    public bool successAttack = false; // 공격 성공 여부

    [SerializeField] private Collider2D hitbox;

    private bool prevEnabled = false;


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
        if (successAttack) // 공격이 성공한 상태일 때는 충돌 처리하지 않음
            return;

        if (isSingleAttack)
        {
            if (other.gameObject == enemy.target)
            {
                Debug.Log($"EnemyAttackBox: Player in attack range!"); //디버그 메시지 출력
                Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음

                if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                    return;

                if (player != null) //플레이어가 존재하면
                {
                    if (skillType == EnemySkillType.Heal)
                    {
                    }
                    else
                    {
                        TakeDamage(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                    }
                }
                successAttack = true; // 공격 성공 상태로 설정
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Player")) // 플레이어 태그를 가진 오브젝트와 충돌했을 때
            {
                Debug.Log("EnemyAttackBox: Player in multi-attack range!"); // 디버그 메시지 출력
                Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음

                if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                    return;

                if (player != null) //플레이어가 존재하면
                {
                    if (skillType == EnemySkillType.Heal)
                    {
                    }
                    else
                    {
                        TakeDamage(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                    }
                }
                successAttack = true; // 공격 성공 상태로 설정
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

    public void SkillDamage(EnemySkill skill)
    {
        hpDamage = skill.hpDamage; //스킬의 HP 피해량 설정
        mentalDamage = skill.mentalDamage; //스킬의 정신력 피해량 설정
        skillType = skill.enemySkillType; //스킬 타입 설정
    }

    public void TakeDamage(Player player, int hpDamage, int mentalDamage)
    {
        player.currentHp -= hpDamage; // 플레이어의 HP 감소
        player.currentMental -= mentalDamage; // 플레이어의 정신력 감소
    }

    /// <summary>
    /// 매 프레임마다 호출되어 콜라이더의 활성화 상태를 확인합니다.
    /// </summary>
    void Update()
    {
        if (hitbox != null)
        {
            if (hitbox.enabled && !prevEnabled)
            {
                // 콜라이더가 방금 켜졌을 때
                OnColliderEnabled();
            }

            prevEnabled = hitbox.enabled;
        }
    }

    /// <summary>
    /// 콜라이더가 활성화되었을 때 호출되는 함수입니다.
    /// 콜라이더 활성화를 이벤트함수를 따로 또 만들어서
    /// 애니메이션에 적용시키는 방식도 가능
    /// </summary>
    private void OnColliderEnabled()
    {
        successAttack = false;
        isSingleAttack = false;
        Debug.Log("Collider enabled → 공격 정보 초기화됨");
    }

}
