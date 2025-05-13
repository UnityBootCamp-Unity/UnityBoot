using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public GameObject effect; //이펙트 등록

    Vector3 dir; //움직일 방향

    private void Start()
    {
        int randValue = Random.Range(0, 10); // 0 ~ 9
        //플레이어 방향으로 이동
        if (randValue < 3) // 0 1 2
        {
            //게임 씬에서 "Player"를 검색합니다.
            var target = GameObject.Find("Player");
            dir = target.transform.position - transform.position;
            //일반화를 통해 균일하게 이동하도록 처리
            //방향의 크기를 1로 설정
            dir.Normalize();    
        }
        else //아래로 이동
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

    //충돌 시작
    private void OnCollisionEnter(Collision collision)
    {
        ScoreManager.Instance.Score++;
        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    //충돌 끝
    private void OnCollisionExit(Collision collision)
    {
        
    }

    //충돌 진행 상황
    private void OnCollisionStay(Collision collision)
    {
        
    }
}
