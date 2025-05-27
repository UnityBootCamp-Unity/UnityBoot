using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    public GameObject Character; //캐릭터
    public GameObject Platform; //발판
    public GameObject Camera; // 캐릭터를 따라다니는 카메라
    public GameObject Menu; // 게임 시작 전 메뉴 UI
    public GameObject GameOverMenu; // 게임 오버 시 나타나는 UI
    public Transform Platform_Parents; //발판 모아줄 위치

    public int platform_pos_idx = 0; //발판 위치에 대한 인덱스 값
    public int character_pos_idx = 0; //캐릭터 위치에 대한 인덱스 값
    public bool isPlaying = false; //실행 유무

    //플랫폼 리스트(배치되어있는 판)
    private List<GameObject> Platform_List = new List<GameObject>();

    //플랫폼에 대한 체크 리스트 (발판의 위치)
    private List<int> Platform_Check_List = new List<int>();

    public bool isLefting = true; // 왼쪽으로 이동 중인지 여부
    public int character_clear_stair = 0; // 캐릭터가 밟은 발판의 개수 (20개씩 확인)
    public int check_clear_bundle = 0; // 60 계단을 20씩 나누어서 확인

    private void Start()
    {
        Menu.gameObject.SetActive(true); // 메뉴 UI 활성화
        SetFlatform(); //발판 설정
        Init(); //게임 초기화
        isLefting = true;
    }

    private void Update()
    {
        //플레이 모드라면
        if (isPlaying)
        {
            if (character_pos_idx == 60)
                character_pos_idx = 0; // 60번째 발판을 밟았을 때 다시 0으로 초기화

            //현재의 기능은 테스트용으로, 버튼을 통한 클릭으로 변경할 예정
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.Move();
                if (isLefting)
                {
                    StartCoroutine(DelayedCheck_Platform(character_pos_idx, 0));
                }
                else
                {
                    StartCoroutine(DelayedCheck_Platform(character_pos_idx, 1));
                }
                //Check_Platform(character_pos_idx, 1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.MoveTurn();
                if (isLefting)
                {
                    StartCoroutine(DelayedCheck_Platform(character_pos_idx, 1));
                    isLefting = false;
                }
                else
                {
                    StartCoroutine(DelayedCheck_Platform(character_pos_idx, 0));
                    isLefting = true;
                }
                //Check_Platform(character_pos_idx, 0);
            }
        }
    }

    private void Check_Platform(int idx, int direction)
    {
        //[체크 확인용 코드]
        Debug.Log("d인덱스 값 : " + idx + "플랫폼 : " + Platform_Check_List[idx] + "/방향 : " + direction);

        //방향이 맞을 경우
        if (Platform_Check_List[idx % 60] == direction)
        {
            
            //캐릭터의 위치 변경
            character_pos_idx++;


            //Character.transform.position = Platform_List[idx].transform.position
            //    + new Vector3(0f, 0.4f, 0f);
            Vector3 character_pos = Character.transform.position; // 캐릭터의 위치
            character_pos.x = Platform_List[idx].transform.position.x + 0.05f;
            character_pos.y += 0.3f;
            Character.transform.position = character_pos; // 캐릭터 위치 변경 적용

            character_clear_stair++; // 캐릭터가 발판을 밟았을 때마다 카운트 증가
            PlayerClear20Flatform(); // 20개의 발판을 넘겼는지 확인
            //바닥 설정
            //NextPlatform(platform_pos_idx);

        }
        else
        {
            GameOver();
            Camera.gameObject.SetActive(false); // 카메라 비활성화
        }
    }

    private void GameOver()
    {
        Debug.Log("게임 오버"); //실제로는 UI의 게임 오버에 대한 패널 활성화
                                //스코어 갱신
        isPlaying = false;
        player.Die(); // 캐릭터 사망 애니메이션 실행
        StartCoroutine(DelayedGameOverMenu()); // 게임 오버 메뉴 활성화 지연 실행
    }

    private void Init()
    {
        Character.transform.position = new Vector3(0.15f,-0.55f, 0);

        for (platform_pos_idx = 0; platform_pos_idx < 60; platform_pos_idx++)
        {
            NextPlatform(platform_pos_idx);
        }

        character_pos_idx = 0;
        //isPlaying = true; //이후에는 플레이 버튼을 눌렀을 때 진행되도록 수정해줘야 합니다.
        player.Idle(); // 캐릭터 초기화

        isPlaying = false; // 게임 시작 전에는 플레이 상태가 아니므로 false로 설정
    }

    private void NextPlatform(int idx)
    {
        int pos = UnityEngine.Random.Range(0, 2);

        if (idx == 0)
        {
            //Platform_Check_List[idx] = pos;
            Platform_List[idx].transform.position = new Vector3(-0.4f, -1.45f, 0);
        }
        else
        {
            if(platform_pos_idx < 60)
            {
                if (pos == 0)
                {
                    Platform_Check_List[idx] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-0.55f, 0.3f, 0);
                }
                else
                {
                    Platform_Check_List[idx] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(0.55f, 0.3f, 0);
                }
            }
            /*else //인덱스 범위를 넘은 경우
            {
                //왼쪽 발판
                if (pos == 0)
                {
                    if(idx % 50 == 0)
                    {
                        Platform_Check_List[49] = pos;
                        Platform_List[idx % 50].transform.position = Platform_List[49].transform.position + new Vector3(-0.55f, 0.3f, 0);
                    }
                    else
                    {
                        Platform_Check_List[idx % 50] = pos;
                        Platform_List[idx % 50].transform.position = Platform_List[idx % 50].transform.position + new Vector3(-0.55f, 0.3f, 0);
                    }
                    
                }
                //오른쪽 발판
                else
                {
                    if (idx % 50 == 0)
                    {
                        Platform_Check_List[49] = pos;
                        Platform_List[idx % 50].transform.position = Platform_List[49].transform.position + new Vector3(0.55f, 0.3f, 0);
                    }
                    else
                    {
                        Platform_Check_List[idx % 50] = pos;
                        Platform_List[idx % 50].transform.position = Platform_List[idx % 50].transform.position + new Vector3(0.55f, 0.3f, 0);
                    }
                }
            }*/
        }
        //platform_pos_idx++;
    }

    private void SetFlatform()
    {
        //임의의 숫자만큼 플랫폼 생성
        for(int i = 0; i < 60; i++)
        {
            GameObject plat = Instantiate(Platform, Vector3.zero, Quaternion.identity, Platform_Parents);
            Platform_List.Add(plat);
            Platform_Check_List.Add(0);
        }
    }

    // 게임 진행 도중 20개의 발판을 넘길경우 맨 아래꺼를 맨 위로 옮기고 추가적인 스테이지 난이도 업
    private void PlayerClear20Flatform()
    {
        if (character_clear_stair <= 20)
            return;

        character_clear_stair = 0;
        int idx = 0;

        if (check_clear_bundle == 0)
        {

            for (idx = 0; idx < 20; idx++)
            {
                int pos = UnityEngine.Random.Range(0, 2);
                Platform_Check_List[idx] = pos;

                if (idx == 0)
                {
                    if (pos == 0)
                        Platform_List[idx].transform.position = Platform_List[59].transform.position + new Vector3(-0.55f, 0.3f, 0);
                    else
                        Platform_List[idx].transform.position = Platform_List[59].transform.position + new Vector3(0.55f, 0.3f, 0);
                }
                else
                {
                    if (pos == 0)
                        Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-0.55f, 0.3f, 0);
                    else
                        Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(0.55f, 0.3f, 0);
                }
            }

            check_clear_bundle = 1; // 20개를 넘겼으니 다음 단계로
        }
        else if (check_clear_bundle == 1)
        {
            for (idx = 20; idx < 40; idx++)
            {
                int pos = UnityEngine.Random.Range(0, 2);
                Platform_Check_List[idx] = pos;

                if (pos == 0)
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-0.55f, 0.3f, 0);
                else
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(0.55f, 0.3f, 0);
            }
            check_clear_bundle = 2; // 40개를 넘겼으니 다음 단계로
        }
        else if (check_clear_bundle == 2)
        {
            for (idx = 40; idx < 60; idx++)
            {
                int pos = UnityEngine.Random.Range(0, 2);
                Platform_Check_List[idx] = pos;

                if (pos == 0)
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-0.55f, 0.3f, 0);
                else
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(0.55f, 0.3f, 0);
            }
            check_clear_bundle = 0; // 60개를 넘겼으니 다음 단계로
        }
    }

    public void OnClickStartButton()
    {
        isPlaying = true; // 게임 시작
        Menu.gameObject.SetActive(false); // 메뉴 UI 비활성화

    }

    public void OnClickReStartButton()
    {
        GameOverMenu.gameObject.SetActive(false); // 게임 오버 메뉴 비활성화
        Camera.gameObject.SetActive(true); // 카메라 활성화
        player.Idle(); // 캐릭터 초기화
        isLefting = true; // 왼쪽으로 이동 상태 초기화
        character_clear_stair = 0;
        check_clear_bundle = 0;
        Init(); // 게임 재시작
        isPlaying = true; // 게임 시작
    }

    public void OnClickReMenuButton()
    {
        GameOverMenu.gameObject.SetActive(false); // 게임 오버 메뉴 비활성화
        Camera.gameObject.SetActive(true); // 카메라 활성화
        player.Idle(); // 캐릭터 초기화
        isLefting = true; // 왼쪽으로 이동 상태 초기화
        character_clear_stair = 0;
        check_clear_bundle = 0;
        Init(); // 게임 재시작
        Menu.gameObject.SetActive(true); // 메뉴 UI 활성화
    }


    private IEnumerator DelayedCheck_Platform(int idx, int direction)
    {
        yield return new WaitForSeconds(0.1f);
        Check_Platform(idx, direction);
    }

    private IEnumerator DelayedGameOverMenu()
    {
        yield return new WaitForSeconds(3f); // 1초 후에 게임 오버 메뉴 활성화
        GameOverMenu.gameObject.SetActive(true); // 게임 오버 메뉴 활성화
    }

}
