using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;                                       //플레이어의 위치
    Vector3 camera_offset;                                  //카메라와 플레이어 간의 거리 간격
    Vector3 moveVector;                                     //카메라가 매 프레임 이동할 위치

    float transition = 0.0f;                                //보간 값(전환용)
    public static float camera_animate_duration = 3.0f;     //카메라를 이용해서 애니메이션 연출할 때 쓸 지속 시간
    public Vector3 animate_offset = new Vector3(0, 5, 5);   //애니메이션을 위한 시작 오프셋

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera_offset = transform.position - target.position; //시작 값 (카메라 위치 - 타겟 위치)
    }

    void Update()
    {
        moveVector = target.position + camera_offset;        //타겟 + 카메라 오프셋으로 방향 설정
        moveVector.x = 0;                                    //카메라 x축 고정(좌우 이동 안함)
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);      //카메라 y축 고정(3 ~ 5 사이의 값으로 제한)

        if(transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //전환이 진행되고 있는 상태일 때 진행할 작업
            //Vector3.Lerp(Vector a, Vector b, float t);
            //a부터 b까지 t 간격으로 서서히 이동하는 명령문(선형 보간)
            transform.position = Vector3.Lerp(moveVector + animate_offset, moveVector, transition);
            //오프셋 적용 위치에서 플레이어의 방향까지 보간 이동을 진행합니다.

            transition += Time.deltaTime / camera_animate_duration;
            //전환 값을 서서히 증가 시킵니다.(프레임 시간 / 애니메이션 시간)

            transform.LookAt(target.position + Vector3.up);
            //위를 쳐다보게 설계합니다.
        }
    }
}
