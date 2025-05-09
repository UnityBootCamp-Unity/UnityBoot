using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController controller; //������Ʈ
    public static Animator animator;

    private Vector3 moveVector;
    private float vertical_velocity = 0.0f;
    private float horizontal_velocity = 0.0f;
    private float gravity = 12.0f;

    [SerializeField] private float jump_speed = 5.0f;
    [SerializeField] private float jump = 5.0f;

    private bool isJumping = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveVector = Vector3.zero; //���� ���� �� ����

        //���� ������� ��� velocity ����
        if (controller.isGrounded)
        {
            vertical_velocity = -0.0f;
            horizontal_velocity = -0.0f;

            //���� ��� �߰�
            if (Input.GetKey(KeyCode.C) && !isJumping)
            {
                isJumping = true;
                animator.SetBool("IsJump", true);
                StartCoroutine(WaitForJump());
                StartCoroutine(JumpUp());
            }
        }
        else
        {
            //�ƴ� ��� �߷�ġ��ŭ ����������
            vertical_velocity -= gravity * Time.deltaTime; //�߷� ����
        }

        //1. �¿� ����
        moveVector.x = 0.0f;
        //2. Y�� ����
        moveVector.y = vertical_velocity;
        //3. X�� ����
        moveVector.z = horizontal_velocity;


        controller.Move(moveVector * Time.deltaTime);
    }

    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(1.8f);
        animator.SetBool("IsJump", false);
        isJumping = false;
    }

    IEnumerator JumpUp()
    {
        yield return new WaitForSeconds(0.8f);
        vertical_velocity = jump;
        horizontal_velocity = jump_speed;
        //controller.Move(moveVector * Time.deltaTime);
    }
}
