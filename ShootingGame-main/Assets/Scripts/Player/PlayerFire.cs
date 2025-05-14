using UnityEngine;
using UnityEngine.UIElements;

//발사대에 연결할 스크립트
//발사 버튼을 눌렀을 경우 총알 발사
//현재 총알에는 방향에 따라 이동하는 코드가 구현이 되어있음.
//따라서 버튼 누르면 생성되게만 만들어주면 구현 완료

public class PlayerFire : MonoBehaviour
{
    [Header("발사 설정")]
    public GameObject bulletFactory; //총알 공장
    public GameObject firePosition;  //발사 지점

    //오브젝트 풀[Object Pool]
    [Header("오브젝트 풀")]
    public int poolSize = 20;             //1. 풀의 크기에 대한 설정(총알 개수)
    public GameObject[] bulletObjectPool; //2. 오브젝트 풀(배열 / 리스트)

    private float fireCooldown = 0.0f;

    private void Start()
    {
        //아이템 등을 이용해 총알의 개수를 바꿀 수 있는 게임이라면
        //풀의 사이즈를 최대로 잡아두고 생성한 뒤,
        //플레이어의 소유 총알 개수를 통해 총알을 발사할 수 있도록 설정합니다.
        bulletObjectPool = new GameObject[poolSize]; //3. 시작 부분에서 풀에 대한 할당

        for(int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(bulletFactory); //4. 풀의 크기만큼 생성을 진행합니다.

            bulletObjectPool[i] = bullet;            //5. 생성된 오브젝트를 풀에 등록합니다.
                                                     //배열일 경우 인덱스로, 리스트일 경우 Add로 추가

            bullet.SetActive(false);                 //6. 생성된 총알을 비활성화합니다.
                                                     //(발사할 때만 활성화)
        }

    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0.0f)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                fireCooldown = 0.5f;
            }
        }
    }
    
    void Shoot()
    {
        /*var bullet = Instantiate(bulletFactory);
        bullet.transform.position = firePosition.transform.position;*/

        //1. 풀 사이즈만큼 반복
        for (int i = 0; i < poolSize; i++)
        {
            //2. 풀에 있는 총알 하나를 받아옵니다.
            var bullet = bulletObjectPool[i];

            if(bullet.activeSelf == false)
            {
                //3. 비활성화일 경우 활성화를 진행합니다.
                bullet.SetActive(true);
                //4. 발사 위치 조정
                bullet.transform.position = firePosition.transform.position;

                //  자식 오브젝트들 모두 다시 켜기
                for (int j = 0; j < bullet.transform.childCount; j++)
                {
                    bullet.transform.GetChild(j).gameObject.SetActive(true);
                }

                //5. 반복문 종료
                break;
            }
        }
    }
}
