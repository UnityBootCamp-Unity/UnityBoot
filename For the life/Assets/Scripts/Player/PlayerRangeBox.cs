using UnityEngine;

public class PlayerRangeBox : MonoBehaviour
{
    private Player player; //�÷��̾� ��ũ��Ʈ ����
    public bool isRange = false; // Ÿ�� ���� ����
    public bool isLongReach = true; // false : �ܰŸ�, true : ���Ÿ�

    public bool healBuffer; // �� ���� ����

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //�θ� ������Ʈ���� Player ��ũ��Ʈ�� ã��
    }

    private void OnEnable()
    {
        isRange = false; //���� �ʱ�ȭ
        isLongReach = true; //���Ÿ� ���� �ʱ�ȭ
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (healBuffer)
        {
            Debug.Log($"[RangeBox] �浹 ��� �±�: {other.tag}");
            if (other.gameObject == player.target) //�÷��̾� �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Player targetPlayer = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��
                if (targetPlayer.currentHp <= 0) // �÷��̾��� HP�� 0���� ū�� Ȯ��
                {
                    isRange = false;
                    return;
                }
                isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
                return;
            }
            else if (player.target == null && other.gameObject.CompareTag("Player")) //�÷��̾� �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Player targetPlayer = other.GetComponent<Player>(); //�÷��̾� ��ũ��Ʈ�� ã��
                if (targetPlayer.currentHp <= 0) // �÷��̾��� HP�� 0���� ū�� Ȯ��
                {
                    isRange = false;
                    return;
                }
                isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
                return;
            }
        }
        
        if (healBuffer) { return; } // �� ���۰� Ȱ��ȭ�� ���¿��� �ٸ� �浹�� ����

        if (other.gameObject == player.target) //Enemy �±׸� ���� ������Ʈ�� �浹���� ��
        {
            Enemy enemy = other.GetComponent<Enemy>(); //�� ��ũ��Ʈ�� ã��
            if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
            {
                isRange = false;
                return;
            }

            isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
            return;
        }
        else if (player.target == null)
        {
            if (other.gameObject.CompareTag("Enemy")) //Enemy �±׸� ���� ������Ʈ�� �浹���� ��
            {
                Enemy enemy = other.GetComponent<Enemy>(); //�� ��ũ��Ʈ�� ã��
                if (enemy.currentHp <= 0) // ���� HP�� 0���� ū�� Ȯ��
                {
                    isRange = false;
                    return;
                }

                isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
                //���� ��ü�� Ÿ���� ���ϴ� �͵� ������ ��
                return;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (healBuffer)
        {
            if (other.gameObject == player.target) //Player �±׸� ���� ������Ʈ�� �浹�� ������ ��
            {
                isRange = false; //Ÿ�� ���� ���� ���θ� false�� ����
            }
            else if (other.gameObject.CompareTag("Player")) //Player �±׸� ���� ������Ʈ�� �浹�� ������ ��
            {
                isRange = false; //Ÿ�� ���� ���� ���θ� false�� ����
            }
        }

        if(healBuffer) { return; } // �� ���۰� Ȱ��ȭ�� ���¿��� �ٸ� �浹�� ����

        if (other.gameObject == player.target) //Enemy �±׸� ���� ������Ʈ�� �浹�� ������ ��
        {
            isRange = false; //Ÿ�� ���� ���� ���θ� false�� ����
        }
        else if (other.gameObject.CompareTag("Enemy")) //Enemy �±׸� ���� ������Ʈ�� �浹�� ������ ��
        {
            isRange = false; //Ÿ�� ���� ���� ���θ� false�� ����
        }
    }

    public void CheckHealBuffer(PlayerSkill skill)
    {
        if (skill.playerSkillType == PlayerSkillType.Heal)
        {
            healBuffer = true; // �� ���� Ȱ��ȭ
        }
        else
        {
            healBuffer = false; // �� ���� ��Ȱ��ȭ
        }
    }
}
