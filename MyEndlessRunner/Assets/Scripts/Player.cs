using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController controller; //������Ʈ
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
        moveVector = Vector3.zero; //���� ���� �� ����

        //���� ������� ��� velocity ����
        if (controller.isGrounded)
        {
            vertical_velocity = -0.0f;

            //���� ��� �߰�
            if (Input.GetKey(KeyCode.C) && !isJumping)
            {
                isJumping = true;
                animator.SetBool("IsJump", true);
                StartCoroutine(WaitForJump());
            }
        }
        else
        {
            //�ƴ� ��� �߷�ġ��ŭ ����������
            vertical_velocity -= gravity * Time.deltaTime; //�߷� ����
        }

        //1. �¿� �̵�
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        //2. ���� ����
        moveVector.y = vertical_velocity;
        //3. ������ �̵�
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
