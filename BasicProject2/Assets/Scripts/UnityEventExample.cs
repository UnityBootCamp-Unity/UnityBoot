using UnityEngine;
using UnityEngine.Events;

//����Ƽ���� ����� �� �ִ� �븮�� ��� �� �ϳ�

//Action�̳� Func�� C# ������ �븮��
//�� ������ �ν����Ϳ� ������� �ʽ��ϴ�.

public class UnityEventExample : MonoBehaviour
{
    public UnityEvent onSample;

    private void Update()
    {
        //onSample�� ������ ����� ��ϵ� ���¿��� A Ű�� ������ ���
        if (Input.GetKeyDown(KeyCode.A) && onSample != null)
        {
            //�� ����� �����Ѵ�.
            onSample.Invoke();
        }
    }
}
