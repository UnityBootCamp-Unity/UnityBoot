using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    Animator animator;
    public GameObject Character;

    public bool isTurning = false; // 방향 전환 여부

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
            transform.rotation = Quaternion.Euler(0, 180, 0); // 캐릭터를 오른쪽으로 회전
            isTurning = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 캐릭터를 왼쪽으로 회전
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
        transform.rotation = Quaternion.Euler(0, 0, 0); // 캐릭터를 기본 방향으로 회전
        isTurning = false; // 방향 전환 상태 초기화
    }

    IEnumerator Down()
    {
        float duration = 2; // 하강 시간
        yield return new WaitForSeconds(1f);
        while (duration >= 0)
        {
            Character.transform.position += Vector3.down  * Time.deltaTime * 10.0f;
            duration -= Time.deltaTime; // 남은 시간 감소
            yield return null; // 프레임마다 기다림
        }
    }
}
