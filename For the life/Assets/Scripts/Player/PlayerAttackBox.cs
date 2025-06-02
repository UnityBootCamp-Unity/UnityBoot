using UnityEngine;

// Ÿ���� ������� �Ծ����� ���⼭ �浹 Ȯ�� ���ϰ� �ϱ�(Enable �� �ٽ� false�� �ʱ�ȭ)
public class PlayerAttackBox : MonoBehaviour
{
    private Player player; //�÷��̾� ��ũ��Ʈ ����
    public bool isSingleAttack = false; // �̱� ���� ����

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //�θ� ������Ʈ���� Player ��ũ��Ʈ�� ã��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isSingleAttack)
        {
            if (other.gameObject == player.target)
            {
                Debug.Log($"PlayerAttackBox: Enemy in attack range!"); //����� �޽��� ���
                /*Enemy enemy = other.GetComponent<Enemy>(); //�� ��ũ��Ʈ�� ã��
                if (enemy != null) //���� �����ϸ�
                {
                    enemy.TakeDamage(player.playerData.attackPower); //������ �÷��̾��� ���ݷ¸�ŭ ���ظ� ��
                    Debug.Log($"Player {player.playerData.unityName} attacked {enemy.name} for {player.playerData.attackPower} damage.");
                }*/
            }
        }
        else if (!isSingleAttack)
        {
            if (other.gameObject.CompareTag("Enemy")) // �� �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Debug.Log("PlayerAttackBox: Enemy in multi-attack range!"); // ����� �޽��� ���
                /*Enemy enemy = other.GetComponent<Enemy>(); // �� ��ũ��Ʈ�� ã��
                if (enemy != null) // ���� �����ϸ�
                {
                    enemy.TakeDamage(player.playerData.attackPower); // ������ �÷��̾��� ���ݷ¸�ŭ ���ظ� ��
                    Debug.Log($"Player {player.playerData.unityName} multi-attacked {enemy.name} for {player.playerData.attackPower} damage.");
                }*/
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
}
