using System.Collections;
using UnityEngine;

public class BossBullet1 : MonoBehaviour
{
    private float speed = 2.0f;
    public float rotateSpeed = 180f; // ȸ�� �ӵ�
    public float directionLerpSpeed = 1.0f; // �̵� ���� ���� �ӵ�

    Vector3 dir = Vector3.zero;
    private bool targeting = false;

    [Header("���� �� ����")]
    public Boss boss;
    public GameObject effect;

    void OnEnable()
    {
        if(transform.position.x > boss.transform.position.x)
        {
            // �÷��̾ ���� �����ʿ� ������ ������ �̻��ϸ� ���
            dir = Vector3.right;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else
        {
            // �÷��̾ ���� ���ʿ� ������ ���� �̻��ϸ� ���
            dir = Vector3.left;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }


        transform.position += dir * 1.0f * Time.deltaTime;
        StartCoroutine(TargetOn());
    }

    private void Update()
    {
        if (targeting)
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            if (target == null)
            {
                dir = transform.up;
                transform.position += dir * speed * Time.deltaTime;
                return;
            }


            Vector3 targetDir = target.transform.position - transform.position;

            targetDir.Normalize();
            //targetDir.y = -1;

            // �Ѿ��� �÷��̾ ���� ȸ��
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, rotation, rotateSpeed * Time.deltaTime );

            dir = Vector3.Lerp(dir, targetDir, directionLerpSpeed * Time.deltaTime);
                
            StartCoroutine(TargetOff());
        }


        transform.position += dir * speed * Time.deltaTime;
    }

    //�浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        //ScoreManager.Instance.Score++;

        if (collision.gameObject.tag.Contains("Player"))
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                var explosion = Instantiate(effect);
                explosion.transform.position = transform.position;
            }
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }

    IEnumerator TargetOn()
    {
        yield return new WaitForSeconds(0.2f);
        targeting = true;
    }

    IEnumerator TargetOff()
    {
        yield return new WaitForSeconds(3.0f);
        targeting = false;
        dir = transform.up;
    }
}
