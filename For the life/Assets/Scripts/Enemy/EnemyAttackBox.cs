using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    
    private Enemy enemy; //�� ��ũ��Ʈ ����
    public bool isSingleAttack = false; // �̱� ���� ����

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
        if (isSingleAttack)
        {
            if (other.gameObject == enemy.target)
            {
                Debug.Log($"EnemyAttackBox: Player in attack range!"); //����� �޽��� ���
                /*Player player = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��
                if (player != null) //�÷��̾ �����ϸ�
                {
                    player.TakeDamage(enemy.enemyData.attackPower); //�÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    Debug.Log($"Enemy {enemy.enemyData.unityName} attacked {player.name} for {enemy.enemyData.attackPower} damage.");
                }*/
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Player")) // �÷��̾� �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Debug.Log("EnemyAttackBox: Player in multi-attack range!"); // ����� �޽��� ���
                /*Player player = other.GetComponent<Player>(); // �÷��̾� ��ũ��Ʈ�� ã��
                if (player != null) // �÷��̾ �����ϸ�
                {
                    player.TakeDamage(enemy.enemyData.attackPower); // �÷��̾�� ���� ���ݷ¸�ŭ ���ظ� ��
                    Debug.Log($"Enemy {enemy.enemyData.unityName} multi-attacked {player.name} for {enemy.enemyData.attackPower} damage.");
                }*/
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
}
