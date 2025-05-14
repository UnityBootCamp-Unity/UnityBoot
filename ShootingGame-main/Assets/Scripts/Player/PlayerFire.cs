using UnityEngine;
using UnityEngine.UIElements;

//�߻�뿡 ������ ��ũ��Ʈ
//�߻� ��ư�� ������ ��� �Ѿ� �߻�
//���� �Ѿ˿��� ���⿡ ���� �̵��ϴ� �ڵ尡 ������ �Ǿ�����.
//���� ��ư ������ �����ǰԸ� ������ָ� ���� �Ϸ�

public class PlayerFire : MonoBehaviour
{
    [Header("�߻� ����")]
    public GameObject bulletFactory; //�Ѿ� ����
    public GameObject firePosition;  //�߻� ����

    //������Ʈ Ǯ[Object Pool]
    [Header("������Ʈ Ǯ")]
    public int poolSize = 20;             //1. Ǯ�� ũ�⿡ ���� ����(�Ѿ� ����)
    public GameObject[] bulletObjectPool; //2. ������Ʈ Ǯ(�迭 / ����Ʈ)

    private float fireCooldown = 0.0f;

    private void Start()
    {
        //������ ���� �̿��� �Ѿ��� ������ �ٲ� �� �ִ� �����̶��
        //Ǯ�� ����� �ִ�� ��Ƶΰ� ������ ��,
        //�÷��̾��� ���� �Ѿ� ������ ���� �Ѿ��� �߻��� �� �ֵ��� �����մϴ�.
        bulletObjectPool = new GameObject[poolSize]; //3. ���� �κп��� Ǯ�� ���� �Ҵ�

        for(int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(bulletFactory); //4. Ǯ�� ũ�⸸ŭ ������ �����մϴ�.

            bulletObjectPool[i] = bullet;            //5. ������ ������Ʈ�� Ǯ�� ����մϴ�.
                                                     //�迭�� ��� �ε�����, ����Ʈ�� ��� Add�� �߰�

            bullet.SetActive(false);                 //6. ������ �Ѿ��� ��Ȱ��ȭ�մϴ�.
                                                     //(�߻��� ���� Ȱ��ȭ)
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

        //1. Ǯ �����ŭ �ݺ�
        for (int i = 0; i < poolSize; i++)
        {
            //2. Ǯ�� �ִ� �Ѿ� �ϳ��� �޾ƿɴϴ�.
            var bullet = bulletObjectPool[i];

            if(bullet.activeSelf == false)
            {
                //3. ��Ȱ��ȭ�� ��� Ȱ��ȭ�� �����մϴ�.
                bullet.SetActive(true);
                //4. �߻� ��ġ ����
                bullet.transform.position = firePosition.transform.position;

                //  �ڽ� ������Ʈ�� ��� �ٽ� �ѱ�
                for (int j = 0; j < bullet.transform.childCount; j++)
                {
                    bullet.transform.GetChild(j).gameObject.SetActive(true);
                }

                //5. �ݺ��� ����
                break;
            }
        }
    }
}
