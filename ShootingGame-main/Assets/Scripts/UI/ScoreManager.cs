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

    //점수에 대한 프로퍼티 설계
    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;
            currentScoreUI.text = "현재 점수 : " + currentScore;

            if(currentScore > highScore)
            {
                highScore = currentScore;
                highScoreUI.text = "최고 점수 : " + highScore;

                PlayerPrefs.SetInt("HIGH_SCORE", highScore);
                PlayerPrefs.Save();
            }
        }
    }

    #region 싱글톤
    public static ScoreManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 살아있게 함
        }
        else
        {
            Destroy(gameObject); // 이미 하나 있다면 새로 생긴 건 파괴
        }
    }
    #endregion
}
