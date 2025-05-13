using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public GameObject effect; //����Ʈ ���

    Vector3 dir; //������ ����

    private void Start()
    {
        int randValue = Random.Range(0, 10); // 0 ~ 9
        //�÷��̾� �������� �̵�
        if (randValue < 3) // 0 1 2
        {
            //���� ������ "Player"�� �˻��մϴ�.
            var target = GameObject.Find("Player");
            dir = target.transform.position - transform.position;
            //�Ϲ�ȭ�� ���� �����ϰ� �̵��ϵ��� ó��
            //������ ũ�⸦ 1�� ����
            dir.Normalize();    
        }
        else //�Ʒ��� �̵�
        {
            dir = Vector3.down;
        }
    }


    void Update()
    {

        //Vector3 dir = Vector3.down;

        //transform.Translate(dir * speed * Time.deltaTime);
        transform.position += dir * speed * Time.deltaTime;
    }

    //�浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        ScoreManager.Instance.Score++;
        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    //�浹 ��
    private void OnCollisionExit(Collision collision)
    {
        
    }

    //�浹 ���� ��Ȳ
    private void OnCollisionStay(Collision collision)
    {
        
    }
}
