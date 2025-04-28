using System.Collections;
using UnityEngine;

//������Ʈ(Component)
//����Ƽ ������Ʈ�� ����� ���
//�����Ǵ� ������Ʈ�� �ְ�, ��ũ��Ʈ�� ���� ����ڰ� ������ִ� ����� ����
//������Ʈ�ν� Ȱ���� �����մϴ�.(Mono ���)

//MonoBehavior ���
//1. ����Ƽ ������Ʈ�� �ش� Ŭ������ ������Ʈ�ν� ����� �� �ֽ��ϴ�.



public class Monster : Unit
{
    //����Ƽ �ν����Ϳ��ش� �ʵ� ���� ���� ���� ����
    [Range(1,5)] public float speed;

    /* Animator animator;*/

    //���� Ŭ�������� ��Ȳ�� �°� �ִϸ��̼��� �����Ű�� �մϴ�.
    //�̶� �ʿ��� �����ʹ� �����ϱ��?
    //1. Animation
    //2. Animator (����)

    bool isSpawn = false; //���� ����

    //���Ͱ� ���������� ������ �۾�(����)
    //������ Ŀ���� ����
    IEnumerator OnSpawn()
    {
        float current = 0.0f; //������ �� �����
        float percent = 0.0f; //�ݺ����� ���� ����
        float start = 0.0f; //��ȭ ���� ��
        float end = transform.localScale.x; // ��ȭ ������ ��

        //localScale�� ���� ������Ʈ�� ������� ũ�⸦ �ǹ��մϴ�.
        //����� ������Ʈ�� ũ��� ����մϴ�.

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 3.0f;

            //start���� end �������� percent �������� �̵��ض�.
            var pos = Mathf.Lerp(start, end, percent);

            //����� ��ġ��ŭ ������(ũ��)�� �����մϴ�.
            transform.localScale = new Vector3(pos, pos, pos);
            //Ż���ߴٰ� ���ƿɴϴ�.
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        isSpawn = true;
    }

    protected override void Start()
    {
        base.Start(); //Unity�� Start ȣ��
        //Monster�� ������ Start �۾� ����
        //MonsterInit();

        //�⺻ ü���� 5�� �����Ѵ�.
        HP = 5.0f;
        GetDamage(5.0f);
    }

    public GameObject effect; //����Ʈ ����

    public void GetDamage(double dmg)
    {
        HP -= dmg;//������ ü���� ��������ŭ ��´�.
        if (HP <= 0)
        {
            var eff = Resources.Load<GameObject>(effect.name);
            //����� ����Ʈ�� �̸����� �ε��Ѵ�.
            Instantiate(eff, transform.position, Quaternion.identity);
            //�ε��� ���� �����Ѵ�.

            /*//����Ʈ�� ������ ��ǥ ��ġ�� ����
            var effect = Manager.Pool.pooling("Effect01").get(
                (value) =>
                {
                    value.transform.position = new Vector3(transform.position.x,
                        transform.position.y, transform.position.z);
                });*/
        }
    }

    public void MonsterInit() => StartCoroutine(OnSpawn());


    /*private void Start()
    {
        *//*animator = GetComponent<Animator>();
        //�ڵ� ������ Animator�� �ν��ϰ�, Animator�� �ʵ峪 �޼ҵ带
        //����� �� �ֽ��ϴ�.*//*
        StartCoroutine(OnSpawn());
    }*/

    //����Ƽ ������ ����Ŭ �Լ� // �����Ӹ��� �ѹ��� �ݺ�
    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (isSpawn == false)
            return;

        var distance = Vector3.Distance(transform.position, Vector3.zero);
        //������ ���غ��� ���� �Ÿ��� ������
        if (distance <= 0.5f)
        {
            SetAnimator("IsIDLE"); //������ �����մϴ�.
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,Vector3.zero, Time.deltaTime * speed);
            SetAnimator("IsMOVE"); //�̵� ���� �����մϴ�.
        }

        #region �ʱ�
        //1. transform.position : ���� ������Ʈ�� ��ġ�� ��Ÿ���ϴ�.
        //2. Vector3 : 3D ȯ���� ��ǥ�� (X, Y, Z ��) ����
        //3. MoveTowrads(start, end, speed); start���� end �������� speed ��ġ��ŭ �̵��մϴ�.
        //4. Time.deltaTime : ���� �������� �Ϸ�Ǳ���� �ɸ� �ð�
        //                    (��ǻ���� ������ �������� ���� Ŀ��)
        //                    �Ϲ������� �� 1��
        //                    ������Ʈ���� �۾��� �ϴµ� �־�� ���� �� ����
        //5. transform.LookAt(Vector3.zero) : Ư�� ������ �ٶ󺸰� �������ִ� ���

        //���� ���� : �⺻������ �������ִ� Vector ��
        //Vector3.right == new Vector3(1,0, 0); // ������ ������ ��Ÿ���ϴ�.
        //Vector3.left == new Vector3(-1,0, 0); // ���� ������ ��Ÿ���ϴ�.
        //Vector3.up == new Vector3(0,1, 0); // ���� ������ ��Ÿ���ϴ�.
        //Vector3.down == new Vector3(0,-1, 0); // �Ʒ��� ������ ��Ÿ���ϴ�.
        //Vector3.forward == new Vector3(0, 0, 1); // ���� ������ ��Ÿ���ϴ�.
        //Vector3.back == new Vector3(0, 0, -1); // ���� ������ ��Ÿ���ϴ�.
        //Vector3.zero == new Vecotr3(0, 0, 0); // 0,0,0 ��ǥ�� ��Ÿ���ϴ�.
        //Vector3.one == new Vecotr3(1, 1, 1); // 1,1,1 ��ǥ�� ��Ÿ���ϴ�.
        #endregion
    }

/*    private void SetAnimator(string temp)
    {
        //�⺻ �Ķ���Ϳ� ���� reset
        //����Ƽ Animator�� ������ parameter�� �̸��� ��Ȯ�ϰ� �����մϴ�.
        animator.SetBool("IsIDLE", false);
        animator.SetBool("IsMOVE", false);

        //���ڷ� ���޹��� ���� true�� �����մϴ�.
        animator.SetBool(temp, true);
    }*/
}
