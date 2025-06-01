using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("데이터")]
    public EnemyData enemyData; //ScriptableObject로 적의 데이터 관리
    public EnemySkill[] skills; //적이 사용하는 스킬들

    [Header("공격")]
    public EnemyAttackBox[] enemyAttackBox; //적의 공격 박스
    public EnemyRangeBox[] enemyRangeBox; //적의 범위 탐지 박스
    public float cooldownTime = 0.0f; //적의 공격 쿨타임

    [Header("애니메이션")]
    public Animator animator; //적의 애니메이션 컨트롤러

    public int currentHp; //현재 HP
    public bool isDead = false; // 죽었는지 여부
    public bool isFacingRight = true; // 현재 적(자신)의 방향 (오른쪽을 바라보고 있는지 여부)

    public float duration = 0.0f; // 적의 지속 시간
    public bool casting = false; // 적의 스킬 시전 여부
    public bool doing = false; // 적의 스킬 지속 여부

    public GameObject target; //적의 타겟


    private void OnEnable()
    {
        animator = GetComponent<Animator>(); //적의 애니메이션 컨트롤러를 가져옴

        currentHp = enemyData.maxHp; //적의 현재 HP를 최대 HP로 초기화
        skills = enemyData.skills; //적의 스킬을 초기화
        target = null; //타겟 초기화
        for(int i = 0; i < skills.Length; i++)
        {
            if (skills[i] != null) //스킬이 있으면
            {
                skills[i].isTrueActive = true; //스킬을 사용할 수 있는 상태로 설정
            }
        }
    }

    private void Update()
    {
        if (isDead) return; //적이 죽었으면 함수 종료

        if (currentHp <= 0) //현재 HP가 0 이하이면
        {
            isDead = true; //죽음 상태로 설정
            Debug.Log($"{enemyData.unityName} has died."); //죽음 메시지 출력
            animator.SetTrigger("Death"); //죽음 애니메이션 트리거 설정
            StartCoroutine(WaitForActiveFalse()); //5초 후에 적 오브젝트 비활성화
            return; //함수 종료
        }

        if (casting || doing) return; //스킬 시전 중이거나 지속 중이면 함수 종료

        if (DefenseManager.instance.isTurning) //턴이 진행 중일 때
        {
            animator.SetBool("isMoving", false); // 이동중 애니메이션 해제
            duration = 0.0f; // 지속하고 있는 스킬 중지
            target = FindClosestTarget(); //타겟을 찾는 함수 호출
        }
        else if (!DefenseManager.instance.isTurning) //액션이 진행중일 때
        {
            if (target.activeSelf == false || target == null) //타겟이 없으면
            {
                target = FindClosestTarget(); //가장 가까운 플레이어를 찾음
            }
            bool inRange = false; //타겟이 범위에 있는지 여부
            for (int i = 0; i < enemyRangeBox.Length; i++) //모든 범위 박스를 순회하면서
            {
                enemyRangeBox[i].ReachCheck(skills[i]); // 적의 원거리 체크
                if (enemyRangeBox[i].isRange == true) //범위가 존재하면
                {
                    inRange = true; //타겟이 범위에 있음
                    break; //루프 종료
                }
            }

            if (inRange)
            {
                AttackTrigger(); //타겟 범위에 플레이어가 있으면 공격 함수 호출
                animator.SetBool("isMoving", false); // 이동 중 애니메이션 해제
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

            cooldownTime -= Time.deltaTime; //쿨타임 감소
        }
    }

    /// <summary>
    ///가장 가까운 플레이어를 찾는 함수
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //플레이어 태그를 가진 모든 게임 오브젝트를 찾음
        GameObject closest = null; //가장 가까운 플레이어를 저장할 변수
        float closestDistance = Mathf.Infinity; //무한대로 초기화하여 가장 가까운 거리를 찾기 위함

        foreach (var player in players) //모든 플레이어를 순회하면서
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); //현재 적과 플레이어 간의 거리 계산
            if (distance < closestDistance) //가장 가까운 거리를 찾기 위한 조건
            {
                closestDistance = distance;
                closest = player;
            }
        }

        return closest; //가장 가까운 플레이어를 반환
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
            EnemySkill skill = skills[randomIndex]; //선택된 스킬 가져오기

            if (skill.isTrueActive == false) return; //스킬이 사용할 수 없으면 종료

            enemyRangeBox[randomIndex].ReachCheck(skill); // 적의 원거리 체크

            if (enemyRangeBox[randomIndex].isRange == true)
            {
                enemyAttackBox[randomIndex].SingleCheck(skill); //적의 공격이 단일인지 멀티인지 확인하고 공격 실행
                animator.SetTrigger("Attack" + (randomIndex + 1)); //적의 공격 애니메이션 트리거 설정

                StartCoroutine(WaitSkillDuration(skill)); //스킬 시전 및 지속 코루틴 시작

                StartCoroutine(WaitSkillCooldown(skill)); //스킬 쿨타임 대기 코루틴 시작
                cooldownTime = 3.0f; //공격의 쿨타임 설정
            }
        }

    }

    /// <summary>
    /// 적이 타겟(플레이어)에게 이동하는 함수
    /// </summary>
    public void MoveToTarget()
    {
        Vector2 currentPosition = transform.position; //현재 몹(자신)의 위치
        Vector2 targetPosition = target.transform.position; //타겟(플레이어)의 위치
        Vector2 direction = (targetPosition - currentPosition).normalized; //타겟 방향 벡터 계산
        float moveSpeed = enemyData.moveSpeed * Time.deltaTime; //이동 속도 계산
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
    /// 적이 바라보는 방향을 반전시키는 함수
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight; // 현재 방향 반전
        Vector3 scale = transform.localScale; //현재 오브젝트의 스케일 가져오기
        scale.x *= -1; //X축 반전
        transform.localScale = scale; //반전된 스케일 적용
    }

    /// <summary>
    /// 죽을 시 5초 후에 적 오브젝트를 비활성화하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForActiveFalse()
    {
        yield return new WaitForSeconds(5.0f); //5 초 대기
        this.gameObject.SetActive(false); //적 오브젝트 비활성화
    }

    /// <summary>
    /// 스킬 사용 후 쿨타임 동안 대기하는 코루틴
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillCooldown(EnemySkill skill)
    {
        skill.isTrueActive = false; //스킬 사용 불가 상태로 설정
        yield return new WaitForSeconds(skill.cooldownTime); //스킬의 쿨타임 동안 대기
        skill.isTrueActive = true; //스킬 사용 가능 상태로 설정
    }

    IEnumerator WaitSkillDuration(EnemySkill skill)
    {
        duration = skill.castTime; //스킬의 시전 시간 설정
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
        yield return null; //코루틴 종료
    }
}
