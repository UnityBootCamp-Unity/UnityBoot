using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController Instance; //�Ŵ��� ���� ����

    public int StagePoint = 0; //����

    public Text PointText;   //������ ǥ���� UI
    public Image DeadImage;

    public Color flashColor = Color.black;
    public float duration;

    Color startColor = new Color(0, 0, 0, 0); // ������ ����
    Color endColor = new Color(0, 0, 0, 1);   // �������� ����


    private void Awake()
    {
        Instance = this;

        StartCoroutine(FadeIn());

    }


    //������ �����ϸ�, �ؽ�Ʈ UI�� ��ġ�� ����
    public void AddPoint(int Point)
    {
        StagePoint += Point;
        PointText.text = Point.ToString();
    }

    //���� ���� ���ε�
    public void FinishGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
        StartCoroutine(Fade());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float fadeDuration = 1.5f; // 1.5�� ���� ������ ���̵� ��
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
        float fadeDuration = 1.5f; // 1.5�� ���� ������ ���̵� �ƿ�

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
