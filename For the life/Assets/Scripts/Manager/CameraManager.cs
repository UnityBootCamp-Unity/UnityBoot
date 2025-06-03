using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float scrollSpeed = 5f;     //���콺 �� �̵� �ӵ�
    public float minX = -22;          //�ּ� X ��ǥ (����)
    public float maxX = -2f;           //�ִ� X ��ǥ (������)

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            // X������ �̵��� �Ÿ� ���
            float moveAmount = scrollInput * scrollSpeed;

            // ���� ��ġ�� �̵� �Ÿ� ����(�� �ø� �� ���� �̵�)
            Vector3 newPosition = transform.position + new Vector3(-moveAmount, 0, 0);

            // �ּ� / �ִ� ������ ����
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // ���� �̵�
            transform.position = newPosition;
        }
    }
}
