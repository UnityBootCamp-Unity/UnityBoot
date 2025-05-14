using System;
using UnityEngine;

//유닛 1개 생성기
public class EnemyManager : MonoBehaviour
{
    float currentTime; //현재 시간
    public float step = 1;//시간 간격
    public GameObject enemyFactory; //적 공장

    public Action onEnemySpawned; //적 생성 시의 콜백 기능 구현

    //오브젝트 풀
    [Header("오브젝트 풀")]
    public int poolSize = 10;
    public GameObject[] pool;
    public Transform[] spawnPoint; //기존의 생성 위치


    //기존 방식 : EnemyManager.cs를 연결한 생성 지점을 배치해서
    //            무한 생성

    //바꾸는 방식 : EnemyManager 는 1개, 생성 지점을 연결해
    //              해당 지점에 시간에 맞춰 활성화

    //[보스]
    [Header("보스 출현")]
    public bool isBoss = false;


    //태어날 때에 대한 작업
    private void OnEnable()
    {
        pool = new GameObject[poolSize];

        for(int i = 0; i < poolSize; i++)
        {
            var enemy = Instantiate(enemyFactory);
            pool[i] = enemy;
            enemy.SetActive(false);
        }
    }

    private void Update()
    {
        //보스의 등장 이후 일반 유닛에 대한 풀 생성 중지
        if (isBoss)
            return;

        currentTime += Time.deltaTime;

        if (currentTime > step)
        {
            //var enemy = Instantiate(enemyFactory);
            //enemy.transform.position = transform.position;
            //enemy.transform.parent = transform;
            for (int i = 0; i < poolSize; i++)
            {
                var enemy = pool[i];
                if (enemy.activeSelf == false)
                {
                    enemy.transform.position = spawnPoint[i].position;
                    enemy.SetActive(true);
                    enemy.transform.parent = transform;

                    onEnemySpawned?.Invoke(); //이벤트 대한 실행

                    break;
                }
            }
            currentTime = 0;
        }
    }
}
