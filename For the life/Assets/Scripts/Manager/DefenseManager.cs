using UnityEngine;

public class DefenseManager : MonoBehaviour
{
    public static DefenseManager instance; // �̱��� �ν��Ͻ�

    public bool isTurning = false; // ���� ���� ������ ����

    private void Start()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
        }
    }
}
