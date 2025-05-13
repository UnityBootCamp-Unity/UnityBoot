using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("== UI Component ==")]
    public Text currentScoreUI;
    public Text highScoreUI;

    [Header("== Fields ==")]
    public int currentScore;
    public int highScore;

    //������ ���� ������Ƽ ����
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;
            currentScoreUI.text = "���� ���� : " + currentScore;

            if(currentScore > highScore)
            {
                highScore = currentScore;
                highScoreUI.text = "�ְ� ���� : " + highScore;

                PlayerPrefs.SetInt("HIGH_SCORE", highScore);
                PlayerPrefs.Save();
            }
        }
    }

    #region �̱���
    public static ScoreManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����ְ� ��
        }
        else
        {
            Destroy(gameObject); // �̹� �ϳ� �ִٸ� ���� ���� �� �ı�
        }
    }
    #endregion
}
