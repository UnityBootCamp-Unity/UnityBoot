using System;
using UnityEngine;

//���� 1�� ������
public class EnemyManager : MonoBehaviour
{
    float currentTime; //���� �ð�
    public float step = 1;//�ð� ����
    public GameObject enemyFactory; //�� ����

    public Action onEnemySpawned; //�� ���� ���� �ݹ� ��� ����

    //������Ʈ Ǯ
    [Header("������Ʈ Ǯ")]
    public int poolSize = 10;
    public GameObject[] pool;
    public Transform[] spawnPoint; //������ ���� ��ġ


    //���� ��� : EnemyManager.cs�� ������ ���� ������ ��ġ�ؼ�
    //            ���� ����

    //�ٲٴ� ��� : EnemyManager �� 1��, ���� ������ ������
    //              �ش� ������ �ð��� ���� Ȱ��ȭ

    //[����]
    [Header("���� ����")]
    public bool isBoss = false;


    //�¾ ���� ���� �۾�
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
        //������ ���� ���� �Ϲ� ���ֿ� ���� Ǯ ���� ����
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

                    onEnemySpawned?.Invoke(); //�̺�Ʈ ���� ����

                    break;
                }
            }
            currentTime = 0;
        }
    }
}
