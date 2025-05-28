using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//배틀에서 사용할 턴에 대한 상태를 enum으로 표현
public enum BattleTurn
{
    None                //턴이 멈춘 상태
    ,Start               //시작
    ,Turn1, Turn2       //플레이어 턴, 상대 턴
    ,Action1, Action2   //플레이어의 동작, 상대의 동작
    ,Win, Lose          //승 , 패
}

//UI에서의 메뉴 화면에 대한 정보
public enum UI_State
{
    MainMenu,SKillMenu
}

public class BattleSystem : MonoBehaviour
{
    //배틀 시스템이 감지할 배틀의 상태
    //(턴마다 바뀜)
    public BattleTurn state;

    //현재 UI에 대한 상태
    public UI_State ui_state;

    //맵 가운데에서 턴 페이즈에 대한 텍스트
    public Text PhaseText;
    public Color color;

    //배틀 해설 텍스트
    public Text BattleDescriptionText;

    //텍스트 컬러변경
    public Color testColor;

    //플레이어와 상대방에 대한 등록
    public Unit playerUnit;
    public Unit otherUnit;

    public Button[] buttons; //메뉴 버튼
    public Button[] skill_buttons; //스킬에 대한 버튼

    //메뉴 온 오프
    public GameObject mainmenuPanel;
    public GameObject skillmenuPanel;

    //플레이어의 슬라이더 바 연결
    public UnitHPbar player_bar;
    public UnitHPbar other_bar;

    //현재 메뉴들에 대한 인덱스 값
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;

    //타이핑 상태인지 확인
    private bool isTyping = false;

    private void Start()
    {
        state = BattleTurn.Start;

        //슬라이더 바 설정
        player_bar.SetSliderBar(playerUnit);
        other_bar.SetSliderBar(otherUnit);

    }

    private void Update()
    {
        PaseText();
        checkedHP();
        switch (ui_state)
        {
            case UI_State.MainMenu:
                HandleMainMenuInput();
                break;
            case UI_State.SKillMenu:
                HandleSkillMenuInput();
                break;
        }
    }

    private void checkedHP()
    {
        Debug.Log(state);
        if (playerUnit.currentHP <= 0)
        {
            PhaseText.text = "";
            CloseMenu();
            state = BattleTurn.Turn1; // 플레이어 턴이 끝나면 상대 턴으로 넘어감
            StopAllCoroutines(); // 모든 코루틴 중지
            Typing.instance.StartTyping(playerUnit.name + "은(는) 당신을 슬프게 하지 않을려고 버티었다.", BattleDescriptionText);
            playerUnit.currentHP = 1;
        }
        else if(otherUnit.currentHP <= 0)
        {
            PhaseText.text = "";
            CloseMenu();
            state = BattleTurn.Win; // 플레이어가 승리한 상태로 변경
            StopAllCoroutines(); // 모든 코루틴 중지
            Typing.instance.StartTyping(otherUnit.name + "은(는) 당신에게 패배했다.", BattleDescriptionText);
            otherUnit.currentHP = 1;
        }
    }

    //스킬로 공격
    public IEnumerator PlayerAttack(Skill skill)
    {
        otherUnit.currentHP -= skill.damage;
        other_bar.UpdateHP(otherUnit.currentHP);

        yield return StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyAttack(Skill skill)
    {
        playerUnit.currentHP -= skill.damage;
        player_bar.UpdateHP(playerUnit.currentHP);

        yield return StartCoroutine(PlayerTurn());
    }

    public IEnumerator PlayerTurn()
    {
        state = BattleTurn.Turn1; //플레이어 턴으로 변경

        yield return StartCoroutine(Phase("Player's turn"));
        //메인 메뉴에 대한 오픈
        OpenMainMenu();
    }

    public IEnumerator EnemyTurn()
    {
        state = BattleTurn.Turn2; //상대 턴으로 변경

        yield return StartCoroutine(Phase("Enemy's turn"));

        //적이 진행할 행동 구현 (적이 가진 1번째 스킬을 사용한다.)
        Debug.Log(otherUnit.unitName + "의" + otherUnit.skills[0].skillName + "!!");

        yield return StartCoroutine(EnemyAttack(otherUnit.skills[0]));
    }

    //상황에 맞게 페이즈 텍스트에 대한 일시적인 표현을 위한 코드
    public IEnumerator Phase(string message , float duration = 1.5f)
    {
        PhaseText.text = message;
        PhaseText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        PhaseText.gameObject.SetActive(false);
    }

    private void OpenMainMenu()
    {
        mainmenuPanel.SetActive(true);
        skillmenuPanel.SetActive(false);
        ui_state = UI_State.MainMenu;
        mainMenuIdx = 0;
        //선택된 UI에 대한 설정 - 현재 선택된 요소를 해제합니다.
        EventSystem.current.SetSelectedGameObject(null);

        //선택 위치를 버튼으로 변경
        EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);
    }

    private void CloseMenu()
    {
        mainmenuPanel.SetActive(false);
        skillmenuPanel.SetActive(false);
        ui_state = UI_State.MainMenu;
        mainMenuIdx = 0;
        //선택된 UI에 대한 설정 - 현재 선택된 요소를 해제합니다.
        EventSystem.current.SetSelectedGameObject(null);
    }

