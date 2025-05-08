using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //��Ʈ�� ��ư�� ���ؼ� �������� �޴�
    public GameObject ControlKeyMenu;
    public GameObject ExitInformation;

    public void OnStartButtonEnter()
    {
        SceneManager.LoadScene("GameScene");
        //GameScene ������ �ε��մϴ�.
    }

    //���� �ִٸ� ����
    public void OnControlKeyButtonEnter()
    {
        //activeSelf�� ������ ���ӿ�����Ʈ�� Ȱ�� ���������� ���θ� Ȯ���� �� �ִ� ������Ƽ
        if (ControlKeyMenu.activeSelf == true)
        {
            ControlKeyMenu.SetActive(false);
        }
        else
        {
            ControlKeyMenu.SetActive(true);
        }
    }

    public void OnControlKeyButtonExit()
    {
        ControlKeyMenu.SetActive(false);
    }

    //������ �� ȯ�濡���� �����
    //���� �� ȯ�漼���� ���Ḧ ��Ȳ�� ���� ó���մϴ�.
    public void OnExitButtonEnter()
    {
        ExitInformation.SetActive(true);
    }

    public void OnExitTrue()
    {
#if UNITY_EDITOR // ����Ƽ ������ �ʿ����� �۾�
        UnityEditor.EditorApplication.isPlaying = false;
        //������ �ٷ� ������ ���(�����, �����)
#else
        Application.Quit(); //���� ��Ȱ��ȭ�Ǵ� �ڵ尡 �ٷ� ����
#endif
    }

    public void OnExitFalse()
    {
        ExitInformation?.SetActive(false);
    }
}
