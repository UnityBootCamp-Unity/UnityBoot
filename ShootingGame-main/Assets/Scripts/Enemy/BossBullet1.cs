using System.Collections;
using UnityEngine;

public class BossBullet1 : MonoBehaviour
{
    private float speed = 2.0f;
    Vector3 dir = Vector3.zero;


    void OnEnable()
    {
        var target = GameObject.FindGameObjectWithTag("Player");
        dir = target.transform.position - transform.position;

        dir.Normalize();
        dir.y = -1;

        // �Ѿ��� �÷��̷��� ���� ȸ��
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
        transform.rotation = rotation;

        transform.position += dir * speed * Time.deltaTime;
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
}
