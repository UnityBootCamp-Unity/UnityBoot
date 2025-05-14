using UnityEngine;

//추가로 고려할 것
//1. 보스에 대한 Boss 클래스
//2. 보스의 정보를 관리하는 BossData 클래스(SO)

public class BossManager : MonoBehaviour
{
    public GameObject boss;
    public EnemyManager enemyManager;

    public void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Instantiate(boss);

        enemyManager.isBoss = true;
    }

    public void OnDead()
    {
        enemyManager.isBoss = false;
    }
}
