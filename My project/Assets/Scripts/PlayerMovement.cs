using System;
using UnityEngine;


[Serializable]
public class PlayerStat
{
    public float speed = 5.0f;          //�÷��̾��� �̵� �ӵ�
    //public int count_of_harvest; //���� ��Ȯ���� ����(2025-50-23 : �κ��丮 ���� �׽�
    //Ʈ�� ���� ��Ȱ��ȭ
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private Vector2 last = Vector2.down;
    void SetAnimateMovement(Vector3 direction)
    {
        if (animator != null)
        {
            //magnitude : ������ ����
            //x,y,z�� ���� ������ ������ ���� ��Ʈ ��
            if (direction.magnitude > 0.0f)
            {
                animator.SetBool("IsMove", true);

                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);

                last = direction.normalized;
            }
            else 
            {
                animator.SetBool("IsMove", false);

                animator.SetFloat("horizontal", last.x);
                animator.SetFloat("vertical", last.y);
            }
        }
    }

    void SetAnimateSlash()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("Slash", true);
            animator.SetFloat("horizontal", last.x);
            animator.SetFloat("vertical", last.y);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            animator.SetBool("Slash", false);
            animator.SetFloat("horizontal", last.x);
            animator.SetFloat("vertical", last.y);
        }
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector2(h, v);
        SetAnimateMovement(dir);
        transform.position += dir * stat.speed * Time.deltaTime;

        SetAnimateSlash();
    }
}
