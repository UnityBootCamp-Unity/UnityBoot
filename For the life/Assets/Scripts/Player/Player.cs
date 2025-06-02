using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("������")]
    public PlayerData playerData; //ScriptableObject�� �÷��̾��� ������ ����
    public PlayerSkill[] skills; //�÷��̾ ����ϴ� ��ų��

    [Header("����")]
    public PlayerAttackBox[] playerAttackBox; //�÷��̾��� ���� �ڽ�
    public PlayerRangeBox[] playerRangeBox; //�÷��̾��� ���� Ž�� �ڽ�
    public float cooldownTime = 0.0f; //�÷��̾��� ���� ��Ÿ��

    [Header("�ִϸ��̼�")]
    public Animator animator; //�÷��̾��� �ִϸ��̼� ��Ʈ�ѷ�

    public int currentHp; //���� HP
    public bool isDead = false; //�׾����� ����
    public bool isFacingRight = true; //���� �÷��̾�(�ڽ�)�� ���� (�������� �ٶ󺸰� �ִ��� ����)
    public bool isSelected = false; //�÷��̾ ���õǾ����� ����

    private Vector3 moveTargetPosition; //�÷��̾��� �̵� ��ǥ ��ġ
    private bool isMoving = false; //�÷��̾ �̵� ������ ����

    public float duration = 0.0f; //�÷��̾��� ���� �ð�
    public bool casting = false; //�÷��̾��� ��ų ���� ����
    public bool doing = false; //�÷��̾��� ��ų ���� ����

    public GameObject target; //�÷��̾��� Ÿ��

    private void OnEnable()
    {
        animator = GetComponent<Animator>(); //�÷��̾��� �ִϸ��̼� ��Ʈ�ѷ��� ������

        currentHp = playerData.maxHp; //�÷��̾��� ���� HP�� �ִ� HP�� �ʱ�ȭ
        isDead = false; //�÷��̾��� ���� ���� �ʱ�ȭ
        skills = playerData.skills; //�÷��̾��� ��ų�� �ʱ�ȭ
        target = null; //Ÿ�� �ʱ�ȭ
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] != null) //��ų�� ������
            {
                skills[i].isTrueActive = true; //��ų�� ����� �� �ִ� ���·� ����
            }
        }
    }

    private void Update()
    {
        if (isDead) return; //�÷��̾ �׾����� �Լ� ����

        if (currentHp <= 0) //���� HP�� 0 �����̸�
        {
            isDead = true; //���� ���·� ����
            Debug.Log($"{playerData.unityName} has died."); //���� �޽��� ���
            animator.SetTrigger("Death"); //���� �ִϸ��̼� Ʈ���� ����
            StartCoroutine(WaitForActiveFalse()); //5�� �Ŀ� �÷��̾� ������Ʈ ��Ȱ��ȭ
            return; //�Լ� ����
        }

        if (DefenseManager.instance.isTurning) //���� ���� ���� ��
        {
            animator.SetBool("isMoving", false); // �̵��� �ִϸ��̼� ����
            duration = 0.0f; // �����ϰ� �ִ� ��ų ����
            return; //�Լ� ����
        }
        else if (!DefenseManager.instance.isTurning) //���� ���� ������ ���� ��
        {
            /*if (target == null || target.activeSelf == false) //Ÿ���� ������
            {
                target = FindClosestTarget(); //Ÿ���� ã�� �Լ� ȣ��
            }*/
            /*bool inRange = false; //Ÿ�� ���� ���� �ʱ�ȭ
            for (int i = 0; i < playerRangeBox.Length; i++)
            {
                if (playerRangeBox[i].isRange == true) //�÷��̾��� ���� �ڽ��� Ȱ��ȭ�Ǿ� ������
                {
                    inRange = true; //Ÿ���� ������ ����
                    break; //�ݺ��� ����
                }
            }*/

            if (casting || doing) return; //��ų ���� ���̰ų� ���� ���̸� �Լ� ����

            if (isMoving)
            {
                animator.SetBool("isMoving", true); //�̵� �� �ִϸ��̼� Ȱ��ȭ
                transform.position = Vector2.MoveTowards(transform.position, moveTargetPosition,
                    playerData.moveSpeed * Time.deltaTime); //�÷��̾ �̵� ��ǥ ��ġ�� �̵�

                if (Vector2.Distance(transform.position, moveTargetPosition) < 0.05f) //��ǥ ��ġ�� �������� ��
                {
                    isMoving = false; //�̵� �� ���� ����
                    animator.SetBool("isMoving", false); //�̵� �ִϸ��̼� ��Ȱ��ȭ
                }
            }
            else
            {
                if (target != null && target.activeSelf) //Ÿ���� �����ϰ� Ȱ��ȭ�Ǿ� ������
                {
                    bool inRange = false; //Ÿ�� ���� ���� �ʱ�ȭ
                    for (int i = 0; i < playerRangeBox.Length; i++) //��� ���� �ڽ��� ��ȸ�ϸ鼭
                    {
                        //playerRangeBox[i].ReachCheck(skills[i]); //�÷��̾��� ���Ÿ� üũ
                        if (playerRangeBox[i].isRange == true) //������ �����ϸ�
                        {
                            inRange = true; //Ÿ���� ������ ����
                            break; //���� ����
                        }
                    }
                    if (inRange)
                    {
                        AttackTrigger(); //Ÿ�� ������ ���� ������ ���� �Լ� ȣ��
                        animator.SetBool("isMoving", false); //�̵� �� �ִϸ��̼� ����
                    }
                    else if (!inRange)
                    {
                        animator.SetBool("isMoving", true); //�̵� �� �ִϸ��̼� ����
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); //���� �ִϸ��̼� ���� ���� ��������
                        if (stateInfo.IsName("Walk")) //���� �ִϸ��̼��� �ȱ� ������ ��
                        {
                            MoveToTarget(); //Ÿ������ �̵��ϴ� �Լ� ȣ��
                        }
                    }
                }
                else
                {
                    bool inRange = false; //Ÿ�� ���� ���� �ʱ�ȭ
                    for (int i = 0; i < playerRangeBox.Length; i++) //��� ���� �ڽ��� ��ȸ�ϸ鼭
                    {
                        if (playerRangeBox[i].isRange == true) //������ �����ϸ�
                        {
                            inRange = true; //Ÿ���� ������ ����
                            break; //���� ����
                        }
                    }
                    if (inRange)
                    {
                        target = FindClosestTarget(); //Ÿ�� ������ �÷��̾ ������ ���� ����� ���� ã��
                        AttackTrigger(); //Ÿ�� ������ ���� ������ ���� �Լ� ȣ��
                        target = null; //Ÿ�� �ʱ�ȭ
                    }
                }
            }
            cooldownTime -= Time.deltaTime; //��Ÿ�� ����

        }
    }


    /// <summary>
    /// ���� ����� ���� ã�� �Լ�
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //�� ������Ʈ�� ã��
        GameObject closestEnemy = null; //���� ����� �� �ʱ�ȭ
        float closestDistance = Mathf.Infinity; //���� ����� �Ÿ� �ʱ�ȭ
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); //�÷��̾�� �� ������ �Ÿ� ���
            if (distance < closestDistance) //���� ����� ���� ã��
            {
                closestDistance = distance; //���� ����� �Ÿ� ������Ʈ
                closestEnemy = enemy; //���� ����� �� ������Ʈ
            }
        }

        return closestEnemy; //���� ����� �� ��ȯ
    }

    /// <summary>
    /// ��ų �������� �������� �����ؼ� ���� ������ ���� ��� �����Ѵ�.
    /// </summary>
    public void AttackTrigger()
    {
        /*if (target == null) return; //Ÿ���� ������ �Լ� ����
        //���� ���� ������ ���⿡ �߰�
        //��: target.GetComponent<Player>().TakeDamage(skill.hpDamage);
        Debug.Log($"{enemyData.unityName}��(��) {target.name}��(��) �����մϴ�.");*/

        if (cooldownTime <= 0)
        {
            int randomIndex = Random.Range(0, skills.Length); //�������� ��ų �ε��� ����
            PlayerSkill skill = skills[randomIndex]; //���õ� ��ų ��������

            if (skill.isTrueActive == false) return; //��ų�� ����� �� ������ ����

            //playerRangeBox[randomIndex].ReachCheck(skill); // �÷��̾��� ���Ÿ� üũ

            if (playerRangeBox[randomIndex].isRange == true)
            {
                playerAttackBox[randomIndex].SingleCheck(skill); //���� ������ �������� ��Ƽ���� Ȯ���ϰ� ���� ����
                animator.SetTrigger("Attack" + (randomIndex + 1)); //���� ���� �ִϸ��̼� Ʈ���� ����

                StartCoroutine(WaitSkillDuration(skill)); //��ų ���� �� ���� �ڷ�ƾ ����

                StartCoroutine(WaitSkillCooldown(skill)); //��ų ��Ÿ�� ��� �ڷ�ƾ ����
                cooldownTime = 3.0f; //������ ��Ÿ�� ����
            }
        }

    }

    /// <summary>
    /// �¿� ����
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight; // ���� ���� ����
        Vector3 scale = transform.localScale; //���� ������Ʈ�� ������ ��������
        scale.x *= -1; //X�� ����
        transform.localScale = scale; //������ ������ ����
    }


    /// <summary>
    /// �������� ���� �̵�
    /// </summary>
    /// <param name="destination"></param>
    public void MoveTo(Vector2 destination)
    {
        moveTargetPosition = destination; //�̵� ��ǥ ��ġ ����
        isMoving = true; //�̵� �� ���·� ����
        animator.SetBool("isMoving", true); //�̵� �ִϸ��̼� Ȱ��ȭ

        // �� ���� Ȯ�� �� Flip
        if (moveTargetPosition.x < transform.position.x && isFacingRight)
        {
            Flip(); // �������� ȸ��
        }
        else if (moveTargetPosition.x > transform.position.x && !isFacingRight)
        {
            Flip(); // ���������� ȸ��
        }
    }

    /// <summary>
    /// �÷��̾ Ÿ��(��)���� �̵��ϴ� �Լ�
    /// </summary>
    public void MoveToTarget()
    {
        Vector2 currentPosition = transform.position; //���� �÷��̾��� ��ġ
        Vector2 targetPosition = target.transform.position; //Ÿ��(��)�� ��ġ
        Vector2 direction = (targetPosition - currentPosition).normalized; //Ÿ�� ���� ���� ���
        float moveSpeed = playerData.moveSpeed * Time.deltaTime; //�̵� �ӵ� ���
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed); //���� ��ġ���� Ÿ�� ��ġ�� �̵�

        /*animator.SetFloat("MoveX", direction.x); //�ִϸ��̼� �̵� ���� ����
        animator.SetFloat("MoveY", direction.y); //�ִϸ��̼� �̵� ���� ����*/

        // �� ���� Ȯ�� �� Flip
        if (targetPosition.x < currentPosition.x && isFacingRight)
        {
            Flip(); // �������� ȸ��
        }
        else if (targetPosition.x > currentPosition.x && !isFacingRight)
        {
            Flip(); // ���������� ȸ��
        }

    }

    /// <summary>
    /// ��ų ��� �� ��Ÿ�� ���� ����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillCooldown(PlayerSkill skill)
    {
        skill.isTrueActive = false; //��ų�� ����� �� ������ ����
        yield return new WaitForSeconds(skill.cooldownTime); //��ų ��Ÿ�� ���
        skill.isTrueActive = true; //��ų�� ����� �� �ֵ��� ����
    }

    /// <summary>
    /// ��ų ���� �� ���� �ð��� ����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillDuration(PlayerSkill skill)
    {
        duration = skill.castTime; //��ų�� ���� �ð� ����
        if (duration > 0.0f)
        {
            casting = true; //��ų ���� �� ���·� ����
            while (duration > 0.0f)
            {
                if (DefenseManager.instance.isTurning) yield break; //���� ���� ���̸� �ڷ�ƾ ����

                duration -= Time.deltaTime; //���� ���� �ð� ����
                yield return null; //���� �����ӱ��� ���
            }
            casting = false; //��ų ���� �� ���� ����
            animator.SetTrigger("Finish"); //ĳ���� �Ϸ� �ִϸ��̼� Ʈ���� ����
        }

        duration = skill.durationTime;
        if (duration > 0.0f)
        {
            doing = true; //��ų ���� �� ���·� ����
            while (duration > 0.0f)
            {
                if (DefenseManager.instance.isTurning) yield break; //���� ���� ���̸� �ڷ�ƾ ����
                duration -= Time.deltaTime; //���� ���� �ð� ����
                yield return null; //���� �����ӱ��� ���
            }
            doing = false; //��ų ���� �� ���� ����
            animator.SetTrigger("Finish"); //��ų �Ϸ� �ִϸ��̼� Ʈ���� ����
        }

        //���⼭ ��ų�� ��Ÿ�� �ڷ�ƾ �����ص� ������ ��?

        yield return null; //�ڷ�ƾ ����
    }

    /// <summary>
    /// ���� �� 5�� �Ŀ� �� ������Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForActiveFalse()
    {
        yield return new WaitForSeconds(5.0f); //5 �� ���
        this.gameObject.SetActive(false); //�� ������Ʈ ��Ȱ��ȭ
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}