using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public PlayerHealth playerHealth;   //플레이어 체력(죽음 상태 확인)
    public GameObject enemy;            //소환할 몬스터
    public float intervalTime = 10.0f;   //반복 시간(젠 타임)
    public Transform[] spawnPools;      //소환 지점

    private void Start()
    {
        InvokeRepeating("Spawn", intervalTime, intervalTime);
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
            return;

        //모든 몬스터의 생성 지점 중 하나를 랜덤으로 선택
        int spawnPointIndex = Random.Range(0, spawnPools.Length);

        //선택된 지점의 위치와 회전 값을 받아서 생성
        Instantiate(enemy, spawnPools[spawnPointIndex].position, spawnPools[spawnPointIndex].rotation, spawnPools[spawnPointIndex]);

    }
}
