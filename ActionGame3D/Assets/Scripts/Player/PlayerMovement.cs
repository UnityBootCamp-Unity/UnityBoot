using System.Collections;
using UnityEngine;

//1. �ش� ��ũ��Ʈ�� ��� �� �ִϸ����� ������Ʈ�� �䱸�մϴ�.
// -> �ش� �Ӽ��� ���ԵǾ��ִ� ��ũ��Ʈ�� ������Ʈ�� �������� ���
//    ������ ���� ó���˴ϴ�.
//    1. �䱸�ϰ� �ִ� �ִϸ����� ������Ʈ�� ���� ��� �ڵ� �������ݴϴ�.
//    2. �� ��ũ��Ʈ�� ������Ʈ�� ���Ǵ� ����, � ����
//       �䱸�ϴ� ������Ʈ�� ������ �� �����ϴ�.
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    PlayerAttack playerAttack;

    //����, ��ų, ��ÿ� ���� �ð�
    float lastAttackTime, lastSkillTime, lastDashTime;
    public bool attacking = false;
    public bool dashing = false;
    public bool skilling = false;


    //UI�� ��Ʈ�ѷ��� ��ġ�ؼ� �� ��Ʈ�ѷ��� �̵��� �����غ� ����
    float h, v;

    //��ƽ�� ��ġ�� ���޹޾Ƽ� x�� y���� ó���մϴ�.
    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    //UI�� ��ư ���� �̿��ؼ� ������ �����ؾ� �ϹǷ�, ��� ���� �Լ� ������ �����ϴ� ������ ���� ����
    //XXXDown : ������ �� (1��)
    //XXXUp : ������ ���� �� (1��)
    //XXX : ������ �ִ� ����

    //OnAttackUp, OnSkillUp, OnDashUp ���� ������ �����ϱ� ���� ������ ���ǿ� ���� ó���ϴ� �Լ� ==> �÷��� �Լ�

    //��Ÿ ���ݿ� ���� �ڷ�ƾ ����
    private IEnumerator Attack()
    {
        if(Time.time - lastAttackTime > 1f)
        {
            lastAttackTime = Time.time;
            while (attacking)
            {
                animator.SetTrigger("Attack");
                //�ִϸ������� �Ķ���� �߿��� SetTrigger��
                //�����ϴ� ������ ������ �ٷ� �����ϰ� �˴ϴ�.
                //���� ������ ��
                playerAttack.NormalAttack();
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public void OnAttackDown()
    {
        attacking = true;
        animator.SetBool("Combo", true);
        StartCoroutine(Attack());
    }
    public void OnAttackUp()
    {
        attacking = false;
        animator.SetBool("Combo", false);
        animator.ResetTrigger("Attack");
    }
    public void OnSkillDown()
    {
        if (Time.time - lastSkillTime > 1.0f)
        {
 
            animator.SetBool("Skill", true);
            lastSkillTime = Time.time;
            playerAttack.SkillAttack();
        }
    }
    public void OnSkillUp()
    {
        animator.SetBool("Skill", false);
    }
    public void OnDashDown()
    {
        if (Time.time - lastDashTime > 1.0f)
        {
            dashing = true;
            lastDashTime = Time.time;
            animator.SetTrigger("Dash");
            playerAttack.DashAttack();
        }
    }
    public void OnDashUp()
    {
        dashing = false;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        //Stop();

        //�ִϸ����Ϳ� ���� ������ ����Ǿ� �۵��ϵ��� ó���մϴ�.
        if (animator)
        {
            //�̵� ����(����)
            float back = 1;
            if (v < 0f)
                back = -1f;

            animator.SetFloat("Speed", new Vector2(h,v).magnitude);
            //magnitude == ������ ����, ũ��

            Rigidbody rbody = GetComponent<Rigidbody>();

            //������ٵ� ����Ǿ����� ��
            if (rbody)
            {
                Vector3 speed = rbody.linearVelocity;
                speed.x = 4 * h;
                speed.z = 4 * v;
                rbody.linearVelocity = speed;

                //���� ��ȯ
                if(h != 0f || v != 0f)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
                }
            }
        }
    }

    public void Stop()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BG_Skill"))
        {
            h = 0.0f;
            v = 0.0f;
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BG_Attack00"))
        {
            h = 0.0f;
            v = 0.0f;
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BG_Combo1_1"))
        {
            h = 0.0f;
            v = 0.0f;
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BG_Combo1_2"))
        {
            h = 0.0f;
            v = 0.0f;
            return;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BG_Combo1_3"))
        {
            h = 0.0f;
            v = 0.0f;
            return;
        }
        h = 0.0f;
        v = 0.0f;
    }
}
