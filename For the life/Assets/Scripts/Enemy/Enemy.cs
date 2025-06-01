using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("������")]
    public EnemyData enemyData; //ScriptableObject�� ���� ������ ����
    public EnemySkill[] skills; //���� ����ϴ� ��ų��

    [Header("����")]
    public EnemyAttackBox[] enemyAttackBox; //���� ���� �ڽ�
    public EnemyRangeBox[] enemyRangeBox; //���� ���� Ž�� �ڽ�
    public float cooldownTime = 0.0f; //���� ���� ��Ÿ��

    [Header("�ִϸ��̼�")]
    public Animator animator; //���� �ִϸ��̼� ��Ʈ�ѷ�

    public int currentHp; //���� HP
    public bool isDead = false; // �׾����� ����
    public bool isFacingRight = true; // ���� ��(�ڽ�)�� ���� (�������� �ٶ󺸰� �ִ��� ����)

    public float duration = 0.0f; // ���� ���� �ð�
    public bool casting = false; // ���� ��ų ���� ����
    public bool doing = false; // ���� ��ų ���� ����

    public GameObject target; //���� Ÿ��


    private void OnEnable()
    {
        animator = GetComponent<Animator>(); //���� �ִϸ��̼� ��Ʈ�ѷ��� ������

        currentHp = enemyData.maxHp; //���� ���� HP�� �ִ� HP�� �ʱ�ȭ
        skills = enemyData.skills; //���� ��ų�� �ʱ�ȭ
        target = null; //Ÿ�� �ʱ�ȭ
        for(int i = 0; i < skills.Length; i++)
        {
            if (skills[i] != null) //��ų�� ������
            {
                skills[i].isTrueActive = true; //��ų�� ����� �� �ִ� ���·� ����
            }
        }
    }

    private void Update()
    {
        if (isDead) return; //���� �׾����� �Լ� ����

        if (currentHp <= 0) //���� HP�� 0 �����̸�
        {
            isDead = true; //���� ���·� ����
            Debug.Log($"{enemyData.unityName} has died."); //���� �޽��� ���
            animator.SetTrigger("Death"); //���� �ִϸ��̼� Ʈ���� ����
            StartCoroutine(WaitForActiveFalse()); //5�� �Ŀ� �� ������Ʈ ��Ȱ��ȭ
            return; //�Լ� ����
        }

        if (casting || doing) return; //��ų ���� ���̰ų� ���� ���̸� �Լ� ����

        if (DefenseManager.instance.isTurning) //���� ���� ���� ��
        {
            animator.SetBool("isMoving", false); // �̵��� �ִϸ��̼� ����
            duration = 0.0f; // �����ϰ� �ִ� ��ų ����
            target = FindClosestTarget(); //Ÿ���� ã�� �Լ� ȣ��
        }
        else if (!DefenseManager.instance.isTurning) //�׼��� �������� ��
        {
            if (target.activeSelf == false || target == null) //Ÿ���� ������
            {
                target = FindClosestTarget(); //���� ����� �÷��̾ ã��
            }
            bool inRange = false; //Ÿ���� ������ �ִ��� ����
            for (int i = 0; i < enemyRangeBox.Length; i++) //��� ���� �ڽ��� ��ȸ�ϸ鼭
            {
                enemyRangeBox[i].ReachCheck(skills[i]); // ���� ���Ÿ� üũ
                if (enemyRangeBox[i].isRange == true) //������ �����ϸ�
                {
                    inRange = true; //Ÿ���� ������ ����
                    break; //���� ����
                }
            }

            if (inRange)
            {
                AttackTrigger(); //Ÿ�� ������ �÷��̾ ������ ���� �Լ� ȣ��
                animator.SetBool("isMoving", false); // �̵� �� �ִϸ��̼� ����
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

            cooldownTime -= Time.deltaTime; //��Ÿ�� ����
        }
    }

    /// <summary>
    ///���� ����� �÷��̾ ã�� �Լ�
    /// </summary>
    /// <returns></returns>
    GameObject FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //�÷��̾� �±׸� ���� ��� ���� ������Ʈ�� ã��
        GameObject closest = null; //���� ����� �÷��̾ ������ ����
        float closestDistance = Mathf.Infinity; //���Ѵ�� �ʱ�ȭ�Ͽ� ���� ����� �Ÿ��� ã�� ����

        foreach (var player in players) //��� �÷��̾ ��ȸ�ϸ鼭
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); //���� ���� �÷��̾� ���� �Ÿ� ���
            if (distance < closestDistance) //���� ����� �Ÿ��� ã�� ���� ����
            {
                closestDistance = distance;
                closest = player;
            }
        }

        return closest; //���� ����� �÷��̾ ��ȯ
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
            EnemySkill skill = skills[randomIndex]; //���õ� ��ų ��������

            if (skill.isTrueActive == false) return; //��ų�� ����� �� ������ ����

            enemyRangeBox[randomIndex].ReachCheck(skill); // ���� ���Ÿ� üũ

            if (enemyRangeBox[randomIndex].isRange == true)
            {
                enemyAttackBox[randomIndex].SingleCheck(skill); //���� ������ �������� ��Ƽ���� Ȯ���ϰ� ���� ����
                animator.SetTrigger("Attack" + (randomIndex + 1)); //���� ���� �ִϸ��̼� Ʈ���� ����

                StartCoroutine(WaitSkillDuration(skill)); //��ų ���� �� ���� �ڷ�ƾ ����

                StartCoroutine(WaitSkillCooldown(skill)); //��ų ��Ÿ�� ��� �ڷ�ƾ ����
                cooldownTime = 3.0f; //������ ��Ÿ�� ����
            }
        }

    }

    /// <summary>
    /// ���� Ÿ��(�÷��̾�)���� �̵��ϴ� �Լ�
    /// </summary>
    public void MoveToTarget()
    {
        Vector2 currentPosition = transform.position; //���� ��(�ڽ�)�� ��ġ
        Vector2 targetPosition = target.transform.position; //Ÿ��(�÷��̾�)�� ��ġ
        Vector2 direction = (targetPosition - currentPosition).normalized; //Ÿ�� ���� ���� ���
        float moveSpeed = enemyData.moveSpeed * Time.deltaTime; //�̵� �ӵ� ���
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
    /// ���� �ٶ󺸴� ������ ������Ű�� �Լ�
    /// </summary>
    public void Flip()
    {
        isFacingRight = !isFacingRight; // ���� ���� ����
        Vector3 scale = transform.localScale; //���� ������Ʈ�� ������ ��������
        scale.x *= -1; //X�� ����
        transform.localScale = scale; //������ ������ ����
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

    /// <summary>
    /// ��ų ��� �� ��Ÿ�� ���� ����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator WaitSkillCooldown(EnemySkill skill)
    {
        skill.isTrueActive = false; //��ų ��� �Ұ� ���·� ����
        yield return new WaitForSeconds(skill.cooldownTime); //��ų�� ��Ÿ�� ���� ���
        skill.isTrueActive = true; //��ų ��� ���� ���·� ����
    }

    IEnumerator WaitSkillDuration(EnemySkill skill)
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
        yield return null; //�ڷ�ƾ ����
    }
}
