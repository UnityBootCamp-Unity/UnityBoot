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

    [Header("�÷��̾� ����")]
    public string unitName; // �÷��̾��� �̸�
    public int maxHp; // �ִ� HP
    public int currentHp; //���� HP
    public int maxMental; // �ִ� ���ŷ�
    public int currentMental; // ���� ���ŷ�
    public string behavior; //�÷��̾��� �ൿ (��: ����, ��� ��)
    public bool isDead = false; //�׾����� ����
    
    /*public int EquipmentLevel; //�÷��̾��� ��� ����
    public int attackPower; //�÷��̾��� ���ݷ�*/

    public bool isFacingRight = false; //���� �÷��̾�(�ڽ�)�� ���� (�������� �ٶ󺸰� �ִ��� ����)
    public bool isSelected = false; //�÷��̾ ���õǾ����� ����

    private Vector3 moveTargetPosition; //�÷��̾��� �̵� ��ǥ ��ġ
    private bool isMoving = false; //�÷��̾ �̵� ������ ����

    public float duration = 0.0f; //�÷��̾��� ���� �ð�
    public bool casting = false; //�÷��̾��� ��ų ���� ����
    public bool doing = false; //�÷��̾��� ��ų ���� ����
    public bool durationSkill; // ���� ��ų�� ���ӵǴ��� ����

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

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (playerData == null) //�÷��̾� �����Ͱ� �������� �ʾ�����
        {
            Debug.LogError("PlayerData is not assigned! Please assign a PlayerData ScriptableObject to the Player component.");
        }
        else
        {
            unitName = playerData.unityName; //�÷��̾��� �̸� ����
            maxHp = playerData.maxHp; //�÷��̾��� �ִ� HP ����
            currentHp = maxHp; //�÷��̾��� ���� HP�� �ִ� HP�� �ʱ�ȭ
            maxMental = playerData.maxMental; //�÷��̾��� �ִ� ���ŷ� ����
            currentMental = maxMental; //�÷��̾��� ���� ���ŷ��� �ִ� ���ŷ����� �ʱ�ȭ
            behavior = "Idle"; //�÷��̾��� �ʱ� �ൿ ���� ����

        }
    }

    private void Update()
    {
        if (isDead) return; //�÷��̾ �׾����� �Լ� ����

        if (currentHp > maxHp) //���� HP�� �ִ� HP���� ũ��
        {
            currentHp = maxHp; //���� HP�� �ִ� HP�� ����
        }

        if (currentHp <= 0)
        {
            isDead = true; //���� ���·� ����
            Debug.Log($"{unitName} has died.");
            animator.SetTrigger("Death"); //���� �ִϸ��̼� Ʈ���� ����
            StartCoroutine(WaitForActiveFalse()); //5�� �Ŀ� �÷��̾� ������Ʈ ��Ȱ��ȭ
            return; //�Լ� ����

        }

        if (target != null)
        {
            Enemy enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent == null || enemyComponent.currentHp <= 0 || !target.activeSelf)
            {
                target = null; // �׾��ų� ��Ȱ��ȭ�Ǹ� Ÿ�� �ʱ�ȭ
            }
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
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript == null || enemyScript.currentHp <= 0) //���� ���ų� HP�� 0 ������ ��� ����
                continue;

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
    /// ü���� ���� ���� ���� ã�� �Լ�
    /// �ڵ� �ϼ����� ���� Ȯ�� ����
    /// </summary>
    /// <returns></returns>
    GameObject FindLessHpTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //�� ������Ʈ�� ã��
        GameObject lessHpEnemy = null; //ü���� ���� ���� �� �ʱ�ȭ
        int lessHp = int.MaxValue; //���� ���� ü�� �ʱ�ȭ
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>(); //�� ������Ʈ ��������
            if (enemyComponent != null && enemyComponent.currentHp < lessHp) //���� �����ϰ� ���� ü���� ���� ������
            {
                lessHp = enemyComponent.currentHp; //���� ���� ü�� ������Ʈ
                lessHpEnemy = enemy; //ü���� ���� ���� �� ������Ʈ
            }
        }
        return lessHpEnemy; //ü���� ���� ���� �� ��ȯ
    }

    GameObject FindLessHpPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //�÷��̾� ������Ʈ�� ã��
        GameObject lessHpPlayer = null; //ü���� ���� ���� �÷��̾� �ʱ�ȭ
        int lessHp = int.MaxValue; //���� ���� ü�� �ʱ�ȭ
        foreach (GameObject player in players)
        {
            Player playerComponent = player.GetComponent<Player>(); //�÷��̾� ������Ʈ ��������
            if (playerComponent != null && playerComponent.currentHp < lessHp) //�÷��̾ �����ϰ� ���� ü���� ���� ������
            {
                lessHp = playerComponent.currentHp; //���� ���� ü�� ������Ʈ
                lessHpPlayer = player; //ü���� ���� ���� �÷��̾� ������Ʈ
            }
        }
        return lessHpPlayer; //ü���� ���� ���� �÷��̾� ��ȯ
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

            playerAttackBox[randomIndex].SkillDamage(skill); //��ų ���ط� ����(�� ����)

            if (skill.playerSkillType == PlayerSkillType.Heal && playerRangeBox[randomIndex].isRange == true)
            {
                //Heal ��ų�� �������� ����
                Debug.Log($"{unitName} is healing instead of attacking.");
                playerRangeBox[randomIndex].CheckHealBuffer(skill); //�÷��̾��� �� ���� üũ
                playerAttackBox[randomIndex].SingleCheck(skill); //���� �� ���۰� �������� ��Ƽ���� Ȯ��

                animator.SetTrigger("Heal" + (randomIndex + 1)); //�� �ִϸ��̼� Ʈ���� ����
                StartCoroutine(WaitSkillDuration(skill)); //��ų ���� �� ���� �ڷ�ƾ ����
                StartCoroutine(WaitSkillCooldown(skill)); //��ų ��Ÿ�� ��� �ڷ�ƾ ����
                cooldownTime = 3.0f; //������ ��Ÿ�� ����
                return;
            }

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
        //skill.isTrueActive = false; //��ų�� ����� �� ������ ����
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
        durationSkill = true; // ��ų�� ���ӵǴ� ���·� ����
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
        durationSkill = false; // ��ų�� ���ӵǴ� ���·� ����
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
        Dead();
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    /// <summary>
    /// �÷��̾��� �ൿ�� �����ϴ� �Լ�
    /// </summary>
    public void Behavior()
    {
        // �÷��̾��� �ൿ�� �����ϴ� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
        // ��: ����, ���, �̵� ��
        Debug.Log($"{unitName} is currently {behavior}.");

        if (isDead)
            behavior = "dead"; //�׾��� �� �ൿ ���� ����
        else if (casting)
            behavior = "casting"; //��ų ���� �� �ൿ ���� ����
        else if (target != null)
            behavior = "attacking"; //Ÿ���� ������ ��
        else if (isMoving)
            behavior = "moving"; //�̵� �� �ൿ ���� ����
        else
            behavior = "idle"; //�⺻ �ൿ ���� ����
    }

    public void Dead()
    {
        if (isDead)
        {
            gameObject.transform.position = new Vector3(-100, -100, -100); //�׾��� �� �÷��̾ ȭ�� ������ �̵�
        }
    }
}