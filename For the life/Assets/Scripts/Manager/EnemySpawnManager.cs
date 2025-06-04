using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("몹 스폰")]
    public GameObject[] monsterPrefabs; //몬스터 프리팹 배열
    public Transform[] spawnPoints; //몬스터 소환 위치 배열
    public int initialPoolSizePerType = 10; //각 몬스터 종류별 초기 풀 크기
    public float spawnInterval = 2f; //몬스터 소환 간격 (초)
    public float spawnTime = 0f; //소환 타이머

    public Transform poolParent; //빈 오브젝트를 할당할 변수 (Hierarchy에서 연결)

    //몬스터 종류별로 리스트를 관리
    private Dictionary<GameObject, List<GameObject>> poolDict = new Dictionary<GameObject, List<GameObject>>();

    private void Start()
    {
        // 각 몬스터 종류별로 초기 풀 생성
        foreach (var prefab in monsterPrefabs)
        {
            List<GameObject> poolList = new List<GameObject>();

            for (int i = 0; i < initialPoolSizePerType; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                if (poolParent != null) obj.transform.SetParent(poolParent);
                poolList.Add(obj);
            }

            poolDict[prefab] = poolList;
        }
    }

    private void Update()
    {
        if(DefenseManager.instance.isTurning)
        {
            return; //턴 진행 중일때는 중지
        }

        if (DefenseManager.instance.turnCount >= DefenseManager.instance.maxTurnCount)
            return; //최대 턴 수에 도달하면 몬스터 소환 중지

        // 몬스터 소환 간격에 따라 몬스터를 소환
        if (spawnTime <= 0)
        {
            SpawnMonster();
            spawnTime = spawnInterval; // 소환 간격 초기화
        }

        spawnTime -= Time.deltaTime;
    }

    private void SpawnMonster()
    {
        // 랜덤한 몬스터 프리팹 선택
        int prefabIndex = Random.Range(0, monsterPrefabs.Length);
        GameObject selectedPrefab = monsterPrefabs[prefabIndex];

        GameObject monster = GetPooledMonster(selectedPrefab);

        if (monster != null)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            monster.transform.position = spawnPoints[spawnIndex].position;
            monster.SetActive(true);
        }
    }

    private GameObject GetPooledMonster(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab))
            return null;

        List<GameObject> poolList = poolDict[prefab];

        // 비활성화된 몬스터 반환
        foreach (var obj in poolList)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // 부족하면 새로 생성
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        if (poolParent != null) newObj.transform.SetParent(poolParent);
        poolList.Add(newObj);
        return newObj;
    }

    public void ReturnMonsterToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        //여기서 부모를 다시 poolParent로 옮길 필요는 없지만
        //안전하게 유지하려면 아래 코드를 추가해도 됨
        if (poolParent != null)
        {
            enemy.transform.SetParent(poolParent);
        }
    }
}