using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// Ÿ���� ������� �Ծ����� ���⼭ �浹 Ȯ�� ���ϰ� �ϱ�(Enable �� �ٽ� false�� �ʱ�ȭ)
public class EnemyAttackBox : MonoBehaviour
{
    
    private Enemy enemy; //�� ��ũ��Ʈ ����
    public bool isSingleAttack = false; // �̱� ���� ����

    [Header("��ų ����")]
    public int hpDamage; // ���� ��ų�� �ִ� ���ط�
    public int mentalDamage; // ���� ��ų�� �ִ� ���ŷ� ���ط�
    public bool durationSkill; // ���� ��ų�� ���ӵǴ��� ����
    public EnemySkillType skillType; //���� ��ų Ÿ�� (���� ����, ���� ���� ��)

    public bool successAttack = false; // ���� ���� ����

    [SerializeField] private Collider2D hitbox;

    private bool prevEnabled = false;


    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); //�θ� ������Ʈ���� Enemy ��ũ��Ʈ�� ã��
    }


    /// <summary>
    /// ���� ���� ������ ���� �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    /// ���� �� �÷��̾��� HP ����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (successAttack) // ������ ������ ������ ���� �浹 ó������ ����
            return;

        if (isSingleAttack)
        {
            if (other.gameObject == enemy.target)
            {
                Debug.Log($"EnemyAttackBox: Player in attack range!"); //����� �޽��� ���
                Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��

                if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                    return;

                if (player != null) //�÷��̾ �����ϸ�
                {
                    if (skillType == EnemySkillType.Heal)
                    {
                    }
                    else
                    {
                        TakeDamage(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    }
                }
                successAttack = true; // ���� ���� ���·� ����
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Player")) // �÷��̾� �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Debug.Log("EnemyAttackBox: Player in multi-attack range!"); // ����� �޽��� ���
                Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��

                if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                    return;

                if (player != null) //�÷��̾ �����ϸ�
                {
                    if (skillType == EnemySkillType.Heal)
                    {
                    }
                    else
                    {
                        TakeDamage(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    }
                }
                successAttack = true; // ���� ���� ���·� ����
            }
        }
    }

    /// <summary>
    ///  ���� ������ �������� ��Ƽ���� Ȯ���ϰ� �ش� ������ �����մϴ�.
    /// </summary>
    /// <param name="skill"></param>
    public void SingleCheck(EnemySkill skill)
    {
        if(skill.enemySkillType == EnemySkillType.SingleAttack)
        {
            isSingleAttack = true; // �̱� ���� ���θ� true�� ����
        }
        else if(skill.enemySkillType == EnemySkillType.MultiAttack)
        {
            isSingleAttack = false; // ��Ƽ ���� ���θ� false�� ����
        }
    }

    public void SkillDamage(EnemySkill skill)
    {
        hpDamage = skill.hpDamage; //��ų�� HP ���ط� ����
        mentalDamage = skill.mentalDamage; //��ų�� ���ŷ� ���ط� ����
        skillType = skill.enemySkillType; //��ų Ÿ�� ����
    }

    public void TakeDamage(Player player, int hpDamage, int mentalDamage)
    {
        player.currentHp -= hpDamage; // �÷��̾��� HP ����
        player.currentMental -= mentalDamage; // �÷��̾��� ���ŷ� ����
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǿ� �ݶ��̴��� Ȱ��ȭ ���¸� Ȯ���մϴ�.
    /// </summary>
    void Update()
    {
        if (hitbox != null)
        {
            if (hitbox.enabled && !prevEnabled)
            {
                // �ݶ��̴��� ��� ������ ��
                OnColliderEnabled();
            }

            prevEnabled = hitbox.enabled;
        }
    }

    /// <summary>
    /// �ݶ��̴��� Ȱ��ȭ�Ǿ��� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// �ݶ��̴� Ȱ��ȭ�� �̺�Ʈ�Լ��� ���� �� ����
    /// �ִϸ��̼ǿ� �����Ű�� ��ĵ� ����
    /// </summary>
    private void OnColliderEnabled()
    {
        successAttack = false;
        isSingleAttack = false;
        Debug.Log("Collider enabled �� ���� ���� �ʱ�ȭ��");
    }

}
