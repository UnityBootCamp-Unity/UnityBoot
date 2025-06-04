using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseManager : MonoBehaviour
{
    public static DefenseManager instance; // �̱��� �ν��Ͻ�

    [Header("�� ���� ����")]
    public bool isTurning = true; // ���� ���� ������ ����
    public int turnCount = 0; // ���� �� ��
    public int maxTurnCount = 15; // �ִ� �� ��
    public float turnTime = 30f; // �� �ð� (�� ����)
    public bool allEnemiesInactive = true; //��� ���� ��Ȱ��ȭ�Ǿ����� ���θ� Ȯ���ϴ� ����

    [Header("�� ���� ����")]
    public bool isEnemyWeakening = false; // �� ��ȭ ����
    public float damageWeakening = 1; //������ �ִ� ���ط� ���� (50%�� ������)

    [Header("UI ���� ����")]
    public GameObject turnUI; // �� UI ������Ʈ
    public Button endTurnButton; // �� ���� ��ư
    public Button clearGameButton; // Ŭ���� ��ư (��� ���� ��Ȱ��ȭ�Ǹ� Ȱ��ȭ��)
    public Text turnCountText; // ���� �� �� �ؽ�Ʈ

    private void Start()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
        }
    }

    private void Update()
    {
        //�׼� ���� ���� ���� �� Ÿ�̸� ������Ʈ
        if (!isTurning)
        {
            turnTime -= Time.deltaTime;
            if (turnTime <= 0)
            {
                EndTurn();
            }
        }
    }

    /// <summary>
    /// �� ���� ó�� �Լ�
    /// UI ������Ʈ �� �� ��ȭ ó�� ����
    /// UI�� ���� ��ũ��Ʈ ���� ó���ϴ°� ���� ��
    /// </summary>
    public void EndTurn()
    {   //�׼� �� ���� ó��
        isTurning = true;
        turnCount++;

        //�� UI ������Ʈ
        turnUI.SetActive(true); //�� UI ��Ȱ��ȭ
        clearGameButton.gameObject.SetActive(false); //Ŭ���� ��ư ��Ȱ��ȭ

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        allEnemiesInactive = true;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf) // �ϳ��� Ȱ��ȭ�Ǿ� ������
            {
                allEnemiesInactive = false;
                break; // �� �� �ʿ� ����
            }
        }


        if (allEnemiesInactive) //���� Ŭ���� ���� Ȯ��
        {
            turnCountText.text = "Defeated all enemies"; //��� ���� ��Ȱ��ȭ�Ǹ� �ؽ�Ʈ ������Ʈ
            endTurnButton.gameObject.SetActive(false); //�� ���� ��ư ��Ȱ��ȭ
            clearGameButton.gameObject.SetActive(true); //Ŭ���� ��ư Ȱ��ȭ

            //endTurnButton.interactable = false; // ��� ���� ��Ȱ��ȭ�Ǹ� ��ư ��Ȱ��ȭ
        }
        else if (turnCount >= maxTurnCount)
        {
            turnCountText.text = "The day is bright\n and The enemy is weakened"; //�ִ� �� ���� �����ϸ� �ؽ�Ʈ ������Ʈ
        }
        else
        {
            turnCountText.text = "The Current Turn\n" + turnCount.ToString() + " / " + maxTurnCount.ToString(); //���� �� �� �ؽ�Ʈ ������Ʈ
        }


        //�ִ� �� ���� �����ϸ� ���� ��ȭ ó��
        if (turnCount >= maxTurnCount && isEnemyWeakening == false)
        {
            Debug.Log("���� ����: �ִ� �� ���� �����߽��ϴ�.");

            //��� ���� ã�Ƽ� ��ȭ ó��
            foreach (GameObject enemyObj in enemies)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    //ü��, �ӵ� (���� 50%, 70%)
                    enemy.NerfStats(0.5f, 0.7f);
                    damageWeakening = 0.5f; //������ �ִ� ���ط� ���� (50%�� ����)
                }
            }
            isEnemyWeakening = true; //�� ��ȭ ���·� ����

        }

        if(turnCount < maxTurnCount)
        {
            damageWeakening = 1; //������ �ִ� ���ط� �ʱ�ȭ
        }
            turnTime = 30f; //�� �ð� �ʱ�ȭ
    }

    public void NextTurnButton()
    {
        Debug.Log("NextTurnButton clicked");

        if (isTurning == false) { return; } //���� ���� ���̸� ���� �� ��ư ��Ȱ��ȭ

        turnUI.SetActive(false); //�� UI ��Ȱ��ȭ

        isTurning = false; //�� ���� ���·� ����
    }

    //���� Ŭ���� �� ��ư �� �� �޴��� �ƴ� �κ�� �̵��� �� �ֵ��� ��ġ ����
    public void NextMainMenu()
    {
        Debug.Log("NextMainMenu clicked");
        //���� �޴��� �̵��ϴ� ���� �߰�
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

}
