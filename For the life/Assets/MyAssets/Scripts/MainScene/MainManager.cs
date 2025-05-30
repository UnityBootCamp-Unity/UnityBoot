using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject PlayMenuPanel;

    void Start()
    {
        /*// 1920x1080�� 0.24�� ũ��� �ػ� ����
        int width = Mathf.RoundToInt(1920 * 0.24f);
        int height = Mathf.RoundToInt(1080 * 0.24f);

        // ��üȭ�� ���� â���� �ػ� ����
        Screen.SetResolution(width, height, false);*/
    }

    public void onClickPlayButton()
    {
        PlayMenuPanel.SetActive(true);
    }

    public void onClickExitButton()
    {
        Application.Quit();
    }

    public void onClickCloseButton()
    {
        if (PlayMenuPanel != null)
        {
            PlayMenuPanel.SetActive(false);
        }
    }

    public void onStartButton()
    {
        SceneManager.LoadScene("DefenseScene");
    }
}
