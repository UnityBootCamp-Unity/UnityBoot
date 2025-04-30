using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Item_Object : MonoBehaviour
{
    public Transform ItemText;
    public Text text; //TMP ���ô� �е��� TMP�� ����

    public float angle = 45.0f;
    public float gravity = 9.8f;
    public float range = 2.0f;

    bool ischeck = false;

    //������ ��� ���� ó���ϴ� �ڵ�
     void ItemRare()
    {
        ischeck = true;
        transform.rotation = Quaternion.identity; //�״�� �� �Ѿ���� ȸ�� �� 0
        //������ �ؽ�Ʈ Ȱ��ȭ
        ItemText.gameObject.SetActive(true);
        ItemText.parent = B_Canvas.instance.GetLayer(2);
        text.text = "������"; //������ �̸� ����
    }

    private void Update()
    {
        if (ischeck == false)
            return;

        ItemText.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        //���޹��� ���� �������� �� �ֺ��� ��ġ�� �� �ֵ��� ���� ����
        Vector3 item_pos = new Vector3
            (
            pos.x + (Random.insideUnitSphere.x * range),
            0.0f,   //���� ��ġ
            pos.z + (Random.insideUnitSphere.z * range)
            );
        //�� ����� ���� ���� ��� �� �������� �۾� ����
        //��ü �̵� ����
        StartCoroutine(Simulate(pos));
    }

    IEnumerator Simulate(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);
        float radian = angle * Mathf.Deg2Rad; //���� ��ȯ ��
        float velocity = Mathf.Sqrt(target_Distance * gravity / Mathf.Sin(2 * radian));

        float vx = velocity * Mathf.Cos(radian);
        float vy = velocity * Mathf.Sin(radian);

        float duration = target_Distance / vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);
        //LookAt ó�� ȸ�� ���� �ٶ󺸰� ����� �ڵ�

        float simulate_time = 0.0f;

        while(simulate_time < duration)
        {
            simulate_time += Time.deltaTime;

            //�ð��� �������� ������ ���� �Ʒ���, �غ� �������� �̵�
            transform.Translate(0, (vy - (gravity * simulate_time)), vx * Time.deltaTime);

            yield return null;
        }

        //������ �̵� �ùķ��̼��� ������ ��� üũ �� ȭ�鿡 ������ �̸� ����
        ItemRare();

    }
    /*IEnumerator Simulate(Vector3 pos)
    {
        //[Ÿ���� �Ÿ�]
        var targetDistance = Vector3.Distance(transform.position, pos);

        //��� ���� ����
        var velocity = targetDistance / (Mathf.Sin(angle * Mathf.Deg2Rad) / gravity);

        //Mathf.Sin : �ﰢ�Լ� �߿��� Sin ���� ��ȯ�ϴ� ���

        //�ﰢ�� �������� ���� ���θ� ���� w,h����� �� , Sin h / a(����)�� ����ϴ� ��
        //����Ƽ���� ���� 45���� ��� ������ ���̰� 1�� �ﰢ���� ��������ϴ�.(����Ƽ ��ü ����)

        //Mathf.Sin(45 * Mathf.Deg2Rad) ==> ������ ���̰� 1�̰� ������ 45���� �ﰢ���� ����(h)�� �����մϴ�.

        //Mathf.Cos(45 * Mathf.Deg2Rad) ==> ������ ���̰� 1�̰� ������ 45���� �ﰢ���� �غ�(w)�� �����մϴ�.

        //Deg2Rad�� ��(Degree) --> ����(Radian)���� �������ִ� �ڵ�

        //��� ���� : ����Ƽ���� sin, cos �Լ��� ����� �� ���� ������ ����(radian)���� ����ϱ� ����

        //1. ���� ����� �����Ѵ�.
        //2. Ÿ���� �Ÿ��� �߷� ���� ���� ����� ���� õõ�� �����̰Բ� �Ѵ�.

        float sx = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad); //�غ� ��
        float sy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad); //���� ��

        //�ð�(�غ��� ��ġ���� �̵��� �Ÿ� ��)
        float duration = targetDistance / sx; //10

        //���� ������ ���� �ð� üũ
        float time = 0.0f;

        while(time < duration)
        {
            //�� ���� ���� ���� �������� ��ġ�� ������ŵ�ϴ�.
            transform.Translate(0, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }
    }*/
}
