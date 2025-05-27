using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    Animator animator;
    public GameObject Character;

    public bool isTurning = false; // ���� ��ȯ ����

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move()
    {
        animator.SetTrigger("Walk");
    }

    public void MoveTurn()
    {
        if (!isTurning)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // ĳ���͸� ���������� ȸ��
            isTurning = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // ĳ���͸� �������� ȸ��
            isTurning = false;
        }
        animator.SetTrigger("Walk");
    }

    public void Die()
    {
        animator.SetBool("Die", true);
        StartCoroutine(Down());
    }

    public void Idle()
    {
        animator.SetBool("Die", false);
        animator.ResetTrigger("Walk");
        transform.rotation = Quaternion.Euler(0, 0, 0); // ĳ���͸� �⺻ �������� ȸ��
        isTurning = false; // ���� ��ȯ ���� �ʱ�ȭ
    }

    IEnumerator Down()
    {
        float duration = 2; // �ϰ� �ð�
        yield return new WaitForSeconds(1f);
        while (duration >= 0)
        {
            Character.transform.position += Vector3.down  * Time.deltaTime * 10.0f;
            duration -= Time.deltaTime; // ���� �ð� ����
            yield return null; // �����Ӹ��� ��ٸ�
        }
    }
}
