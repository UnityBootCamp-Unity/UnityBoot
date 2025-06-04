using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscOptionManager : MonoBehaviour
{
    public static EscOptionManager instance; // 싱글톤 인스턴스
    public GameObject escMenu; // ESC 메뉴 UI 오브젝트
    public GameObject escMainMenuButton; // ESC 메뉴에서 메인 메뉴로 이동하는 버튼
    public GameObject escExitButton; // ESC 메뉴에서 게임 종료 버튼

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            escMenu.SetActive(false); // 초기에는 ESC 메뉴를 비활성화

            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
            DontDestroyOnLoad(escMenu); // ESC 메뉴 UI도 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체를 파괴
        }
    }

    private void Update()
    {
        // 현재 씬이 MainScene이면 함수 실행 중단
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            return; // 함수 실행 중단
        }

        // ESC 키 입력 시 메뉴 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escMenu.activeSelf)
            {
                escMenu.SetActive(false); // ESC 메뉴가 활성화되어 있으면 비활성화
            }
            else
            {
                escMenu.SetActive(true); // ESC 메뉴가 비활성화되어 있으면 활성화
            }
        }
    }

    /// <summary>
    /// 메인 메뉴로 이동하는 버튼 클릭 이벤트 핸들러
    /// </summary>
    public void onClickMainMenu()
    {
        Debug.Log("NextMainMenu clicked");
        //메인 메뉴로 이동하는 로직 추가
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        escMenu.SetActive(false); // ESC 메뉴가 활성화되어 있으면 비활성화
    }

    /// <summary>
    /// 게임 종료 버튼 클릭 이벤트 핸들러
    /// </summary>
    public void onClickExitGame()
    {
        Debug.Log("ExitGame clicked");
        Application.Quit(); // 게임 종료
        escMenu.SetActive(false); // ESC 메뉴가 활성화되어 있으면 비활성화
    }
}
