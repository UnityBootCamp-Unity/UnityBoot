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

    //오브젝트 풀[Object Pool]
    [Header("스킬1 오브젝트 풀")]
    public int onePoolSize = 30;             //1. 풀의 크기에 대한 설정(총알 개수)
    public GameObject one_skill_bullet;
    public GameObject[] one_skill_pool; //2. 오브젝트 풀(배열 / 리스트)
    public GameObject[] one_skill_transform;

    [Header("스킬2 오브젝트 풀")]
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
        //아이템 등을 이용해 총알의 개수를 바꿀 수 있는 게임이라면
        //풀의 사이즈를 최대로 잡아두고 생성한 뒤,
        //플레이어의 소유 총알 개수를 통해 총알을 발사할 수 있도록 설정합니다.
        one_skill_pool = new GameObject[onePoolSize]; //3. 시작 부분에서 풀에 대한 할당

        for (int i = 0; i < onePoolSize; i++)
        {
            var bullet1 = Instantiate(one_skill_bullet); //4. 풀의 크기만큼 생성을 진행합니다.

            one_skill_pool[i] = bullet1;            //5. 생성된 오브젝트를 풀에 등록합니다.
                                                     //배열일 경우 인덱스로, 리스트일 경우 Add로 추가

            bullet1.SetActive(false);                 //6. 생성된 총알을 비활성화합니다.
                                                     //(발사할 때만 활성화)
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
            yield return null; // 한 프레임 기다림
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

            //1. 풀 사이즈만큼 반복
            for (int i = 0; i < onePoolSize; i++)
            {
                //2. 풀에 있는 총알 하나를 받아옵니다.
                var bullet = one_skill_pool[i];

                if (bullet.activeSelf == false)
                {
                    //4. 발사 위치 조정
                    bullet.transform.position = one_skill_transform[shoot].transform.position;

                    // 여기서 Boss 참조 넣어주기
                    BossBullet1 bulletScript = bullet.GetComponent<BossBullet1>();
                    bulletScript.boss = this;


                    //3. 비활성화일 경우 활성화를 진행합니다.
                    bullet.SetActive(true);

                    //  자식 오브젝트들 모두 다시 켜기
                    for (int j = 0; j < bullet.transform.childCount; j++)
                    {
                        bullet.transform.GetChild(j).gameObject.SetActive(true);
                    }

                    //5. 반복문 종료
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
