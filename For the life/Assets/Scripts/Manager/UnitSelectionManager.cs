using System.Collections.Generic;
using UnityEngine;


public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager instance; //싱글톤 인스턴스

    public List<Player> selectedPlayers = new List<Player>(); //선택된 플레이어 목록

    public Enemy selectedEnemy;  // 적 선택도 관리

    [Header("유닛이 있는 레이어")]
    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    /*//드래그 선택용 변수
    private Vector2 dragStartPos; //드래그 시작 위치
    private Vector2 dragEndPos; //드래그 끝 위치
    private bool isDragging = false; //드래그 중인지 여부*/

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; //싱글톤 인스턴스 설정
        }
        else
        {
            Destroy(gameObject); //이미 인스턴스가 존재하면 현재 오브젝트 삭제
        }
    }
    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        /*//드래그 시작 (왼쪽 클릭 Down)
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = mousePos; //드래그 시작 위치 저장
            isDragging = true; //드래그 시작
        }
        //드래그 종료 (왼쪽 클릭 Up)
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false; //드래그 종료
            dragEndPos = mousePos; //드래그 끝 위치 저장

            //드래그 영역 내의 플레이어 선택
            SelectPlayersInDragRect(dragStartPos, dragEndPos);

            //적 선택 해제 (드래그 시 적은 선택 해제)
            DeselectEnemy();
        }*/

        //마우스 왼쪽 클릭 (선택 or 선택 해제)
        if (Input.GetMouseButtonDown(0))
        {

            //클릭 시 Player 레이어에서 Raycast를 사용하여 유닛을 선택
            RaycastHit2D hitPlayer = Physics2D.Raycast(mousePos, Vector2.zero, 0f, playerLayer);

            //Player 클릭 시 선택
            if (hitPlayer.collider != null)
            {
                Player clickedPlayer = hitPlayer.collider.GetComponent<Player>(); //Player 컴포넌트 가져오기
                if (clickedPlayer != null)
                {
                    DeselectAllPlayers();
                    DeselectEnemy();

                    // 만약 이미 선택된 플레이어가 있다면, 해당 플레이어를 선택 해제하고 새로 클릭한 플레이어를 선택
                    SelectPlayer(clickedPlayer);
                    return;
                }
            }

            //Enemy 클릭 시 선택
            RaycastHit2D hitEnemy = Physics2D.Raycast(mousePos, Vector2.zero, 0f, enemyLayer);
            if (hitEnemy.collider != null)
            {
                Enemy clickedEnemy = hitEnemy.collider.GetComponent<Enemy>();
                if (clickedEnemy != null)
                {
                    DeselectAllPlayers();
                    DeselectEnemy();

                    // 만약 이미 선택된 적이 있다면, 해당 적을 선택 해제하고 새로 클릭한 적을 선택
                    SelectEnemy(clickedEnemy);
                    return;
                }
            }

            // 아무것도 클릭 안하면 모든 선택 해제
            DeselectAllPlayers();
            DeselectEnemy();
            
        }


        if (!DefenseManager.instance.isTurning)
            return;

        // 아래꺼 참고해서 유닛 공격, 정찰, 스탑 관련 넣기

        /// <summary>
        /// 수학적 계산...
        /// </summary>
        //오른쪽 클릭 시 Player만 이동 (다중)
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedPlayers.Count > 0)
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                /*foreach (Player player in selectedPlayers)
                {
                    player.MoveTo(mouseWorldPos); //선택된 모든 플레이어 이동
                }*/

                //이동할 때 뭉치는 게 아니라 퍼트리기
                //1. 유닛 수 파악
                int unitCount = selectedPlayers.Count;

                //2. 몇 줄 몇 칸으로 배치할지 계산 (정사각형 형태에 가까운 격자)
                int columns = Mathf.CeilToInt(Mathf.Sqrt(unitCount)); //열 수
                int rows = Mathf.CeilToInt((float)unitCount / columns); //행 수

                //3. 격자 전체 크기 조정 (각 유닛 사이 간격)
                float spacing = 0.5f + (0.2f * unitCount); //유닛 간 거리 (조정 가능) //유닛 수가 많을수록 약간 더 넓게
                float gridWidth = (columns -1) * spacing;
                float gridHeight = (rows - 1) * spacing;

                //4. 시작점 좌표 계산 (격자의 좌상단)
                Vector2 startPos = mouseWorldPos - new Vector2(gridWidth / 2f, gridHeight / 2f);

                //5. 유닛들에게 하나씩 위치 배정
                for (int i = 0; i < unitCount; i++)
                {
                    int row = i / columns; //현재 행
                    int column = i % columns; //현재 열
                                              //격자 내 위치 계산
                                              //Vector2 targetPosition = startPos + new Vector2(column * spacing, row * spacing);
                                              // ✅ 위에서 아래로 (y축은 감소)
                    Vector2 offset = new Vector2(column * spacing, -row * spacing);
                    Vector2 targetPosition = startPos + offset;

                    selectedPlayers[i].MoveTo(targetPosition); //각 플레이어를 해당 위치로 이동
                }
            }
            //Enemy는 이동 불가 → 무시
        }
    }

    /*//드래그 영역 내의 플레이어 선택
    private void SelectPlayersInDragRect(Vector2 start, Vector2 end)
    {
        DeselectAllPlayers(); //기존 선택된 플레이어 모두 해제

        float minX = Mathf.Min(start.x, end.x);
        float maxX = Mathf.Max(start.x, end.x);
        float minY = Mathf.Min(start.y, end.y);
        float maxY = Mathf.Max(start.y, end.y);

        //FindObjectsByType : 항상 InstanceID 기준으로 정렬하기에 성능이 낭비된다
        //따라서 FindObjectsSortMode.None 옵션을 사용하여 정렬하지 않도록 한다.
        Player[] allPlayers = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None); //씬 내 모든 플레이어 찾기
        
        foreach (var player in allPlayers)
        {
            Vector2 playerPos = player.transform.position;
            //플레이어가 드래그 영역 내에 있는지 확인
            if (playerPos.x >= minX && playerPos.x <= maxX && playerPos.y >= minY && playerPos.y <= maxY)
            {
                SelectPlayer(player); //드래그 영역 내의 플레이어 선택
            }
        }
    }*/

    //Player 선택 (다중)
    public void SelectPlayer(Player unit)
    {
        if(!selectedPlayers.Contains(unit)) //이미 선택된 플레이어가 아니라면
        {
            selectedPlayers.Add(unit); //선택된 플레이어 목록에 추가
            unit.SetSelected(true); //플레이어 선택 상태 설정
            Debug.Log("Selected Player: " + unit.name);
        }
    }

    //Enemy 선택 (단일)
    public void SelectEnemy(Enemy enemy)
    {
        if (selectedEnemy != null && selectedEnemy != enemy)
            selectedEnemy.SetSelected(false);

        selectedEnemy = enemy;
        selectedEnemy.SetSelected(true);
        Debug.Log("Selected Enemy: " + selectedEnemy.name);
    }

    //플레이어 전체 선택 해제
    public void DeselectAllPlayers()
    {
        foreach (var p in selectedPlayers)
            p.SetSelected(false); //모든 선택된 플레이어 선택 해제
        selectedPlayers.Clear(); //선택된 플레이어 목록 초기화
        Debug.Log("Deselected All Players.");
    }

    //Enemy 단일 선택 해제
    public void DeselectEnemy()
    {
        if (selectedEnemy != null)
        {
            selectedEnemy.SetSelected(false);
            selectedEnemy = null;
            Debug.Log("Deselected Enemy.");
        }
    }

}