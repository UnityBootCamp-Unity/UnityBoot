using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("데이터")]
    public PlayerData playerData; //ScriptableObject로 플레이어의 데이터 관리
    public PlayerSkill[] skills; //플레이어가 사용하는 스킬들

    [Header("공격")]
    public PlayerAttackBox[] playerAttackBox; //플레이어의 공격 박스
    public PlayerRangeBox[] playerRangeBox; //플레이어의 범위 탐지 박스
    public float cooldownTime = 0.0f; //플레이어의 공격 쿨타임

    [Header("애니메이션")]
    public Animator animator; //플레이어의 애니메이션 컨트롤러

    [Header("플레이어 상태")]
    public string unitName; // 플레이어의 이름
    public int maxHp; // 최대 HP
    public int currentHp; //현재 HP
    public int maxMental; // 최대 정신력
    public int currentMental; // 현재 정신력
    public string behavior; //플레이어의 행동 (예: 공격, 방어 등)
    public bool isDead = false; //죽었는지 여부
    
    /*public int EquipmentLevel; //플레이어의 장비 레벨
    public int attackPower; //플레이어의 공격력*/

    public bool isFacingRight = false; //현재 플레이어(자신)의 방향 (오른쪽을 바라보고 있는지 여부)
    public bool isSelected = false; //플레이어가 선택되었는지 여부

    private Vector3 moveTargetPosition; //플레이어의 이동 목표 위치
    private bool isMoving = false; //플레이어가 이동 중인지 여부

    public float duration = 0.0f; //플레이어의 지속 시간
    public bool casting = false; //플레이어의 스킬 시전 여부
    public bool doing = false; //플레이어의 스킬 지속 여부
    public bool durationSkill; // 적의 스킬이 지속되는지 여부

    public GameObject target; //플레이어의 타겟


    private void OnEnable()
    {
        animator = GetComponent<Animator>(); //플레이어의 애니메이션 컨트롤러를 가져옴

        currentHp = playerData.maxHp; //플레이어의 현재 HP를 최대 HP로 초기화
        isDead = false; //플레이어의 죽음 상태 초기화
        skills = playerData.skills; //플레이어의 스킬을 초기화
        target = null; //타겟 초기화
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] != null) //스킬이 있으면
            {
                skills[i].isTrueActive = true; //스킬을 사용할 수 있는 상태로 설정
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (playerData == null) //플레이어 데이터가 설정되지 않았으면
        {
            Debug.LogError("PlayerData is not assigned! Please assign a PlayerData ScriptableObject to the Player component.");
        }
        else
        {
            unitName = playerData.unityName; //플레이어의 이름 설정
            maxHp = playerData.maxHp; //플레이어의 최대 HP 설정
            currentHp = maxHp; //플레이어의 현재 HP를 최대 HP로 초기화
            maxMental = playerData.maxMental; //플레이어의 최대 정신력 설정
            currentMental = maxMental; //플레이어의 현재 정신력을 최대 정신력으로 초기화
            behavior = "Idle"; //플레이어의 초기 행동 상태 설정

        }
    }

    private void Update()
    {
        if (isDead) return; //플레이어가 죽었으면 함수 종료

        if (currentHp > maxHp) //현재 HP가 최대 HP보다 크면
        {
            currentHp = maxHp; //현재 HP를 최대 HP로 설정
        }

        if (currentHp <= 0)
        {
            isDead = true; //죽음 상태로 설정
            Debug.Log($"{unitName} has died.");
            animator.SetTrigger("Death"); //죽음 애니메이션 트리거 설정
            StartCoroutine(WaitForActiveFalse()); //5초 후에 플레이어 오브젝트 비활성화
            return; //함수 종료

        }

        if (target != null)
        {
            Enemy enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent == null || enemyComponent.currentHp <= 0 || !target.activeSelf)
            {
                target = null; // 죽었거나 비활성화되면 타겟 초기화
            }
        }

        if (DefenseManager.instance.isTurning) //턴이 진행 중일 때
        {
            animator.SetBool("isMoving", false); // 이동중 애니메이션 해제
            duration = 0.0f; // 지속하고 있는 스킬 중지
            return; //함수 종료
        }
        else if (!DefenseManager.instance.isTurning) //턴이 진행 중이지 않을 때
        {
            /*if (target == null || target.activeSelf == false) //타겟이 없으면
            {
                target = FindClosestTarget(); //타겟을 찾는 함수 호출
            }*/
            /*bool inRange = false; //타겟 범위 여부 초기화
            for (int i = 0; i < playerRangeBox.Length; i++)
            {
                if (playerRangeBox[i].isRange == true) //플레이어의 범위 박스가 활성화되어 있으면
                {
                    inRange = true; //타겟이 범위에 있음
                    break; //반복문 종료
                }
            }*/

            if (casting || doing) return; //스킬 시전 중이거나 지속 중이면 함수 종료

            if (isMoving)
            {
                animator.SetBool("isMoving", true); //이동 중 애니메이션 활성화
                transform.position = Vector2.MoveTowards(transform.position, moveTargetPosition,
                    playerData.moveSpeed * Time.deltaTime); //플레이어를 이동 목표 위치로 이동

                if (Vector2.Distance(transform.position, moveTargetPosition) < 0.05f) //목표 위치에 도달했을 때
                {
                    isMoving = false; //이동 중 상태 해제
                    animator.SetBool("isMoving", false); //이동 애니메이션 비활성화
                }
            }
            else
            {
                if (target != null && target.activeSelf) //타겟이 존재하고 활성화되어 있으면
                {
                    bool inRange = false; //타겟 범위 여부 초기화
                    for (int i = 0; i < playerRangeBox.Length; i++) //모든 범위 박스를 순회하면서
                    {
                        //playerRangeBox[i].ReachCheck(skills[i]); //플레이어의 원거리 체크
                        if (playerRangeBox[i].isRange == true) //범위가 존재하면
                        {
                            inRange = true; //타겟이 범위에 있음
                            break; //루프 종료
                        }
                    }
                    if (inRange)
                    {
                        AttackTrigger(); //타겟 범위에 적이 있으면 공격 함수 호출
                        animator.SetBool("isMoving", false); //이동 중 애니메이션 해제
                    }
                    else if (!inRange)
                    {
                        animator.SetBool("isMoving", true); //이동 중 애니메이션 설정
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); //현재 애니메이션 상태 정보 가져오기
                        if (stateInfo.IsName("Walk")) //현재 애니메이션이 걷기 상태일 때
                        {
                            MoveToTarget(); //타겟으로 이동하는 함수 호출
                        }
                    }
                }
                else
                {

                    bool inRange = false; //타겟 범위 여부 초기화
                    for (int i = 0; i < playerRangeBox.Length; i++) //모든 범위 박스를 순회하면서
                    {
                        if (playerRangeBox[i].isRange == true) //범위가 존재하면
                        {
                            inRange = true; //타겟이 범위에 있음
                            break; //루프 종료
                        }
                    }
                    if (inRange)
                    {
                        target = FindClosestTarget(); //타겟 범위에 플레이어가 있으면 가장 가까운 적을 찾음
                        AttackTrigger(); //타겟 범위에 적이 있으면 공격 함수 호출
                        target = null; //타겟 초기화
                    }
                }
            }
            cooldownTime -= Time.deltaTime; //쿨타임 감소

        }
    }


    /// <summary>
    /// 가장 가까운 적을 찾는 함수
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //적 오브젝트를 찾음
        GameObject closestEnemy = null; //가장 가까운 적 초기화
        float closestDistance = Mathf.Infinity; //가장 가까운 거리 초기화
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript == null || enemyScript.currentHp <= 0) //적이 없거나 HP가 0 이하인 경우 무시
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position); //플레이어와 적 사이의 거리 계산
            if (distance < closestDistance) //가장 가까운 적을 찾음
            {
                closestDistance = distance; //가장 가까운 거리 업데이트
                closestEnemy = enemy; //가장 가까운 적 업데이트
            }
        }

        return closestEnemy; //가장 가까운 적 반환
    }

    /// <summary>
    /// 체력이 가장 낮은 적을 찾는 함수
    /// 자동 완성으로 아직 확인 안함
    /// </summary>
    /// <returns></returns>
    GameObject FindLessHpTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //적 오브젝트를 찾음
        GameObject lessHpEnemy = null; //체력이 가장 낮은 적 초기화
        int lessHp = int.MaxValue; //가장 낮은 체력 초기화
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>(); //적 컴포넌트 가져오기
            if (enemyComponent != null && enemyComponent.currentHp < lessHp) //적이 존재하고 현재 체력이 가장 낮으면
            {
                lessHp = enemyComponent.currentHp; //가장 낮은 체력 업데이트
                lessHpEnemy = enemy; //체력이 가장 낮은 적 업데이트
            }
        }
        return lessHpEnemy; //체력이 가장 낮은 적 반환
    }

    GameObject FindLessHpPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //플레이어 오브젝트를 찾음
        GameObject lessHpPlayer = null; //체력이 가장 낮은 플레이어 초기화
        int lessHp = int.MaxValue; //가장 낮은 체력 초기화
        foreach (GameObject player in players)
        {
            Player playerComponent = player.GetComponent<Player>(); //플레이어 컴포넌트 가져오기
            if (playerComponent != null && playerComponent.currentHp < lessHp) //플레이어가 존재하고 현재 체력이 가장 낮으면
            {
                lessHp = playerComponent.currentHp; //가장 낮은 체력 업데이트
                lessHpPlayer = player; //체력이 가장 낮은 플레이어 업데이트
            }
        }
        return lessHpPlayer; //체력이 가장 낮은 플레이어 반환
    }

    /// <summary>
    /// 스킬 갯수까지 랜덤으로 선택해서 적이 범위에 있을 경우 공격한다.
    /// </summary>
    public void AttackTrigger()
    {
        /*if (target == null) return; //타겟이 없으면 함수 종료
        //적의 공격 로직을 여기에 추가
        //예: target.GetComponent<Player>().TakeDamage(skill.hpDamage);
        Debug.Log($"{enemyData.unityName}이(가) {target.name}을(를) 공격합니다.");*/

        if (cooldownTime <= 0)
        {
            int randomIndex = Random.Range(0, skills.Length); //랜덤으로 스킬 인덱스 선택
            PlayerSkill skill = skills[randomIndex]; //선택된 스킬 가져오기

            if (skill.isTrueActive == false) return; //스킬이 사용할 수 없으면 종료

            playerAttackBox[randomIndex].SkillDamage(skill); //스킬 피해량 설정(힐 포함)

            if (skill.playerSkillType == PlayerSkillType.Heal && playerRangeBox[randomIndex].isRange == true)
            {
                //Heal 스킬은 공격하지 않음
                Debug.Log($"{unitName} is healing instead of attacking.");
                playerRangeBox[randomIndex].CheckHealBuffer(skill); //플레이어의 힐 버퍼 체크
                playerAttackBox[randomIndex].SingleCheck(skill); //나의 힐 버퍼가 단일인지 멀티인지 확인

                animator.SetTrigger("Heal" + (randomIndex + 1)); //힐 애니메이션 트리거 설정
                StartCoroutine(WaitSkillDuration(skill)); //스킬 시전 및 지속 코루틴 시작
                StartCoroutine(WaitSkillCooldown(skill)); //스킬 쿨타임 대기 코루틴 시작
                cooldownTime = 3.0f; //공격의 쿨타임 설정
                return;
            }

            //playerRangeBox[randomIndex].ReachCheck(skill); // 플레이어의 원거리 체크

            if (playerRangeBox[randomIndex].isRange == true)
            {
                playerAttackBox[randomIndex].SingleCheck(skill); //나의 공격이 단일인지 멀티인지 확인하고 공격 실행
                animator.SetTrigger("Attack" + (randomIndex + 1)); //나의 공격 애니메이션 트리거 설정

                StartCoroutine(WaitSkillDuration(skill)); //스킬 시전 및 지속 코루틴 시작

                StartCoroutine(WaitSkillCooldown(skill)); //스킬 쿨타임 대기 코루틴 시작
                cooldownTime = 3.0f; //공격의 쿨타임 설정
            }
        }

    }

    /// <summary>
    /// 좌우 반전
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight; // 현재 방향 반전
        Vector3 scale = transform.localScale; //현재 오브젝트의 스케일 가져오기
        scale.x *= -1; //X축 반전
        transform.localScale = scale; //반전된 스케일 적용
    }


    /// <summary>
    /// 목적지를 향해 이동
    /// </summary>
    /// <param name="destination"></param>
    public void MoveTo(Vector2 destination)
    {
        moveTargetPosition = destination; //이동 목표 위치 설정
        isMoving = true; //이동 중 상태로 설정
        animator.SetBool("isMoving", true); //이동 애니메이션 활성화

        // ★ 방향 확인 및 Flip
        if (moveTargetPosition.x < transform.position.x && isFacingRight)
        {
            Flip(); // 왼쪽으로 회전
        }
        else if (moveTargetPosition.x > transform.position.x && !isFacingRight)
        {
            Flip(); // 오른쪽으로 회전
        }
    }

    /// <summary>
    /// 플레이어가 타겟(몹)에게 이동하는 함수
    /// </summary>
    public void MoveToTarget()
    {
        Vector2 currentPosition = transform.position; //현재 플레이어의 위치
        Vector2 targetPosition = target.transform.position; //타겟(몹)의 위치
        Vector2 direction = (targetPosition - currentPosition).normalized; //타겟 방향 벡터 계산
        float moveSpeed = playerData.moveSpeed * Time.deltaTime; //이동 속도 계산
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed); //현재 위치에서 타겟 위치로 이동

        /*animator.SetFloat("MoveX", direction.x); //애니메이션 이동 방향 설정
        animator.SetFloat("MoveY", direction.y); //애니메이션 이동 방향 설정*/

        // ★ 방향 확인 및 Flip
        if (targetPosition.x < currentPosition.x && isFacingRight)
        {
            Flip(); // 왼쪽으로 회전
        }
        else if (targetPosition.x > currentPosition.x && !isFacingRight)
        {
            Flip(); // 오른쪽으로 회전
        }

    }

    /// <summary>
    /// 스킬 사용 후 쿨타임 동안 대기하는 코루틴
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillCooldown(PlayerSkill skill)
    {
        //skill.isTrueActive = false; //스킬을 사용할 수 없도록 설정
        yield return new WaitForSeconds(skill.cooldownTime); //스킬 쿨타임 대기
        skill.isTrueActive = true; //스킬을 사용할 수 있도록 설정
    }

    /// <summary>
    /// 스킬 시전 및 지속 시간을 대기하는 코루틴
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillDuration(PlayerSkill skill)
    {
        duration = skill.castTime; //스킬의 시전 시간 설정
        durationSkill = true; // 스킬이 지속되는 상태로 설정
        if (duration > 0.0f)
        {
            casting = true; //스킬 시전 중 상태로 설정
            while (duration > 0.0f)
            {
                if (DefenseManager.instance.isTurning) yield break; //턴이 진행 중이면 코루틴 종료

                duration -= Time.deltaTime; //남은 시전 시간 감소
                yield return null; //다음 프레임까지 대기
            }
            casting = false; //스킬 시전 중 상태 해제
            animator.SetTrigger("Finish"); //캐스팅 완료 애니메이션 트리거 설정
        }

        duration = skill.durationTime;
        if (duration > 0.0f)
        {
            doing = true; //스킬 지속 중 상태로 설정
            while (duration > 0.0f)
            {
                if (DefenseManager.instance.isTurning) yield break; //턴이 진행 중이면 코루틴 종료
                duration -= Time.deltaTime; //남은 지속 시간 감소
                yield return null; //다음 프레임까지 대기
            }
            doing = false; //스킬 지속 중 상태 해제
            animator.SetTrigger("Finish"); //스킬 완료 애니메이션 트리거 설정
        }

        //여기서 스킬별 쿨타임 코루틴 실행해도 괜찮을 듯?
        durationSkill = false; // 스킬이 지속되는 상태로 설정
        yield return null; //코루틴 종료
    }

    /// <summary>
    /// 죽을 시 5초 후에 적 오브젝트를 비활성화하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForActiveFalse()
    {
        yield return new WaitForSeconds(5.0f); //5 초 대기
        this.gameObject.SetActive(false); //적 오브젝트 비활성화
        Dead();
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    /// <summary>
    /// 플레이어의 행동을 정의하는 함수
    /// </summary>
    public void Behavior()
    {
        // 플레이어의 행동을 정의하는 로직을 여기에 추가할 수 있습니다.
        // 예: 공격, 방어, 이동 등
        Debug.Log($"{unitName} is currently {behavior}.");

        if (isDead)
            behavior = "dead"; //죽었을 때 행동 상태 변경
        else if (casting)
            behavior = "casting"; //스킬 시전 중 행동 상태 변경
        else if (target != null)
            behavior = "attacking"; //타겟이 존재할 때
        else if (isMoving)
            behavior = "moving"; //이동 중 행동 상태 변경
        else
            behavior = "idle"; //기본 행동 상태 변경
    }

    public void Dead()
    {
        if (isDead)
        {
            gameObject.transform.position = new Vector3(-100, -100, -100); //죽었을 때 플레이어를 화면 밖으로 이동
        }
    }
}