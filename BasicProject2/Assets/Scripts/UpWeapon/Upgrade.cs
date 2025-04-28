using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Upgrade : Weapon
{
    public UnityEvent OnUpgrade; // ���׷��̵� �̺�Ʈ
    public Text upgrade_level_text; // ���׷��̵� ���� �ؽ�Ʈ
    public Text upgrade_probability_text; // ���׷��̵� Ȯ�� �ؽ�Ʈ
    public Text message_current;
    public Text message_all_probability; // ���׷��̵� ��ü Ȯ�� �ؽ�Ʈ

    public int upgrade_level = 0; // ���׷��̵� ����
    public int max_upgrade_level = 20; // �ִ� ���׷��̵� ����

    void SuccessUpgrade() => upgrade_level++; // ���׷��̵� ���� �� ���� ����

    void FailUpgrade()
    {
        if (upgrade_level < 10)
            upgrade_level--; // ���׷��̵� ���� �� ���� ����
        else if (upgrade_level < 18)
            upgrade_level -= 2; // ���׷��̵� ���� �� ���� ����
        else
            upgrade_level = 0; // ���׷��̵� ���� �� ���� �ʱ�ȭ
    }

    void UpgradeText()
    {
        if (upgrade_level < 10)
            message_all_probability.text = "���� �� ���� - 1";
        else if (upgrade_level < 18)
            message_all_probability.text = "���� �� ���� - 2";
        else
            message_all_probability.text = "���� �� ���� �ʱ�ȭ";

        upgrade_level_text.text = $"{upgrade_level} ����";
        upgrade_probability_text.text = $"{(100 - (upgrade_level * 4))} %";
        message_current.text = $"[���׷��̵� ���� ��...] ���� ���� {upgrade_level} / {max_upgrade_level} ����";
    }

    void UpgradeProcess()
    {
        int upgrade_probability = Random.Range(0, 100); // ���׷��̵� Ȯ��
        if (upgrade_level < max_upgrade_level)
        {
            if (upgrade_probability >= upgrade_level * 4)
            {
                SuccessUpgrade(); // ���׷��̵� ����
                /*message_current.text = "���׷��̵� ����";
                StartCoroutine(WaitForSeconds(3.0f)); // 3�� ��� �� �޽��� �ʱ�ȭ*/
            }
            else
            {
                FailUpgrade(); // ���׷��̵� ����
                /*message_current.text = "���׷��̵� ����";
                StartCoroutine(WaitForSeconds(3.0f)); // 3�� ��� �� �޽��� �ʱ�ȭ*/
            }
        }
        else
        {
            message_current.text = "�ִ� ���� ����";
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnUpgrade.AddListener(UpgradeText); // ���׷��̵� �ؽ�Ʈ ���
        OnUpgrade.AddListener(UpgradeProcess); // ���׷��̵� ���μ��� ���
    }

    // Update is called once per frame
    void Update()
    {
        if (OnUpgrade != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnUpgrade.Invoke();
            }
        }
        else
        {
            Debug.Log("��ϵ� �񿵱� �����ʰ� �����ϴ�.");
            //�񿵱� ������ : ��ũ��Ʈ�� ���ؼ� (AddListener)�� ���ؼ� �߰��� ������
            //                RemoveListener�� ���ؼ� ���Ű� �����մϴ�.
            //���� ������ : �ν����͸� ���ؼ� �߰��� ������
            //              �̰� �ν����͸� ���� ���� ������ �մϴ�.
        }
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
