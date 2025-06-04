using TMPro;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject nightBackground1; //ù ��° �߰� ���
    public GameObject nightBackground2; //�� ��° �߰� ���
    public GameObject nightBackground3; //�� ��° �߰� ���

    public GameObject SunsetBackground; //���� ���

    public float fadeUpSpeed = 0.08f; //���̵� �� �ӵ�

    private void Start()
    {
        // �ʱ� ��� ����
        SetNightBackground1();
    }

    private void Update()
    {
        if (!DefenseManager.instance.isTurning)
        {
            if (DefenseManager.instance.turnCount >= 10)
            {
                // �� ��° �߰� ��� Ȱ��ȭ
                SunsetBackground.SetActive(true);

                // ���� ��ġ ����, y�� ��ǥ������
                Vector3 currentPos = SunsetBackground.transform.position;
                Vector3 targetPosition = new Vector3(currentPos.x, 5.75f, currentPos.z);

                // y�� �������θ� �̵�
                SunsetBackground.transform.position = Vector3.MoveTowards(
                    currentPos,
                    targetPosition,
                    fadeUpSpeed * Time.deltaTime
                );

                // �����ߴ��� üũ
                if (SunsetBackground.transform.position.y >= 5.75f)
                {
                    // ��Ȯ�ϰ� ��ġ ����
                    SunsetBackground.transform.position = targetPosition;

                    // ù ��° �߰� ��� ��Ȱ��ȭ
                    nightBackground1.SetActive(false);
                    nightBackground2.SetActive(false);
                    nightBackground3.SetActive(false);
                }
            }
        }
    }

    public void SetNightBackground1()
    {
        // ù ��° �߰� ��� Ȱ��ȭ
        nightBackground1.SetActive(true);
        nightBackground2.SetActive(true);
        nightBackground3.SetActive(true);
        SunsetBackground.SetActive(false);
        // ��ġ �ʱ�ȭ
        SunsetBackground.transform.position = new Vector3(-12.03f, -2.64f, 90.00f);
    }
}
