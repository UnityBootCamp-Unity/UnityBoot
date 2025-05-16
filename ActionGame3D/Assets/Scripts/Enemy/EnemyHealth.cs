using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;    //���� ü��
    public int currentHealth;           //���� ü��

    public float flashSpeed = 5.0f;             //�� ���� �ð�
    public Color flashColor = new Color(1, 0, 0, 0.1f); //������
    public float sinkSpeed = 1.0f;             //�������� ������ �Ʒ��� ������� �ӵ�
    public Color slimeColor = new Color(126, 255, 0, 255); // ������ ���� (������)

    public bool isDead, isSinking, damaged; //�� ���¿� ���� bool ��


    private void Awake()
    {
        currentHealth = startingHealth;
    }

    //�������� ���� ���� ����
    private void Update()
    {
        //������ ó���� ���� �������� ���� �����ϴ� �ڵ�
        if (damaged)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", flashColor);
            //transform.GetChild(0).GetComponent<Renderer>().material.color = flashColor;
        }
        else
        {
            //transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            transform.GetChild(0).GetComponent<Renderer>().
                material.SetColor("_Color",
                Color.Lerp(transform.GetChild(0).
                GetComponent<Renderer>().
                material.GetColor("_Color"), Color.green, flashSpeed * Time.deltaTime));
        }

        damaged = false;

        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        if(currentHealth <=0 && !isDead)
        {
            Death();
        }
    }

    //�������� ������ ������, �÷��̾�κ��� ƨ�ܳ����� ȿ�� ����
    public IEnumerator StartDamage(int damage, Vector3 playerPosition, float delay, float pushback)
    {
        yield return new WaitForSeconds(delay);

        try
        {
            //������ �Լ� ����
            TakeDamage(damage);

            //�÷��̾�κ��� �־��� ���� ���
            Vector3 diff = (playerPosition - transform.position).normalized;
            //diff = diff / diff.sqrMagnitude;

            //AddForce(��ġ, ���):�� ���� �������� ���� ���մϴ�.
            GetComponent<Rigidbody>().AddForce(diff * 50f * pushback, ForceMode.Impulse);
        }
        catch(MissingReferenceException e) //��ü ������ ��ȿ���� ���� ��Ȳ�� ���� �ȳ���
        {
            Debug.Log(e.ToString()); //���� �޼����� ����׷� �����.
        }
    }

    private void Death()
    {
        isDead = true;
        //�׾��� ��, ���� �հ� �Ʒ��� ����ɴ� ó���� �����ϱ� ���� ó��
        transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        StartSinking();
    }

    private void StartSinking()
    {
        //����Ƽ������ �׺�޽� ������ ���ڽ��ϴ�.
        //�ʰ� ��ֹ� ���� �Ǵ��Ͽ�, �÷��̾� �������� ������ �� ����� ������Ʈ
        GetComponent<NavMeshAgent>().enabled = false;
        //���� ���꿡 ���� ó���� ���� �ʰڽ��ϴ�.
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2.0f);
    }
}
