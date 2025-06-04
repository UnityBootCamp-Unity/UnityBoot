using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseManager : MonoBehaviour
{
    public static DefenseManager instance; // 싱글톤 인스턴스

    [Header("턴 관련 설정")]
    public bool isTurning = true; // 턴이 진행 중인지 여부
    public int turnCount = 0; // 현재 턴 수
    public int maxTurnCount = 15; // 최대 턴 수
    public float turnTime = 30f; // 턴 시간 (초 단위)
    public bool allEnemiesInactive = true; //모든 적이 비활성화되었는지 여부를 확인하는 변수

    [Header("적 관련 설정")]
    public bool isEnemyWeakening = false; // 적 약화 여부
    public float damageWeakening = 1; //적에게 주는 피해량 감소 (50%로 설정됨)

    [Header("UI 관련 설정")]
    public GameObject turnUI; // 턴 UI 오브젝트
    public Button endTurnButton; // 턴 종료 버튼
    public Button clearGameButton; // 클리어 버튼 (모든 적이 비활성화되면 활성화됨)
    public Text turnCountText; // 현재 턴 수 텍스트

    private void Start()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체를 파괴
        }
    }

    private void Update()
    {
        //액션 턴이 진행 중일 때 타이머 업데이트
        if (!isTurning)
        {
            turnTime -= Time.deltaTime;
            if (turnTime <= 0)
            {
                EndTurn();
            }
        }
    }

    /// <summary>
    /// 턴 종료 처리 함수
    /// UI 업데이트 및 적 약화 처리 포함
    /// UI는 따로 스크립트 만들어서 처리하는게 좋을 듯
    /// </summary>
    public void EndTurn()
    {   //액션 턴 종료 처리
        isTurning = true;
        turnCount++;

        //턴 UI 업데이트
        turnUI.SetActive(true); //턴 UI 비활성화
        clearGameButton.gameObject.SetActive(false); //클리어 버튼 비활성화

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        allEnemiesInactive = true;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf) // 하나라도 활성화되어 있으면
            {
                allEnemiesInactive = false;
                break; // 더 볼 필요 없음
            }
        }


        if (allEnemiesInactive) //게임 클리어 여부 확인
        {
            turnCountText.text = "Defeated all enemies"; //모든 적이 비활성화되면 텍스트 업데이트
            endTurnButton.gameObject.SetActive(false); //턴 종료 버튼 비활성화
            clearGameButton.gameObject.SetActive(true); //클리어 버튼 활성화

            //endTurnButton.interactable = false; // 모든 적이 비활성화되면 버튼 비활성화
        }
        else if (turnCount >= maxTurnCount)
        {
            turnCountText.text = "The day is bright\n and The enemy is weakened"; //최대 턴 수에 도달하면 텍스트 업데이트
        }
        else
        {
            turnCountText.text = "The Current Turn\n" + turnCount.ToString() + " / " + maxTurnCount.ToString(); //현재 턴 수 텍스트 업데이트
        }


        //최대 턴 수에 도달하면 게임 약화 처리
        if (turnCount >= maxTurnCount && isEnemyWeakening == false)
        {
            Debug.Log("게임 종료: 최대 턴 수에 도달했습니다.");

            //모든 적을 찾아서 약화 처리
            foreach (GameObject enemyObj in enemies)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    //체력, 속도 (각각 50%, 70%)
                    enemy.NerfStats(0.5f, 0.7f);
                    damageWeakening = 0.5f; //적에게 주는 피해량 감소 (50%로 설정)
                }
            }
            isEnemyWeakening = true; //적 약화 상태로 설정

        }

        if(turnCount < maxTurnCount)
        {
            damageWeakening = 1; //적에게 주는 피해량 초기화
        }
            turnTime = 30f; //턴 시간 초기화
    }

    public void NextTurnButton()
    {
        Debug.Log("NextTurnButton clicked");

        if (isTurning == false) { return; } //턴이 진행 중이면 다음 턴 버튼 비활성화

        turnUI.SetActive(false); //턴 UI 비활성화

        isTurning = false; //턴 진행 상태로 변경
    }

    //게임 클리어 시 버튼 추 후 메뉴가 아닌 로비로 이동할 수 있도록 조치 예정
    public void NextMainMenu()
    {
        Debug.Log("NextMainMenu clicked");
        //메인 메뉴로 이동하는 로직 추가
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

}
