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
    [Header("오브젝트 풀")]
    public int poolSize = 20;             //1. 풀의 크기에 대한 설정(총알 개수)
    public GameObject one_skill_bullet;
    public GameObject[] one_skill_pool; //2. 오브젝트 풀(배열 / 리스트)
    public GameObject[] one_skill_transform;

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
        one_skill_pool = new GameObject[poolSize]; //3. 시작 부분에서 풀에 대한 할당

        for (int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(one_skill_bullet); //4. 풀의 크기만큼 생성을 진행합니다.

            one_skill_pool[i] = bullet;            //5. 생성된 오브젝트를 풀에 등록합니다.
                                                     //배열일 경우 인덱스로, 리스트일 경우 Add로 추가

            bullet.SetActive(false);                 //6. 생성된 총알을 비활성화합니다.
                                                     //(발사할 때만 활성화)
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
            yield return null; // 한 프레임 기다림
        }
    }

    public void Skill1()
    {
        int shoot = UnityEngine.Random.Range(0, 5);
         //1. 풀 사이즈만큼 반복
        for (int i = 0; i < poolSize; i++)
        {
            //2. 풀에 있는 총알 하나를 받아옵니다.
            var bullet = one_skill_pool[i];

            if(bullet.activeSelf == false)
            {
                //4. 발사 위치 조정
                bullet.transform.position = one_skill_transform[shoot].transform.position;

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
        }
        //switch (shoot)
        //{
        //    case 0:

        //}
    }

}
