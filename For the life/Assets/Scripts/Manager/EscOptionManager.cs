using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscOptionManager : MonoBehaviour
{
    public static EscOptionManager instance; // �̱��� �ν��Ͻ�
    public GameObject escMenu; // ESC �޴� UI ������Ʈ
    public GameObject escMainMenuButton; // ESC �޴����� ���� �޴��� �̵��ϴ� ��ư
    public GameObject escExitButton; // ESC �޴����� ���� ���� ��ư

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            escMenu.SetActive(false); // �ʱ⿡�� ESC �޴��� ��Ȱ��ȭ

            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
            DontDestroyOnLoad(escMenu); // ESC �޴� UI�� �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı�
        }
    }

    private void Update()
    {
        // ���� ���� MainScene�̸� �Լ� ���� �ߴ�
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            return; // �Լ� ���� �ߴ�
        }

        // ESC Ű �Է� �� �޴� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escMenu.activeSelf)
            {
                escMenu.SetActive(false); // ESC �޴��� Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ
            }
            else
            {
                escMenu.SetActive(true); // ESC �޴��� ��Ȱ��ȭ�Ǿ� ������ Ȱ��ȭ
            }
        }
    }

    /// <summary>
    /// ���� �޴��� �̵��ϴ� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    /// </summary>
    public void onClickMainMenu()
    {
        Debug.Log("NextMainMenu clicked");
        //���� �޴��� �̵��ϴ� ���� �߰�
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        escMenu.SetActive(false); // ESC �޴��� Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ
    }

    /// <summary>
    /// ���� ���� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    /// </summary>
    public void onClickExitGame()
    {
        Debug.Log("ExitGame clicked");
        Application.Quit(); // ���� ����
        escMenu.SetActive(false); // ESC �޴��� Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ
    }
}
