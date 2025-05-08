using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController controller; //컴포넌트
    Animator animator;

    private Vector3 moveVector;
    private float vertical_velocity = 0.0f;
    private float gravity = 12.0f;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump = 5.0f;

    private bool isJumping = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveVector = Vector3.zero; //방향 벡터 값 리셋

        //땅에 닿아있을 경우 velocity 고정
        if (controller.isGrounded)
        {
            vertical_velocity = -0.0f;

            //점프 기능 추가
            if (Input.GetKey(KeyCode.C) && !isJumping)
            {
                isJumping = true;
                animator.SetBool("IsJump", true);
                StartCoroutine(WaitForJump());
            }
        }
        else
        {
            //아닐 경우 중력치만큼 떨어지도록
            vertical_velocity -= gravity * Time.deltaTime; //중력 적용
        }

        //1. 좌우 이동
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        //2. 점프 관련
        moveVector.y = vertical_velocity;
        //3. 앞으로 이동
        moveVector.z = Input.GetAxis("Vertical") * speed;
        
        controller.Move(moveVector * Time.deltaTime);
    }

    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("IsJump", false);
        isJumping = false;
    }
}
