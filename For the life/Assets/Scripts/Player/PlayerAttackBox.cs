using System.Collections.Generic;
using UnityEngine;

// 타겟이 대미지를 입었으면 여기서 충돌 확인 안하게 하기(Enable 시 다시 false로 초기화)
public class PlayerAttackBox : MonoBehaviour
{
    private Player player; //플레이어 스크립트 참조
    private List<Player> playersInRange = new List<Player>();  //범위 내 플레이어 리스트
    public bool isSingleAttack = false; // 싱글 공격 여부

    [Header("스킬 상태")]
    public int hpDamage; // 적의 스킬이 주는 피해량
    public int mentalDamage; // 적의 스킬이 주는 정신력 피해량
    public bool durationSkill; // 적의 스킬이 지속되는지 여부
    public PlayerSkillType skillType; //적의 스킬 타입 (단일 공격, 다중 공격 등)

    public bool successAttack = false; // 공격 성공 여부

    [SerializeField] private Collider2D hitbox;

    private bool prevEnabled = false;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //부모 오브젝트에서 Player 스크립트를 찾음
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (successAttack) // 공격이 성공한 상태일 때는 충돌 처리하지 않음
            return;

        if (isSingleAttack)
        {
            if (skillType == PlayerSkillType.Heal)
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Player in H1 range!"); //디버그 메시지 출력
                    Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
                    if(player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // 플레이어의 HP가 최대치 이상으로 회복되지 않도록 설정
                            continue; // 최대치 이상으로 회복되지 않도록 함
                        }

                        if (player != null) //적이 존재하면
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                        }
                        successAttack = true; // 공격 성공 상태로 설정
                    }
                }
                else if (player.target == null && other.gameObject.CompareTag("Player")) // 적 태그를 가진 오브젝트와 충돌했을 때
                {
                    Debug.Log($"PlayerAttackBox: Player in H2 range!"); //디버그 메시지 출력
                    Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음

                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // 플레이어의 HP가 최대치 이상으로 회복되지 않도록 설정
                            continue; // 최대치 이상으로 회복되지 않도록 함
                        }

                        if (player != null) //적이 존재하면
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                        }
                        successAttack = true; // 공격 성공 상태로 설정
                    }
                }
                playersInRange.Clear();
            }
            else
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //디버그 메시지 출력
                    Enemy enemy = other.GetComponent<Enemy>(); //플레이어 스크립트를 찾음
                    if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                        return;

                    if (enemy != null) //적이 존재하면
                    {
                        TakeDamage(enemy, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                    }
                    successAttack = true; // 공격 성공 상태로 설정
                }
                else if (player.target == null && other.gameObject.CompareTag("Enemy")) // 적 태그를 가진 오브젝트와 충돌했을 때
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //디버그 메시지 출력
                    Enemy enemy = other.GetComponent<Enemy>(); //플레이어 스크립트를 찾음

                    if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                        return;

                    if (enemy != null) //적이 존재하면
                    {

                        TakeDamage(enemy, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌

                    }
                    successAttack = true; // 공격 성공 상태로 설정
                }
            }
        }
        else if (!isSingleAttack)
        {
            if (skillType == PlayerSkillType.Heal)
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Player in H1 range!"); //디버그 메시지 출력
                    Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음
                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // 플레이어의 HP가 최대치 이상으로 회복되지 않도록 설정
                            continue; // 최대치 이상으로 회복되지 않도록 함
                        }

                        if (player != null) //적이 존재하면
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                        }
                        successAttack = true; // 공격 성공 상태로 설정
                    }
                }
                else if (player.target == null && other.gameObject.CompareTag("Player")) // 적 태그를 가진 오브젝트와 충돌했을 때
                {
                    Debug.Log($"PlayerAttackBox: Player in H2 range!"); //디버그 메시지 출력
                    Player player = other.GetComponent<Player>(); //플레이어 스크립트를 찾음

                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // 플레이어의 HP가 최대치 이상으로 회복되지 않도록 설정
                            continue; // 최대치 이상으로 회복되지 않도록 함
                        }

                        if (player != null) //적이 존재하면
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                        }
                        successAttack = true; // 공격 성공 상태로 설정
                    }
                }
                playersInRange.Clear();
            }
            else
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //디버그 메시지 출력
                    Enemy enemy = other.GetComponent<Enemy>(); //플레이어 스크립트를 찾음
                    if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                        return;

                    if (enemy != null) //적이 존재하면
                    {
                        TakeDamage(enemy, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌
                    }
                    successAttack = true; // 공격 성공 상태로 설정
                }
                else if (player.target == null && other.gameObject.CompareTag("Enemy")) // 적 태그를 가진 오브젝트와 충돌했을 때
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //디버그 메시지 출력
                    Enemy enemy = other.GetComponent<Enemy>(); //플레이어 스크립트를 찾음

                    if (enemy.currentHp <= 0) // 적의 HP가 0보다 큰지 확인
                        return;

                    if (enemy != null) //적이 존재하면
                    {

                        TakeDamage(enemy, hpDamage, mentalDamage); //플레이어에게 적의 공격력만큼 피해를 줌

                    }
                    successAttack = true; // 공격 성공 상태로 설정
                }
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

    public void SkillDamage(PlayerSkill skill)
    {
        hpDamage = skill.hpDamage; //스킬의 HP 피해량 설정
        mentalDamage = skill.mentalDamage; //스킬의 정신력 피해량 설정
        skillType = skill.playerSkillType; //스킬 타입 설정
    }

    public void TakeDamage(Enemy enemy, int hpDamage, int mentalDamage)
    {
        enemy.currentHp -= hpDamage; // 플레이어의 HP 감소
    }

    public void TakeHealBuffer(Player player, int hpHeal, int mentalHeal)
    {
        player.currentHp += hpHeal; // 플레이어의 HP 회복
        player.currentMental += mentalHeal; // 플레이어의 정신력 회복
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
