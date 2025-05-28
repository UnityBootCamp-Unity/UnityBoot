using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//��Ʋ���� ����� �Ͽ� ���� ���¸� enum���� ǥ��
public enum BattleTurn
{
    None                //���� ���� ����
    ,Start               //����
    ,Turn1, Turn2       //�÷��̾� ��, ��� ��
    ,Action1, Action2   //�÷��̾��� ����, ����� ����
    ,Win, Lose          //�� , ��
}

//UI������ �޴� ȭ�鿡 ���� ����
public enum UI_State
{
    MainMenu,SKillMenu
}

public class BattleSystem : MonoBehaviour
{
    //��Ʋ �ý����� ������ ��Ʋ�� ����
    //(�ϸ��� �ٲ�)
    public BattleTurn state;

    //���� UI�� ���� ����
    public UI_State ui_state;

    //�� ������� �� ����� ���� �ؽ�Ʈ
    public Text PhaseText;
    public Color color;

    //��Ʋ �ؼ� �ؽ�Ʈ
    public Text BattleDescriptionText;

    //�ؽ�Ʈ �÷�����
    public Color testColor;

    //�÷��̾�� ���濡 ���� ���
    public Unit playerUnit;
    public Unit otherUnit;

    public Button[] buttons; //�޴� ��ư
    public Button[] skill_buttons; //��ų�� ���� ��ư

    //�޴� �� ����
    public GameObject mainmenuPanel;
    public GameObject skillmenuPanel;

    //�÷��̾��� �����̴� �� ����
    public UnitHPbar player_bar;
    public UnitHPbar other_bar;

    //���� �޴��鿡 ���� �ε��� ��
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;

    //Ÿ���� �������� Ȯ��
    private bool isTyping = false;

