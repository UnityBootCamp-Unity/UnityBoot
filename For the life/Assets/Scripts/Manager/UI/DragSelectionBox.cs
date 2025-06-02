using UnityEngine;

public class DragSelectionBox : MonoBehaviour
{
    public RectTransform selectionBox; //���� �ڽ� RectTransform
    private Vector2 startPos; //�巡�� ���� ��ġ
    private Vector2 endPos; //�巡�� �� ��ġ

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition; //�巡�� ���� ��ġ ����
            selectionBox.gameObject.SetActive(true); //���� �ڽ� Ȱ��ȭ
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition; //�巡�� �� �� ��ġ ������Ʈ
            UpdateSelectionBox(); //���� �ڽ� ������Ʈ
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false); //�巡�� ���� �� ���� �ڽ� ��Ȱ��ȭ
            SelectUnitsInBox(); //���� �ڽ� �� ���� ����
        }
    }

    private void UpdateSelectionBox()
    {
        Vector2 size = endPos - startPos; //�巡�� ũ�� ���
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y)); //���� �ڽ� ũ�� ����
        //selectionBox.anchoredPosition = startPos + size / 2; //���� �ڽ� ��ġ ����

        // ��ũ�� ��ǥ(startPos + size / 2)�� ĵ���� ��ǥ�� ��ȯ
        Vector2 center = startPos + size / 2;
        RectTransform canvasRect = selectionBox.parent as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, center, null, out localPoint);
        selectionBox.anchoredPosition = localPoint;
    }

    private void SelectUnitsInBox()
    {
        Vector2 min = Vector2.Min(startPos, endPos); //�巡�� ������ �ּ� ��ǥ
        Vector2 max = Vector2.Max(startPos, endPos); //�巡�� ������ �ִ� ��ǥ

        // ���� ���� ����
        UnitSelectionManager.instance.DeselectAllPlayers();
        UnitSelectionManager.instance.DeselectEnemy();

        foreach (var player in GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            Vector2 playerPos = Camera.main.WorldToScreenPoint(player.transform.position); //�÷��̾� ��ġ�� ȭ�� ��ǥ�� ��ȯ
            if (playerPos.x >= min.x && playerPos.x <= max.x && playerPos.y >= min.y && playerPos.y <= max.y)
            {
                UnitSelectionManager.instance.SelectPlayer(player); //�巡�� ���� ���� �÷��̾� ����
            }
        }
    }
}
