using UnityEngine;
using UnityEngine.UIElements;

//�߻�뿡 ������ ��ũ��Ʈ
//�߻� ��ư�� ������ ��� �Ѿ� �߻�
//���� �Ѿ˿��� ���⿡ ���� �̵��ϴ� �ڵ尡 ������ �Ǿ�����.
//���� ��ư ������ �����ǰԸ� ������ָ� ���� �Ϸ�

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletFactory; //�Ѿ� ����

    public GameObject firePosition;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        var bullet = Instantiate(bulletFactory);
        bullet.transform.position = firePosition.transform.position;
    }




 
}
