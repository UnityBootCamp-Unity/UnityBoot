using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public GameObject effect; //����Ʈ ���

    public Action onDead;
    Vector3 dir; //������ ����

    private float directionChangeCooldown = 0f;

    public void Die()
    {
        gameObject.SetActive(false);
        onDead?.Invoke();
    }

    private void Start()
    {

    }


    void Update()
    {
        directionChangeCooldown -= Time.deltaTime;
        if (directionChangeCooldown < 0f)
        {
            MoveToPlayer();
            directionChangeCooldown = 3.0f;
        }
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

        //�浹ü�� �̸��� Bullet�� ���Եȴٸ�?
        //�±׳� ���̾�� �����ص� ������.
        if (collision.gameObject.name.Contains("Bullet"))
        {
            //�浹ü�� ���� ��Ȱ��ȭ
            collision.gameObject.SetActive(false);
        }
        else
        {
            //�÷��̾ ��Ȱ��ȭ�� ó���ҰŸ� �ű⿡ �°� ������ ��
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
        //Destroy(gameObject);
        Die();
    }

    //�浹 ��
    private void OnCollisionExit(Collision collision)
    {
        
    }

    //�浹 ���� ��Ȳ
    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void MoveToPlayer()
    {
        int randValue = UnityEngine.Random.Range(0, 10); // 0 ~ 9
        //�÷��̾� �������� �̵�
        if (randValue < 7) // 0 1 2
        {
            //���� ������ "Player"�� �˻��մϴ�.
            //var target = GameObject.Find("Player");
            var target = GameObject.FindGameObjectWithTag("Player");
            dir = target.transform.position - transform.position;
            //�Ϲ�ȭ�� ���� �����ϰ� �̵��ϵ��� ó��
            //������ ũ�⸦ 1�� ����
            dir.Normalize();
            dir.y = -1;
        }
        else if(randValue >= 7) //�Ʒ��� �̵�
        {
            dir = Vector3.down;
        }
    }
}
