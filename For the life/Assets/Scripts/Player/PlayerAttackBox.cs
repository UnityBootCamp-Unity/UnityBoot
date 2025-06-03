using System.Collections.Generic;
using UnityEngine;

// Ÿ���� ������� �Ծ����� ���⼭ �浹 Ȯ�� ���ϰ� �ϱ�(Enable �� �ٽ� false�� �ʱ�ȭ)
public class PlayerAttackBox : MonoBehaviour
{
    private Player player; //�÷��̾� ��ũ��Ʈ ����
    private List<Player> playersInRange = new List<Player>();  //���� �� �÷��̾� ����Ʈ
    public bool isSingleAttack = false; // �̱� ���� ����

    [Header("��ų ����")]
    public int hpDamage; // ���� ��ų�� �ִ� ���ط�
    public int mentalDamage; // ���� ��ų�� �ִ� ���ŷ� ���ط�
    public bool durationSkill; // ���� ��ų�� ���ӵǴ��� ����
    public PlayerSkillType skillType; //���� ��ų Ÿ�� (���� ����, ���� ���� ��)

    public bool successAttack = false; // ���� ���� ����

    [SerializeField] private Collider2D hitbox;

    private bool prevEnabled = false;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //�θ� ������Ʈ���� Player ��ũ��Ʈ�� ã��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (successAttack) // ������ ������ ������ ���� �浹 ó������ ����
            return;

        if (isSingleAttack)
        {
            if (skillType == PlayerSkillType.Heal)
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Player in H1 range!"); //����� �޽��� ���
                    Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��
                    if(player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // �÷��̾��� HP�� �ִ�ġ �̻����� ȸ������ �ʵ��� ����
                            continue; // �ִ�ġ �̻����� ȸ������ �ʵ��� ��
                        }

                        if (player != null) //���� �����ϸ�
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                        }
                        successAttack = true; // ���� ���� ���·� ����
                    }
                }
                else if (player.target == null && other.gameObject.CompareTag("Player")) // �� �±׸� ���� ������Ʈ�� �浹���� ��
                {
                    Debug.Log($"PlayerAttackBox: Player in H2 range!"); //����� �޽��� ���
                    Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��

                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // �÷��̾��� HP�� �ִ�ġ �̻����� ȸ������ �ʵ��� ����
                            continue; // �ִ�ġ �̻����� ȸ������ �ʵ��� ��
                        }

                        if (player != null) //���� �����ϸ�
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                        }
                        successAttack = true; // ���� ���� ���·� ����
                    }
                }
                playersInRange.Clear();
            }
            else
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //����� �޽��� ���
                    Enemy enemy = other.GetComponent<Enemy>(); //�÷��̾� ��ũ��Ʈ�� ã��
                    if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                        return;

                    if (enemy != null) //���� �����ϸ�
                    {
                        TakeDamage(enemy, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    }
                    successAttack = true; // ���� ���� ���·� ����
                }
                else if (player.target == null && other.gameObject.CompareTag("Enemy")) // �� �±׸� ���� ������Ʈ�� �浹���� ��
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //����� �޽��� ���
                    Enemy enemy = other.GetComponent<Enemy>(); //�÷��̾� ��ũ��Ʈ�� ã��

                    if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                        return;

                    if (enemy != null) //���� �����ϸ�
                    {

                        TakeDamage(enemy, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��

                    }
                    successAttack = true; // ���� ���� ���·� ����
                }
            }
        }
        else if (!isSingleAttack)
        {
            if (skillType == PlayerSkillType.Heal)
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Player in H1 range!"); //����� �޽��� ���
                    Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��
                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // �÷��̾��� HP�� �ִ�ġ �̻����� ȸ������ �ʵ��� ����
                            continue; // �ִ�ġ �̻����� ȸ������ �ʵ��� ��
                        }

                        if (player != null) //���� �����ϸ�
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                        }
                        successAttack = true; // ���� ���� ���·� ����
                    }
                }
                else if (player.target == null && other.gameObject.CompareTag("Player")) // �� �±׸� ���� ������Ʈ�� �浹���� ��
                {
                    Debug.Log($"PlayerAttackBox: Player in H2 range!"); //����� �޽��� ���
                    Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��

                    if (player != null && !playersInRange.Contains(player))
                        playersInRange.Add(player);

                    foreach (Player p in playersInRange)
                    {
                        if (player.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                            continue;

                        if (player.currentHp >= player.maxHp)
                        {
                            player.currentHp = player.maxHp; // �÷��̾��� HP�� �ִ�ġ �̻����� ȸ������ �ʵ��� ����
                            continue; // �ִ�ġ �̻����� ȸ������ �ʵ��� ��
                        }

                        if (player != null) //���� �����ϸ�
                        {
                            TakeHealBuffer(player, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                        }
                        successAttack = true; // ���� ���� ���·� ����
                    }
                }
                playersInRange.Clear();
            }
            else
            {
                if (other.gameObject == player.target)
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //����� �޽��� ���
                    Enemy enemy = other.GetComponent<Enemy>(); //�÷��̾� ��ũ��Ʈ�� ã��
                    if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                        return;

                    if (enemy != null) //���� �����ϸ�
                    {
                        TakeDamage(enemy, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    }
                    successAttack = true; // ���� ���� ���·� ����
                }
                else if (player.target == null && other.gameObject.CompareTag("Enemy")) // �� �±׸� ���� ������Ʈ�� �浹���� ��
                {
                    Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //����� �޽��� ���
                    Enemy enemy = other.GetComponent<Enemy>(); //�÷��̾� ��ũ��Ʈ�� ã��

                    if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                        return;

                    if (enemy != null) //���� �����ϸ�
                    {

                        TakeDamage(enemy, hpDamage, mentalDamage); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��

                    }
                    successAttack = true; // ���� ���� ���·� ����
                }
            }
        }
    }

    public void SingleCheck(PlayerSkill skill)
    {
        if (skill.playerSkillType == PlayerSkillType.SingleAttack)
        {
            isSingleAttack = true; // �̱� ���� ���θ� true�� ����
        }
        else if (skill.playerSkillType == PlayerSkillType.MultiAttack)
        {
            isSingleAttack = false; // ��Ƽ ���� ���θ� false�� ����
        }
    }

    public void SkillDamage(PlayerSkill skill)
    {
        hpDamage = skill.hpDamage; //��ų�� HP ���ط� ����
        mentalDamage = skill.mentalDamage; //��ų�� ���ŷ� ���ط� ����
        skillType = skill.playerSkillType; //��ų Ÿ�� ����
    }

    public void TakeDamage(Enemy enemy, int hpDamage, int mentalDamage)
    {
        enemy.currentHp -= hpDamage; // �÷��̾��� HP ����
    }

    public void TakeHealBuffer(Player player, int hpHeal, int mentalHeal)
    {
        player.currentHp += hpHeal; // �÷��̾��� HP ȸ��
        player.currentMental += mentalHeal; // �÷��̾��� ���ŷ� ȸ��
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
