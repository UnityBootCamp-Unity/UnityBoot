using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;


    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0);

        //transform.Translate(dir * speed * Time.deltaTime);
        transform.position += dir * speed * Time.deltaTime;


        //transform.Translate(Vector3 dir);
        //게임 오브젝트를 이동시키기 위한 용도
        //게임 오브젝트의 위치를 Vector3 방향으로 이동하게 됩니다.
        //(물리 엔진과 관련된 연산을 수행하지는 않고 단순한 이동 기능으로 구현)
        //--> 기본적인 움직임

        //transform.position을 통해 계산해둔 위치로 오브젝트의 position을 바꿀 수 있습니다.
        //--> 주로 포탈 같은 형태의 움직임 구현 시 효과적
        //position을 직접 움직일 오브젝트에는 rigidbody를 쓰지 않는게 좋습니다.
        //(콜라이더들이 rigidbody의 상대적인 위치를 재계산해야하는 경우가 발생합니다.)

        //Rigidbody는 게임 오브젝트에 물리 엔진을 적용해서 충돌, 힘, 중력 등의
        //물리적인 상호작용을 가능하게 해주는 컴포넌트입니다.

        //Rigidbody.AddForce(Vector3 dir, ForceMode mode);
        //물리적인 연산을 통해 움직임을 구현하고, 힘을 주는 설정에 따라 지속적으로 처리할 지,
        //순간적인 힘을 가할지를 처리할 수 있습니다.
    }
}
