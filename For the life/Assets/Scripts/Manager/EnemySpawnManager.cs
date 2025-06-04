using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("�� ����")]
    public GameObject[] monsterPrefabs; //���� ������ �迭
    public Transform[] spawnPoints; //���� ��ȯ ��ġ �迭
    public int initialPoolSizePerType = 10; //�� ���� ������ �ʱ� Ǯ ũ��
    public float spawnInterval = 2f; //���� ��ȯ ���� (��)
    public float spawnTime = 0f; //��ȯ Ÿ�̸�

    public Transform poolParent; //�� ������Ʈ�� �Ҵ��� ���� (Hierarchy���� ����)

    //���� �������� ����Ʈ�� ����
    private Dictionary<GameObject, List<GameObject>> poolDict = new Dictionary<GameObject, List<GameObject>>();

    private void Start()
    {
        // �� ���� �������� �ʱ� Ǯ ����
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
            return; //�� ���� ���϶��� ����
        }

        if (DefenseManager.instance.turnCount >= DefenseManager.instance.maxTurnCount)
            return; //�ִ� �� ���� �����ϸ� ���� ��ȯ ����

        // ���� ��ȯ ���ݿ� ���� ���͸� ��ȯ
        if (spawnTime <= 0)
        {
            SpawnMonster();
            spawnTime = spawnInterval; // ��ȯ ���� �ʱ�ȭ
        }

        spawnTime -= Time.deltaTime;
    }

    private void SpawnMonster()
    {
        // ������ ���� ������ ����
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

        // ��Ȱ��ȭ�� ���� ��ȯ
        foreach (var obj in poolList)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // �����ϸ� ���� ����
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        if (poolParent != null) newObj.transform.SetParent(poolParent);
        poolList.Add(newObj);
        return newObj;
    }

    public void ReturnMonsterToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        //���⼭ �θ� �ٽ� poolParent�� �ű� �ʿ�� ������
        //�����ϰ� �����Ϸ��� �Ʒ� �ڵ带 �߰��ص� ��
        if (poolParent != null)
        {
            enemy.transform.SetParent(poolParent);
        }
    }
}