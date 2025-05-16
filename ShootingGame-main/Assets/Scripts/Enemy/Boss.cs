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
    [Header("��ų1 ������Ʈ Ǯ")]
    public int onePoolSize = 30;             //1. Ǯ�� ũ�⿡ ���� ����(�Ѿ� ����)
    public GameObject one_skill_bullet;
    public GameObject[] one_skill_pool; //2. ������Ʈ Ǯ(�迭 / ����Ʈ)
    public GameObject[] one_skill_transform;

    [Header("��ų2 ������Ʈ Ǯ")]
    public int twoPoolSize = 30;
    public GameObject two_skill_bullet;
    public GameObject[] two_skill_pool;
    public GameObject[] two_skill_transform;


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
        one_skill_pool = new GameObject[onePoolSize]; //3. ���� �κп��� Ǯ�� ���� �Ҵ�

        for (int i = 0; i < onePoolSize; i++)
        {
            var bullet1 = Instantiate(one_skill_bullet); //4. Ǯ�� ũ�⸸ŭ ������ �����մϴ�.

            one_skill_pool[i] = bullet1;            //5. ������ ������Ʈ�� Ǯ�� ����մϴ�.
                                                     //�迭�� ��� �ε�����, ����Ʈ�� ��� Add�� �߰�

            bullet1.SetActive(false);                 //6. ������ �Ѿ��� ��Ȱ��ȭ�մϴ�.
                                                     //(�߻��� ���� Ȱ��ȭ)
        }

        two_skill_pool = new GameObject[twoPoolSize];

        for (int i = 0; i < twoPoolSize; i++)
        {
            var bullet2 = Instantiate(two_skill_bullet);
            two_skill_pool[i] = bullet2;
            bullet2.SetActive(false);
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
                        //Skill1();
                        //cooldown = 6.0f;
                        break;
                    case 1:
                        Skill2();
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
        float duration = 5.0f;
        int shoot = 0;
        var target = GameObject.FindGameObjectWithTag("Player");
        while (duration >= 0f)
        {
            if (target != null)
            {
                if (target.transform.position.x > transform.position.x && target.transform.position.y > 2)
                    shoot = UnityEngine.Random.Range(0, 3);
                else if (target.transform.position.x < transform.position.x && target.transform.position.y > 2)
                    shoot = UnityEngine.Random.Range(3, 6);
                else
                    shoot = UnityEngine.Random.Range(0, 6);
            }
            else
            {
                shoot = UnityEngine.Random.Range(0, 6);
            }

            //1. Ǯ �����ŭ �ݺ�
            for (int i = 0; i < onePoolSize; i++)
            {
                //2. Ǯ�� �ִ� �Ѿ� �ϳ��� �޾ƿɴϴ�.
                var bullet = one_skill_pool[i];

                if (bullet.activeSelf == false)
                {
                    //4. �߻� ��ġ ����
                    bullet.transform.position = one_skill_transform[shoot].transform.position;

                    // ���⼭ Boss ���� �־��ֱ�
                    BossBullet1 bulletScript = bullet.GetComponent<BossBullet1>();
                    bulletScript.boss = this;


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
                duration -= Time.deltaTime;
            }
        }
    }

    private void Skill2()
    {
        var target = GameObject.FindGameObjectWithTag("Player");
        int shoot = 0;

        for (int i = 0; i < twoPoolSize; i++)
        {
            var targetCircle = two_skill_pool[i];

            if (targetCircle.activeSelf == false)
            {
                targetCircle.transform.position = two_skill_transform[shoot].transform.position;

                break;
            }
        }
    }

}
