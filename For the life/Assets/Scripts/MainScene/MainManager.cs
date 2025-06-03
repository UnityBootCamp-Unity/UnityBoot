using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject PlayMenuPanel;

    void Start()
    {
        /*// 1920x1080의 0.24배 크기로 해상도 변경
        int width = Mathf.RoundToInt(1920 * 0.24f);
        int height = Mathf.RoundToInt(1080 * 0.24f);

        // 전체화면 끄고 창모드로 해상도 변경
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
