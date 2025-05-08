using UnityEngine;

public class TestController : MonoBehaviour
{
    //스피드
    [SerializeField] private float speed = 5.0f;
    //방향
    Vector3 moveVector;

    //캐릭터 컨트롤러
    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed; //좌우
        moveVector.z = Input.GetAxisRaw("Vertical") * speed;   //상하
        controller.Move(moveVector * Time.deltaTime);
    }

    //충돌 처리에 대한 이벤트
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            Debug.Log("장애물이다.");
        }

        if(hit.transform.tag == "Stair")
        {
            Debug.Log("계단이다.");
        }
    }
}
