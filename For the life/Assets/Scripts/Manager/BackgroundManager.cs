using TMPro;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject nightBackground1; //첫 번째 야경 배경
    public GameObject nightBackground2; //두 번째 야경 배경
    public GameObject nightBackground3; //세 번째 야경 배경

    public GameObject SunsetBackground; //석양 배경

    public float fadeUpSpeed = 0.08f; //페이드 업 속도

    private void Start()
    {
        // 초기 배경 설정
        SetNightBackground1();
    }

    private void Update()
    {
        if (!DefenseManager.instance.isTurning)
        {
            if (DefenseManager.instance.turnCount >= 10)
            {
                // 두 번째 야경 배경 활성화
                SunsetBackground.SetActive(true);

                // 현재 위치 유지, y만 목표값으로
                Vector3 currentPos = SunsetBackground.transform.position;
                Vector3 targetPosition = new Vector3(currentPos.x, 5.75f, currentPos.z);

                // y축 방향으로만 이동
                SunsetBackground.transform.position = Vector3.MoveTowards(
                    currentPos,
                    targetPosition,
                    fadeUpSpeed * Time.deltaTime
                );

                // 도달했는지 체크
                if (SunsetBackground.transform.position.y >= 5.75f)
                {
                    // 정확하게 위치 고정
                    SunsetBackground.transform.position = targetPosition;

                    // 첫 번째 야경 배경 비활성화
                    nightBackground1.SetActive(false);
                    nightBackground2.SetActive(false);
                    nightBackground3.SetActive(false);
                }
            }
        }
    }

    public void SetNightBackground1()
    {
        // 첫 번째 야경 배경 활성화
        nightBackground1.SetActive(true);
        nightBackground2.SetActive(true);
        nightBackground3.SetActive(true);
        SunsetBackground.SetActive(false);
        // 위치 초기화
        SunsetBackground.transform.position = new Vector3(-12.03f, -2.64f, 90.00f);
    }
}
