using UnityEngine;

public class DragSelectionBox : MonoBehaviour
{
    public RectTransform selectionBox; //선택 박스 RectTransform
    private Vector2 startPos; //드래그 시작 위치
    private Vector2 endPos; //드래그 끝 위치

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition; //드래그 시작 위치 저장
            selectionBox.gameObject.SetActive(true); //선택 박스 활성화
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition; //드래그 중 끝 위치 업데이트
            UpdateSelectionBox(); //선택 박스 업데이트
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false); //드래그 종료 시 선택 박스 비활성화
            SelectUnitsInBox(); //선택 박스 내 유닛 선택
        }
    }

    private void UpdateSelectionBox()
    {
        Vector2 size = endPos - startPos; //드래그 크기 계산
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y)); //선택 박스 크기 설정
        //selectionBox.anchoredPosition = startPos + size / 2; //선택 박스 위치 설정

        // 스크린 좌표(startPos + size / 2)를 캔버스 좌표로 변환
        Vector2 center = startPos + size / 2;
        RectTransform canvasRect = selectionBox.parent as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, center, null, out localPoint);
        selectionBox.anchoredPosition = localPoint;
    }

    private void SelectUnitsInBox()
    {
        Vector2 min = Vector2.Min(startPos, endPos); //드래그 영역의 최소 좌표
        Vector2 max = Vector2.Max(startPos, endPos); //드래그 영역의 최대 좌표

        // 기존 선택 해제
        UnitSelectionManager.instance.DeselectAllPlayers();
        UnitSelectionManager.instance.DeselectEnemy();

        foreach (var player in GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            Vector2 playerPos = Camera.main.WorldToScreenPoint(player.transform.position); //플레이어 위치를 화면 좌표로 변환
            if (playerPos.x >= min.x && playerPos.x <= max.x && playerPos.y >= min.y && playerPos.y <= max.y)
            {
                UnitSelectionManager.instance.SelectPlayer(player); //드래그 영역 내의 플레이어 선택
            }
        }
    }
}
