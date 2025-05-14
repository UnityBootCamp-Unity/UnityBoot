using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public GameObject effect; //����Ʈ ���

    public Action onDead;
    Vector3 dir; //������ ����

    private bool non_to_player = false;

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
        //MoveToPlayer();
        Vector3 dir = Vector3.down;

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
            Destroy(collision.gameObject);
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
        if (randValue < 7 && non_to_player == false) // 0 1 2
        {
            //���� ������ "Player"�� �˻��մϴ�.
            //var target = GameObject.Find("Player");
            var target = GameObject.FindGameObjectWithTag("Player");
            dir = target.transform.position - transform.position;
            //�Ϲ�ȭ�� ���� �����ϰ� �̵��ϵ��� ó��
            //������ ũ�⸦ 1�� ����
            dir.Normalize();
        }
        else if(randValue >= 7 && non_to_player == false) //�Ʒ��� �̵�
        {
            dir = Vector3.down;
            StartCoroutine(GoDown());
        }
    }

    IEnumerator GoDown()
    {
        non_to_player = false;
        yield return new WaitForSeconds(3.0f);
        non_to_player = true;
    }
}
