using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour
{
    public TMP_Text current_score;
    public TMP_Text high_score;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    //해당 메뉴가 활성화 되었을 때 코드
    public void OnEnable()
    {
        Debug.Log("활성화 상태");
        
    }

    public void SetScoreText(float score)
    {
        gameObject.SetActive (true);
        current_score.text = ((int)(score)).ToString();

        //만약에 전달받은 점수가 현재 레지스트리의 하이스코어보다 크다면
        if(score > PlayerPrefs.GetInt("HIGH_SCORE"))
        {
            //새롭게 갱신합니다.
            PlayerPrefs.SetInt("HIGH_SCORE", (int)score);
        }

        high_score.text = PlayerPrefs.GetInt("HIGH_SCORE").ToString();
    }

    //해당 메뉴가 비활성화되었을 때 코드
    public void OnDisable()
    {
        Debug.Log("비활성화 상태");
    }

    public void OnReplayButtonEnter()
    {
        //GetActiveScene()은 현재 활성화되어있는 씬을 의미합니다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitleButtonEnter()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
