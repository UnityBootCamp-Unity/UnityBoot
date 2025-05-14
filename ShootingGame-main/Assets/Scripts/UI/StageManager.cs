using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int kill; //죽여야하는 수
    public int killed; //죽인 수

    //적 매니저 연결(싱글톤으로 바꾸면 연결 x)
    public EnemyManager enemyManager;
    //보스 매니저 연결
    public BossManager bossManager;

    private void Start()
    {
        //적 생성 시에 카운트를 증가(이벤트)
        enemyManager.onEnemySpawned += EnemySpawned;

        //풀 내에 있는 몬스터들에게 이벤트 연결
        foreach(var enemy in enemyManager.pool)
        {
            var go = enemy.GetComponent<Enemy>();
            go.onDead += EnemyKilled;
        }
    }

    private void Update()
    {
        //보스가 처치되었을 경우 적을 다시 생성한다.(스테이지 레벨 업 등이 처리됨)
    }

    void EnemySpawned() => kill++;

    void EnemyKilled()
    {
        killed++;
        Debug.Log($"마리수 : {killed} / {kill}");
        if (killed >= kill)
        {
            //스테이지 클리어
            Debug.Log("Stage Clear");
        }
    }
}
