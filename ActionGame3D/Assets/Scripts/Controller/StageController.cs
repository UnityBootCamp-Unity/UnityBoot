using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController Instance; //매니저 정적 변수

    public int StagePoint = 0; //점수

    public Text PointText;   //점수를 표현할 UI
    public Image DeadImage;

    public Color flashColor = Color.black;
    public float duration;

    Color startColor = new Color(0, 0, 0, 0); // 투명한 검정
    Color endColor = new Color(0, 0, 0, 1);   // 불투명한 검정


    private void Awake()
    {
        Instance = this;

        StartCoroutine(FadeIn());

    }


    //점수가 증가하면, 텍스트 UI에 수치를 적용
    public void AddPoint(int Point)
    {
        StagePoint += Point;
        PointText.text = Point.ToString();
    }

    //씬에 대한 리로드
    public void FinishGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
        StartCoroutine(Fade());
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
}
