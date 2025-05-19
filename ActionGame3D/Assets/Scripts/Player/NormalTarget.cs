using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    //���� Ÿ�ٿ� ���� ����Ʈ
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>();
    }

    //���Ͱ� ���� �ݰ����� ������, ����Ʈ �߰�
    private void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Add(other);
    }

    //���Ͱ� ���� �ݰ濡�� ����� ����Ʈ ����
    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Remove(other);
    }

    //���� ����(Update) ���Ŀ� �߸��� ������ ���� ��ȿ�� �˻�
    //���� ������ ����, Ÿ�� �˻��� ���� ������, ����Ƽ ���� �� �ݶ��̴��� ������Ʈ ���� ������Ʈ����
    //Destroy �� �� �̺�Ʈ �Լ��� ȣ���� �ȵǴ� Ÿ�̹��� ������ �� ����
    private void LateUpdate()
    {
        targetList.RemoveAll(target => target == null);
        //����Ʈ ����
        //1. targetList.Contains(value)
        // ����Ʈ�� �ش� ���� ���Եǰ� �ִ����� Ȯ���մϴ�.

        //2. targetList.Remove(value);
        //����Ʈ�� �ش� ���� �����մϴ�.

        //3. targetList.Add(value);
        //����Ʈ�� �ش� ���� �߰��մϴ�. �߰��� ���� ����Ʈ�� ������ ��

        //4. targetList.RemoveAll(Predicate<T>);
        //���� ���ǿ� ���� �븮�ڸ� �ְ�, ����Ʈ�� ���� ��ü ���Ÿ� �����մϴ�.
        //���� �ش� �ڵ��� ������ �ݶ��̴��� target�� null ����������
        //üũ�ϴ� �ڵ��Դϴ�. null�� ��� �׺��� ���ŵ˴ϴ�.
    }
}