    //메인 메뉴에 대한 선택
    void HandleMainMenuInput()
    {
        //오른쪽 입력에 대한 코드에서의 인덱스 처리
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mainMenuIdx = (mainMenuIdx + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + mainMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //버튼에 각각 onClick 이벤트를 등록한 경우라면 Invoke로 충분
            //buttons[mainMenuIdx].onClick.Invoke();

            //메인 코드로 처리할 경우라면 선택문을 통해 결정
            switch (mainMenuIdx)
            {
                case 0:
                    OpenSkillMenu(); break;
                case 1:
                    OpenBagMenu(); break;
                case 2:
                    SelectPokemonMenu(); break;
                case 3:
                    Run(); break;
            }
        }
    }

    private void OpenSkillMenu()
    {
        mainmenuPanel.SetActive(false);
        skillmenuPanel.SetActive(true);
        ui_state = UI_State.SKillMenu;
        mainMenuIdx = 0;

        for(int i = 0; i < skill_buttons.Length; i++)
        {
            //플레이어 유닛(포켓몬)이 가진 스킬 수만큼 개방하는 기능
            if(i < playerUnit.skills.Length)
            {
                skill_buttons[i].gameObject.SetActive(true);

                skill_buttons[i].GetComponentInChildren<Text>().text =
                    playerUnit.skills[i].skillName;
            }
            //없으면 false
            else
            {
                skill_buttons[i].gameObject.SetActive(false);
            }
        }

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);
    }

    private void HandleSkillMenuInput()
    {
        //아래 입력에 대한 코드에서의 인덱스 처리
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillMenuIdx = (skillMenuIdx + 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + skillMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillMenuIdx = (skillMenuIdx - 1) % skill_buttons.Length;
            if(skillMenuIdx < 0)
            {
                skillMenuIdx = skill_buttons.Length - 1; // 음수 인덱스 방지
            }
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + skillMenuIdx);
        }

        //Enter 키 입력
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Skill player_selected = playerUnit.skills[skillMenuIdx];
            //플레이어의 공격
            Debug.Log(playerUnit.name + "의" + playerUnit.skills[skillMenuIdx].skillName + "!!");

            //공격이 끝난 다음 턴을 넘긴다
            StartCoroutine(PlayerAttack(playerUnit.skills[skillMenuIdx]));
        }

        //ESC 키 입력
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //메인 메뉴 킨다.
            OpenMainMenu();
        }
    }

    private void OpenBagMenu()
    {
        Debug.Log("가방 메뉴 선택");
    }


    private void SelectPokemonMenu()
    {
        Debug.Log("포켓몬 메뉴 선택");
    }

    private void Run()
    {
        Debug.Log("도망갑니다~~~~");
    }

    public void WinBattle()
    {
        if (state == BattleTurn.Win)
        {
            isTyping = false; // 타이핑 상태 초기화
            color.a = 1.0f; // 텍스트의 투명도를 완전히 보이도록 설정
            PhaseText.color = color; // 텍스트 컬러 설정
            BattleDescriptionText.text = ""; // 배틀 설명 텍스트 초기화
        }
    }

    public void StartBattle()
    {
        if(state == BattleTurn.Start)
        {
            isTyping = false; // 타이핑 상태 초기화
            color.a = 1.0f; // 텍스트의 투명도를 완전히 보이도록 설정
            PhaseText.color = color; // 텍스트 컬러 설정
            BattleDescriptionText.text = ""; // 배틀 설명 텍스트 초기화
            state = BattleTurn.Turn1;
        }
    }

    public void Turn1Battle()
    {
        if (state == BattleTurn.Turn1)
        {
            isTyping = false; // 타이핑 상태 초기화
            color.a = 1.0f; // 텍스트의 투명도를 완전히 보이도록 설정
            PhaseText.color = color; // 텍스트 컬러 설정
            BattleDescriptionText.text = ""; // 배틀 설명 텍스트 초기화
        }
    }

    private void PaseText()
    {
        switch (state)
        {
            case BattleTurn.Start:
                //Typing.instance.StartTyping("Start Battle!", PhaseText);
                PhaseText.text = "Start Battle!";
                Color color = PhaseText.color;
                color.a = Mathf.PingPong(Time.time, 1.0f);
                //color.a = Mathf.Clamp(color.a, 0.5f, 1.0f); // 투명도 조정
                PhaseText.color = color; // 텍스트 깜빡임 효과
                if (!isTyping)
                {
                    Typing.instance.StartTyping("앗! 야생의" + otherUnit.unitName + "가 나타났다!", BattleDescriptionText);
                    isTyping = true;
                }
                break;
            case BattleTurn.Turn1:
                //PhaseText.text = "Player's Turn!";
                if (!isTyping)
                {
                    StartCoroutine(PlayerTurn());
                    isTyping = true;
                }
                break;
            //case BattleTurn.Turn2:
            //    PhaseText.text = "Enemy's Turn!";
            //    break;
            //case BattleTurn.Action1:
            //    PhaseText.text = "Player's Action!";
            //    break;
            //case BattleTurn.Action2:
            //    PhaseText.text = "Enemy's Action!";
            //    break;
            case BattleTurn.Win:
                PhaseText.text = "Victory!";
                color = PhaseText.color;
                color.a = Mathf.PingPong(Time.time, 1.0f);
                //color.a = Mathf.Clamp(color.a, 0.5f, 1.0f); // 투명도 조정
                PhaseText.color = color; // 텍스트 깜빡임 효과
                break;
            //case BattleTurn.Lose:
            //    PhaseText.text = "Defeat...";
            //    break;
        }
    }
}
