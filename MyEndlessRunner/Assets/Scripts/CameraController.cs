using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target; //�÷��̾��� ��ġ
    Vector3 camera_offset; //ī�޶�� �÷��̾� ���� �Ÿ� ����
    Vector3 moveVector; //ī�޶� �� ������ �̵��� ��ġ

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera_offset = transform.position - target.position;
    }

    void Update()
    {
        moveVector = target.position + camera_offset;
        moveVector.x = 9;
        moveVector.y = 1;

        transform.position = moveVector;
    }
}
