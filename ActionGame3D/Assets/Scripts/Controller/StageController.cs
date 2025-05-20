using System.Collections;
using Assets.Scripts.Dialog;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController Instance; //매니저 정적 변수

    public int StagePoint = 0; //점수
    public int StageKill = 0; //잡은 수

    public Text PointText;   //점수를 표현할 UI
    public Text QuestText;
    public Image DeadImage;

    public Color flashColor = Color.black;
    public float duration;

    public bool Questing = false;

    Color startColor = new Color(0, 0, 0, 0); // 투명한 검정
    Color endColor = new Color(0, 0, 0, 1);   // 불투명한 검정


    private void Awake()
    {
        Instance = this;

        StartCoroutine(FadeIn());

    }
    private void Start()
    {
        //안내문 데이터 콜백
        DialogDataAlert alert = new DialogDataAlert("START", "10초마다 생성되는 슬라임들을 제거하세요.",
            () =>
            {
                Debug.Log("OK 버튼을 눌러주세요.");
            });

        DialogManager.Instance.Push(alert);
    }

    private void Update()
    {
        if (Questing)
        {
            QuestText.text = "슬라임 잡기\r\n필드에 생성되는 슬라임을 10마리 잡아오세요.\r\n현재 잡은 수 : " + StageKill.ToString() + " / 10\r\n퀘스트 보상 : 골드 500";
        }
    }


    //점수가 증가하면, 텍스트 UI에 수치를 적용
    public void AddPoint(int Point)
    {
        StagePoint += Point;
        if (Questing)
            StageKill += 1;
        PointText.text = Point.ToString();
    }

    //씬에 대한 리로드
    public void FinishGame()
    {
        DialogDataConfirm confirm = new DialogDataConfirm("Restart?", "Pleasepress OK if you want to restart the game",
            delegate (bool answer)
            {
                if (answer)
                    StartCoroutine(Fade());
                else
                {
                    //이전 프로젝트의 내용을 활용해서 데이터 상에서도 종료되도록 수정해주세요.
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
        );

        //매니저에 등록
        DialogManager.Instance.Push(confirm);

        //Application.LoadLevel(Application.loadedLevel);
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float fadeDuration = 1.5f; // 1.5초 동안 서서히 페이드 인
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            DeadImage.color = Color.Lerp(endColor, startColor, elapsed / fadeDuration);
            yield return null;
        }
        DeadImage.raycastTarget = false;

    }
    IEnumerator Fade()
    {
        float elapsed = 0f;
        float fadeDuration = 1.5f; // 1.5초 동안 서서히 페이드 아웃

        DeadImage.raycastTarget = true;
        while ( elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            DeadImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AcceptQuest()
    {
        //안내문 데이터 콜백
        DialogDataAlert quest = new DialogDataAlert("Quest", "슬라임을 잡으시오\n" +StageKill.ToString() + " / 10",
            () =>
            {
                Debug.Log("OK 버튼을 눌러주세요.");
                QuestText.text = "슬라임 잡기\r\n필드에 생성되는 슬라임을 10마리 잡아오세요.\r\n현재 잡은 수 : " + StageKill.ToString() + " / 10\r\n퀘스트 보상 : 골드 500";
                Questing = true;
            });

        DialogManager.Instance.Push(quest);
    }
}
