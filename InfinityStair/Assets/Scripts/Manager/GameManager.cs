using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    public GameObject Character; //ĳ����
    public GameObject Platform; //����
    public GameObject Camera; // ĳ���͸� ����ٴϴ� ī�޶�
    public GameObject Menu; // ���� ���� �� �޴� UI
    public GameObject GameOverMenu; // ���� ���� �� ��Ÿ���� UI
    public Transform Platform_Parents; //���� ����� ��ġ

    public int platform_pos_idx = 0; //���� ��ġ�� ���� �ε��� ��
    public int character_pos_idx = 0; //ĳ���� ��ġ�� ���� �ε��� ��
    public bool isPlaying = false; //���� ����

    //�÷��� ����Ʈ(��ġ�Ǿ��ִ� ��)
    private List<GameObject> Platform_List = new List<GameObject>();

    //�÷����� ���� üũ ����Ʈ (������ ��ġ)
    private List<int> Platform_Check_List = new List<int>();

    public bool isLefting = true; // �������� �̵� ������ ����
    public int character_clear_stair = 0; // ĳ���Ͱ� ���� ������ ���� (20���� Ȯ��)
    public int check_clear_bundle = 0; // 60 ����� 20�� ����� Ȯ��

    private void Start()
    {
        Menu.gameObject.SetActive(true); // �޴� UI Ȱ��ȭ
        SetFlatform(); //���� ����
        Init(); //���� �ʱ�ȭ
        isLefting = true;
    }

    private void Update()
    {
        //�÷��� �����
        if (isPlaying)
        {
            if (character_pos_idx == 60)
                character_pos_idx = 0; // 60��° ������ ����� �� �ٽ� 0���� �ʱ�ȭ

            //������ ����� �׽�Ʈ������, ��ư�� ���� Ŭ������ ������ ����
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
        //[üũ Ȯ�ο� �ڵ�]
        Debug.Log("d�ε��� �� : " + idx + "�÷��� : " + Platform_Check_List[idx] + "/���� : " + direction);

        //������ ���� ���
        if (Platform_Check_List[idx % 60] == direction)
        {
            
            //ĳ������ ��ġ ����
            character_pos_idx++;


            //Character.transform.position = Platform_List[idx].transform.position
            //    + new Vector3(0f, 0.4f, 0f);
            Vector3 character_pos = Character.transform.position; // ĳ������ ��ġ
            character_pos.x = Platform_List[idx].transform.position.x + 0.05f;
            character_pos.y += 0.3f;
            Character.transform.position = character_pos; // ĳ���� ��ġ ���� ����

            character_clear_stair++; // ĳ���Ͱ� ������ ����� ������ ī��Ʈ ����
            PlayerClear20Flatform(); // 20���� ������ �Ѱ���� Ȯ��
            //�ٴ� ����
            //NextPlatform(platform_pos_idx);

        }
        else
        {
            GameOver();
            Camera.gameObject.SetActive(false); // ī�޶� ��Ȱ��ȭ
        }
    }

    private void GameOver()
    {
        Debug.Log("���� ����"); //�����δ� UI�� ���� ������ ���� �г� Ȱ��ȭ
                                //���ھ� ����
        isPlaying = false;
        player.Die(); // ĳ���� ��� �ִϸ��̼� ����
        StartCoroutine(DelayedGameOverMenu()); // ���� ���� �޴� Ȱ��ȭ ���� ����
    }

    private void Init()
    {
        Character.transform.position = new Vector3(0.15f,-0.55f, 0);

        for (platform_pos_idx = 0; platform_pos_idx < 60; platform_pos_idx++)
        {
            NextPlatform(platform_pos_idx);
        }

        character_pos_idx = 0;
        //isPlaying = true; //���Ŀ��� �÷��� ��ư�� ������ �� ����ǵ��� ��������� �մϴ�.
        player.Idle(); // ĳ���� �ʱ�ȭ

        isPlaying = false; // ���� ���� ������ �÷��� ���°� �ƴϹǷ� false�� ����
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
            /*else //�ε��� ������ ���� ���
            {
                //���� ����
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
                //������ ����
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
        //������ ���ڸ�ŭ �÷��� ����
        for(int i = 0; i < 60; i++)
        {
            GameObject plat = Instantiate(Platform, Vector3.zero, Quaternion.identity, Platform_Parents);
            Platform_List.Add(plat);
            Platform_Check_List.Add(0);
        }
    }

    // ���� ���� ���� 20���� ������ �ѱ��� �� �Ʒ����� �� ���� �ű�� �߰����� �������� ���̵� ��
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

            check_clear_bundle = 1; // 20���� �Ѱ����� ���� �ܰ��
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
            check_clear_bundle = 2; // 40���� �Ѱ����� ���� �ܰ��
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
            check_clear_bundle = 0; // 60���� �Ѱ����� ���� �ܰ��
        }
    }

    public void OnClickStartButton()
    {
        isPlaying = true; // ���� ����
        Menu.gameObject.SetActive(false); // �޴� UI ��Ȱ��ȭ

    }

    public void OnClickReStartButton()
    {
        GameOverMenu.gameObject.SetActive(false); // ���� ���� �޴� ��Ȱ��ȭ
        Camera.gameObject.SetActive(true); // ī�޶� Ȱ��ȭ
        player.Idle(); // ĳ���� �ʱ�ȭ
        isLefting = true; // �������� �̵� ���� �ʱ�ȭ
        character_clear_stair = 0;
        check_clear_bundle = 0;
        Init(); // ���� �����
        isPlaying = true; // ���� ����
    }

    public void OnClickReMenuButton()
    {
        GameOverMenu.gameObject.SetActive(false); // ���� ���� �޴� ��Ȱ��ȭ
        Camera.gameObject.SetActive(true); // ī�޶� Ȱ��ȭ
        player.Idle(); // ĳ���� �ʱ�ȭ
        isLefting = true; // �������� �̵� ���� �ʱ�ȭ
        character_clear_stair = 0;
        check_clear_bundle = 0;
        Init(); // ���� �����
        Menu.gameObject.SetActive(true); // �޴� UI Ȱ��ȭ
    }


    private IEnumerator DelayedCheck_Platform(int idx, int direction)
    {
        yield return new WaitForSeconds(0.1f);
        Check_Platform(idx, direction);
    }

    private IEnumerator DelayedGameOverMenu()
    {
        yield return new WaitForSeconds(3f); // 1�� �Ŀ� ���� ���� �޴� Ȱ��ȭ
        GameOverMenu.gameObject.SetActive(true); // ���� ���� �޴� Ȱ��ȭ
    }

}
