using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float scrollSpeed = 5f;     //마우스 휠 이동 속도
    public float minX = -22;          //최소 X 좌표 (왼쪽)
    public float maxX = -2f;           //최대 X 좌표 (오른쪽)

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            // X축으로 이동할 거리 계산
            float moveAmount = scrollInput * scrollSpeed;

            // 현재 위치에 이동 거리 적용(휠 올릴 시 왼쪽 이동)
            Vector3 newPosition = transform.position + new Vector3(-moveAmount, 0, 0);

            // 최소 / 최대 범위로 제한
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // 실제 이동
            transform.position = newPosition;
        }
    }
}
