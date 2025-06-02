using UnityEngine;

public class PlayerRangeBox : MonoBehaviour
{
    private Player player; //�÷��̾� ��ũ��Ʈ ����
    public bool isRange = false; // Ÿ�� ���� ����
    public bool isLongReach = true; // false : �ܰŸ�, true : ���Ÿ�

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
        if (player.target == null)
        {
            if(other.gameObject.CompareTag("Enemy")) //Enemy �±׸� ���� ������Ʈ�� �浹���� ��
            {
                isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
                //���� ��ü�� Ÿ���� ���ϴ� �͵� ������ ��
            }
        }

        if (other.gameObject == player.target) //Enemy �±׸� ���� ������Ʈ�� �浹���� ��
        {
            isRange = true; //Ÿ�� ���� ���� ���θ� true�� ����
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player.target) //Enemy �±׸� ���� ������Ʈ�� �浹�� ������ ��
        {
            isRange = false; //Ÿ�� ���� ���� ���θ� false�� ����
        }
    }
}
