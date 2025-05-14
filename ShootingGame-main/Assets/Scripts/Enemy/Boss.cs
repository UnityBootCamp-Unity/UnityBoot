using System;
using System.Collections;
using UnityEngine;

public enum Skill{
    skill1, skill2, skill3
}

public class Boss : MonoBehaviour
{
    public Action onDead;

    private float speed = 1f;
    private Vector3 dir;
    private float cooldown = 0f;
    private int skill;
    private int skills = Enum.GetValues(typeof(Skill)).Length - 1;

    private bool startMove = true;

    //������Ʈ Ǯ[Object Pool]
    [Header("������Ʈ Ǯ")]
    public int poolSize = 20;             //1. Ǯ�� ũ�⿡ ���� ����(�Ѿ� ����)
    public GameObject one_skill_bullet;
    public GameObject[] one_skill_pool; //2. ������Ʈ Ǯ(�迭 / ����Ʈ)
    public GameObject[] one_skill_transform;

    private void OnEnable()
    {
        dir = Vector3.down;
        StartCoroutine(StartMove(4.11f));
    }

    private void Start()
    {
        //������ ���� �̿��� �Ѿ��� ������ �ٲ� �� �ִ� �����̶��
        //Ǯ�� ����� �ִ�� ��Ƶΰ� ������ ��,
        //�÷��̾��� ���� �Ѿ� ������ ���� �Ѿ��� �߻��� �� �ֵ��� �����մϴ�.
        one_skill_pool = new GameObject[poolSize]; //3. ���� �κп��� Ǯ�� ���� �Ҵ�

        for (int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(one_skill_bullet); //4. Ǯ�� ũ�⸸ŭ ������ �����մϴ�.

            one_skill_pool[i] = bullet;            //5. ������ ������Ʈ�� Ǯ�� ����մϴ�.
                                                     //�迭�� ��� �ε�����, ����Ʈ�� ��� Add�� �߰�

            bullet.SetActive(false);                 //6. ������ �Ѿ��� ��Ȱ��ȭ�մϴ�.
                                                     //(�߻��� ���� Ȱ��ȭ)
        }

    }

    private void Update()
    {

        cooldown -= Time.deltaTime;
        skill = UnityEngine.Random.Range(0, skills);

        if (transform.position.y < 4.11)
        {
            if (cooldown < 0)
            {
                switch (skill)
                {
                    case 0:
                        Skill1();
                        cooldown = 0.5f;
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
            }
        }


        Move();
        

    }

    private void Move()
    {
        if (transform.position.y < 4.11)
        {
            dir.y = 0;

            if (startMove)
            {
                if (UnityEngine.Random.Range(0, 1) == 0)
                    dir = Vector3.right;
                else
                    dir = Vector3.left;

                startMove = false;
            }

            if (transform.position.x > 8)
                dir = Vector3.left;

            if (transform.position.x < -8)
                dir = Vector3.right;

            transform.position += dir * speed * Time.deltaTime;
        }
    }

    public void Dead()
    {
        onDead?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator StartMove(float Y)
    {
        while (transform.position.y > Y)
        {
            transform.position += dir * speed * Time.deltaTime;
            yield return null; // �� ������ ��ٸ�
        }
    }

    public void Skill1()
    {
        int shoot = UnityEngine.Random.Range(0, 5);
         //1. Ǯ �����ŭ �ݺ�
        for (int i = 0; i < poolSize; i++)
        {
            //2. Ǯ�� �ִ� �Ѿ� �ϳ��� �޾ƿɴϴ�.
            var bullet = one_skill_pool[i];

            if(bullet.activeSelf == false)
            {
                //4. �߻� ��ġ ����
                bullet.transform.position = one_skill_transform[shoot].transform.position;

                //3. ��Ȱ��ȭ�� ��� Ȱ��ȭ�� �����մϴ�.
                bullet.SetActive(true);

                //  �ڽ� ������Ʈ�� ��� �ٽ� �ѱ�
                for (int j = 0; j < bullet.transform.childCount; j++)
                {
                    bullet.transform.GetChild(j).gameObject.SetActive(true);
                }

                //5. �ݺ��� ����
                break;
            }
        }
        //switch (shoot)
        //{
        //    case 0:

        //}
    }

}
