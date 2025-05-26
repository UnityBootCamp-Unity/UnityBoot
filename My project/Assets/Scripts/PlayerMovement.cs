using System;
using UnityEngine;


[Serializable]
public class PlayerStat
{
    public float speed = 5.0f;          //플레이어의 이동 속도
    //public int count_of_harvest; //현재 수확물의 개수(2025-50-23 : 인벤토리 로직 테스
    //트로 인해 비활성화
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
            //magnitude : 벡터의 길이
            //x,y,z에 대한 각각의 제곱의 합의 루트 값
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
            animator.SetTrigger("Slash");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            animator.ResetTrigger("Slash");
        }
    }

    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        SetAnimateSlash();
        if (state.IsName("Slash"))
            return;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector2(h, v);
        SetAnimateMovement(dir);

        
        
        transform.position += dir * stat.speed * Time.deltaTime;
    }
}
