using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target; //플레이어의 위치
    Vector3 camera_offset; //카메라와 플레이어 간의 거리 간격
    Vector3 moveVector; //카메라가 매 프레임 이동할 위치

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
