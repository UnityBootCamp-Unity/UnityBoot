using System.Collections;
using Assets.Scripts.Dialog;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public static StageController Instance; //�Ŵ��� ���� ����

    public int StagePoint = 0; //����
    public int StageKill = 0; //���� ��

    public Text PointText;   //������ ǥ���� UI
    public Text QuestText;
    public Image DeadImage;

    public Color flashColor = Color.black;
    public float duration;

    public bool Questing = false;

    Color startColor = new Color(0, 0, 0, 0); // ������ ����
    Color endColor = new Color(0, 0, 0, 1);   // �������� ����


    private void Awake()
    {
        Instance = this;

        StartCoroutine(FadeIn());

    }
    private void Start()
    {
        //�ȳ��� ������ �ݹ�
        DialogDataAlert alert = new DialogDataAlert("START", "10�ʸ��� �����Ǵ� �����ӵ��� �����ϼ���.",
            () =>
            {
                Debug.Log("OK ��ư�� �����ּ���.");
            });

        DialogManager.Instance.Push(alert);
    }

    private void Update()
    {
        if (Questing)
        {
            QuestText.text = "������ ���\r\n�ʵ忡 �����Ǵ� �������� 10���� ��ƿ�����.\r\n���� ���� �� : " + StageKill.ToString() + " / 10\r\n����Ʈ ���� : ��� 500";
        }
    }


    //������ �����ϸ�, �ؽ�Ʈ UI�� ��ġ�� ����
    public void AddPoint(int Point)
    {
        StagePoint += Point;
        if (Questing)
            StageKill += 1;
        PointText.text = Point.ToString();
    }

    //���� ���� ���ε�
    public void FinishGame()
    {
        DialogDataConfirm confirm = new DialogDataConfirm("Restart?", "Pleasepress OK if you want to restart the game",
            delegate (bool answer)
            {
                if (answer)
                    StartCoroutine(Fade());
                else
                {
                    //���� ������Ʈ�� ������ Ȱ���ؼ� ������ �󿡼��� ����ǵ��� �������ּ���.
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
        );

        //�Ŵ����� ���
        DialogManager.Instance.Push(confirm);

        //Application.LoadLevel(Application.loadedLevel);
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

    public void AcceptQuest()
    {
        //�ȳ��� ������ �ݹ�
        DialogDataAlert quest = new DialogDataAlert("Quest", "�������� �����ÿ�\n" +StageKill.ToString() + " / 10",
            () =>
            {
                Debug.Log("OK ��ư�� �����ּ���.");
                QuestText.text = "������ ���\r\n�ʵ忡 �����Ǵ� �������� 10���� ��ƿ�����.\r\n���� ���� �� : " + StageKill.ToString() + " / 10\r\n����Ʈ ���� : ��� 500";
                Questing = true;
            });

        DialogManager.Instance.Push(quest);
    }
}