    private void Start()
    {
        state = BattleTurn.Start;

        //�����̴� �� ����
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
            state = BattleTurn.Turn1; // �÷��̾� ���� ������ ��� ������ �Ѿ
            StopAllCoroutines(); // ��� �ڷ�ƾ ����
            Typing.instance.StartTyping(playerUnit.name + "��(��) ����� ������ ���� �������� ��Ƽ����.", BattleDescriptionText);
            playerUnit.currentHP = 1;
        }
        else if(otherUnit.currentHP <= 0)
        {
            PhaseText.text = "";
            CloseMenu();
            state = BattleTurn.Win; // �÷��̾ �¸��� ���·� ����
            StopAllCoroutines(); // ��� �ڷ�ƾ ����
            Typing.instance.StartTyping(otherUnit.name + "��(��) ��ſ��� �й��ߴ�.", BattleDescriptionText);
            otherUnit.currentHP = 1;
        }
    }

    //��ų�� ����
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
        state = BattleTurn.Turn1; //�÷��̾� ������ ����

        yield return StartCoroutine(Phase("Player's turn"));
        //���� �޴��� ���� ����
        OpenMainMenu();
    }

    public IEnumerator EnemyTurn()
    {
        state = BattleTurn.Turn2; //��� ������ ����

        yield return StartCoroutine(Phase("Enemy's turn"));

        //���� ������ �ൿ ���� (���� ���� 1��° ��ų�� ����Ѵ�.)
        Debug.Log(otherUnit.unitName + "��" + otherUnit.skills[0].skillName + "!!");

        yield return StartCoroutine(EnemyAttack(otherUnit.skills[0]));
    }

    //��Ȳ�� �°� ������ �ؽ�Ʈ�� ���� �Ͻ����� ǥ���� ���� �ڵ�
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
        //���õ� UI�� ���� ���� - ���� ���õ� ��Ҹ� �����մϴ�.
        EventSystem.current.SetSelectedGameObject(null);

        //���� ��ġ�� ��ư���� ����
        EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);
    }

    private void CloseMenu()
    {
        mainmenuPanel.SetActive(false);
        skillmenuPanel.SetActive(false);
        ui_state = UI_State.MainMenu;
        mainMenuIdx = 0;
        //���õ� UI�� ���� ���� - ���� ���õ� ��Ҹ� �����մϴ�.
        EventSystem.current.SetSelectedGameObject(null);
    }

    //���� �޴��� ���� ����
    void HandleMainMenuInput()
    {
        //������ �Է¿� ���� �ڵ忡���� �ε��� ó��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mainMenuIdx = (mainMenuIdx + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);

            Debug.Log("������ idx = " + mainMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //��ư�� ���� onClick �̺�Ʈ�� ����� ����� Invoke�� ���
            //buttons[mainMenuIdx].onClick.Invoke();

            //���� �ڵ�� ó���� ����� ���ù��� ���� ����
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
            //�÷��̾� ����(���ϸ�)�� ���� ��ų ����ŭ �����ϴ� ���
            if(i < playerUnit.skills.Length)
            {
                skill_buttons[i].gameObject.SetActive(true);

                skill_buttons[i].GetComponentInChildren<Text>().text =
                    playerUnit.skills[i].skillName;
            }
            //������ false
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
        //�Ʒ� �Է¿� ���� �ڵ忡���� �ε��� ó��
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillMenuIdx = (skillMenuIdx + 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("������ idx = " + skillMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillMenuIdx = (skillMenuIdx - 1) % skill_buttons.Length;
            if(skillMenuIdx < 0)
            {
                skillMenuIdx = skill_buttons.Length - 1; // ���� �ε��� ����
            }
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("������ idx = " + skillMenuIdx);
        }

        //Enter Ű �Է�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Skill player_selected = playerUnit.skills[skillMenuIdx];
            //�÷��̾��� ����
            Debug.Log(playerUnit.name + "��" + playerUnit.skills[skillMenuIdx].skillName + "!!");

            //������ ���� ���� ���� �ѱ��
            StartCoroutine(PlayerAttack(playerUnit.skills[skillMenuIdx]));
        }

        //ESC Ű �Է�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //���� �޴� Ų��.
            OpenMainMenu();
        }
    }

    private void OpenBagMenu()
    {
        Debug.Log("���� �޴� ����");
    }


    private void SelectPokemonMenu()
    {
        Debug.Log("���ϸ� �޴� ����");
    }

    private void Run()
    {
        Debug.Log("�������ϴ�~~~~");
    }

    public void WinBattle()
    {
        if (state == BattleTurn.Win)
        {
            isTyping = false; // Ÿ���� ���� �ʱ�ȭ
            color.a = 1.0f; // �ؽ�Ʈ�� ������ ������ ���̵��� ����
            PhaseText.color = color; // �ؽ�Ʈ �÷� ����
            BattleDescriptionText.text = ""; // ��Ʋ ���� �ؽ�Ʈ �ʱ�ȭ
        }
    }

    public void StartBattle()
    {
        if(state == BattleTurn.Start)
        {
            isTyping = false; // Ÿ���� ���� �ʱ�ȭ
            color.a = 1.0f; // �ؽ�Ʈ�� ������ ������ ���̵��� ����
            PhaseText.color = color; // �ؽ�Ʈ �÷� ����
            BattleDescriptionText.text = ""; // ��Ʋ ���� �ؽ�Ʈ �ʱ�ȭ
            state = BattleTurn.Turn1;
        }
    }

    public void Turn1Battle()
    {
        if (state == BattleTurn.Turn1)
        {
            isTyping = false; // Ÿ���� ���� �ʱ�ȭ
            color.a = 1.0f; // �ؽ�Ʈ�� ������ ������ ���̵��� ����
            PhaseText.color = color; // �ؽ�Ʈ �÷� ����
            BattleDescriptionText.text = ""; // ��Ʋ ���� �ؽ�Ʈ �ʱ�ȭ
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
                //color.a = Mathf.Clamp(color.a, 0.5f, 1.0f); // ���� ����
                PhaseText.color = color; // �ؽ�Ʈ ������ ȿ��
                if (!isTyping)
                {
                    Typing.instance.StartTyping("��! �߻���" + otherUnit.unitName + "�� ��Ÿ����!", BattleDescriptionText);
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
                //color.a = Mathf.Clamp(color.a, 0.5f, 1.0f); // ���� ����
                PhaseText.color = color; // �ؽ�Ʈ ������ ȿ��
                break;
            //case BattleTurn.Lose:
            //    PhaseText.text = "Defeat...";
            //    break;
        }
    }
}
