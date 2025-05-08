using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public PlayerController playerController;

    //점수
    //1. 인게임 내에서 간단하게 사용할거면 필드로 만든다.
    //2. 게임을 끄고 난 뒤에도 점수가 유지되야 하는 상황일 경우라면
    //   다음과 같은 방법들을 고려합니다.
    //   2-1. PlayerPrefs : 유니티의 데이터를 레지스트리에 저장할 때 사용합니다.
    //                      정수,실수,문장 정도의 간단한 데이터 저장 가능
    //                      따로 제거안하면 게임을 삭제해도 남아있는 경우가 허다함.
    //   2-2. Json        : JavaScript Object Notation : 데이터 전송용 파일
    //                      객체, 배열, 문자열, Boolean, Null, 숫자 등의 데이터 유형 제공
    //                      주로 게임에서 딕셔너리나 리스트 등을 통해서 아이템 테이블, 인벤토리
    //                      몬스터 테이블 등을 구현할려고 할 때 편하게 사용하는 용도

    //   2-3. Firebase    : 데이터베이스와의 연동을 통해 데이터를 관리합니다.

    //   2-4. ScriptableObject : 유니티 내부에 Asset의 형태로써 데이터를 저장해서 사용합니다.
    //                           인게임 데이터를 구성할 때 가장 쉽고 편리합니다.

    //   2-5. CSV         : 엑셀 파일에 필요한 데이터들을 나열해두고, C# 스크립트를 통해 해당 값을 얻어와서
    //                      적용합니다. 주로 맵 패턴 , 스크립트 대화 호출, 기본적인 데이터
    private float score = 0.0f;

    //점수에 따른 난이도 표현
    private int level = 1;

    //최대 레벨
    private int max_level = 10;

    //레벨 당 요구 점수
    private int levelperscore = 10;

    //텍스트 UI
    public TMP_Text scoreText;
    //public TextMeshProUGUI scoreText;
    public TMP_Text levelText;    //현재 레벨
    public TMP_Text perScoreText; //다음 레벨까지의 점수
    public TMP_Text Player_Speed; //플레이어의 이동 속도

    private void Start()
    {
        SetTMP_Text();
    }
    //String Format
    //$"{변수}"를 적을 경우 해당 변수가 문자열로 넘어가게 됩니다.
    //{숫자:N0} : 문자 출력 시 ,를 표시할 수 있습니다. 1000 -> 1,000
    // #,##0 : #은 0이 앞에 안붙음. 숫자가 있으면 표시, 없으면 표시 안함
    //         0이 앞에 붙음. 숫자가 있으면 숫자표시, 없으면 0 표시

    void Update()
    {
        if(score >= levelperscore)
        {
            LevelUP();
        }

        score += Time.deltaTime; //프레임당 100점
        scoreText.text = ((int)score).ToString();
    }

    private void LevelUP()
    {
        if (level == max_level)
            return;

        levelperscore *= 3;
        level++;
        playerController.SetSpeed(level);
        SetTMP_Text();
    }

    public void SetTMP_Text()
    {
        levelText.text = $"leve : {level}";
        perScoreText.text = $"Goal : {levelperscore: #,##0}";
        Player_Speed.text = $"Speed :{playerController.GetSpeed()}";
    }
}
