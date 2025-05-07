using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller; //������Ʈ
    Animator animator;

    private Vector3 moveVector;                                 //���� ����
    private float vertical_velocity = 0.0f;                     //������ ���� ���� �ӵ�
    private float gravity = 12.0f;                              //�߷� ��

    [SerializeField] private float speed = 5.0f;                //�÷��̾��� �̵� �ӵ�
    [SerializeField] private float jump = 5.0f;                 //�÷��̾��� ���� ��ġ

    private bool isJumping = false;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //ī�޶� ��Ʈ�ѷ��� �̿��� �÷��̾� ������ ���� ī�޶� ������ �����غ��� �մϴ�.
        if(Time.timeSinceLevelLoad < CameraController.camera_animate_duration)
        {
            // Time.timeScale = 1;
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        moveVector = Vector3.zero; //���� ���� �� ����

        //���� ������� ��� velocity ����
        if (controller.isGrounded)
        {
            vertical_velocity = -0.0f;

            //���� ��� �߰�
            if (Input.GetKey(KeyCode.UpArrow) && !isJumping && !isSliding)
            {
                isJumping = true;
                vertical_velocity = jump;
                animator.SetBool("IsJump", true);
                StartCoroutine(WaitForJump());
            }
            if (Input.GetKey(KeyCode.DownArrow) && !isSliding && !isJumping)
            {
                isSliding = true;
                animator.SetBool("IsSlide", true);
                StartCoroutine(WaitForSlide());
            }
        }
        else
        {
            //�ƴ� ��� �߷�ġ��ŭ ����������
            vertical_velocity -= gravity * Time.deltaTime; // �߷� ����
        }

        //1. �¿� �̵�
        moveVector.x = Input.GetAxis("Horizontal") * speed;
        //2. ���� ����
        moveVector.y = vertical_velocity;
        //3. ������ �̵�
        moveVector.z = speed;


        //������ ������ �̵� ����
        controller.Move(moveVector * Time.deltaTime);
    }

    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("IsJump", false);
        isJumping = false;
    }

    IEnumerator WaitForSlide()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("IsSlide", false);
        isSliding = false;
    }
}
