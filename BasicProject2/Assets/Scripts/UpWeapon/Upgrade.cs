using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Upgrade : Weapon
{
    public UnityEvent OnUpgrade; // 업그레이드 이벤트
    public Text upgrade_level_text; // 업그레이드 레벨 텍스트
    public Text upgrade_probability_text; // 업그레이드 확률 텍스트
    public Text message_current;
    public Text message_all_probability; // 업그레이드 전체 확률 텍스트

    public int upgrade_level = 0; // 업그레이드 레벨
    public int max_upgrade_level = 20; // 최대 업그레이드 레벨

    void SuccessUpgrade() => upgrade_level++; // 업그레이드 성공 시 레벨 증가

    void FailUpgrade()
    {
        if (upgrade_level < 10)
            upgrade_level--; // 업그레이드 실패 시 레벨 감소
        else if (upgrade_level < 18)
            upgrade_level -= 2; // 업그레이드 실패 시 레벨 감소
        else
            upgrade_level = 0; // 업그레이드 실패 시 레벨 초기화
    }

    void UpgradeText()
    {
        if (upgrade_level < 10)
            message_all_probability.text = "실패 시 레벨 - 1";
        else if (upgrade_level < 18)
            message_all_probability.text = "실패 시 레벨 - 2";
        else
            message_all_probability.text = "실패 시 레벨 초기화";

        upgrade_level_text.text = $"{upgrade_level} 레벨";
        upgrade_probability_text.text = $"{(100 - (upgrade_level * 4))} %";
        message_current.text = $"[업그레이드 진행 중...] 현재 레벨 {upgrade_level} / {max_upgrade_level} 레벨";
    }

    void UpgradeProcess()
    {
        int upgrade_probability = Random.Range(0, 100); // 업그레이드 확률
        if (upgrade_level < max_upgrade_level)
        {
            if (upgrade_probability >= upgrade_level * 4)
            {
                SuccessUpgrade(); // 업그레이드 성공
                /*message_current.text = "업그레이드 성공";
                StartCoroutine(WaitForSeconds(3.0f)); // 3초 대기 후 메시지 초기화*/
            }
            else
            {
                FailUpgrade(); // 업그레이드 실패
                /*message_current.text = "업그레이드 실패";
                StartCoroutine(WaitForSeconds(3.0f)); // 3초 대기 후 메시지 초기화*/
            }
        }
        else
        {
            message_current.text = "최대 레벨 도달";
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnUpgrade.AddListener(UpgradeText); // 업그레이드 텍스트 등록
        OnUpgrade.AddListener(UpgradeProcess); // 업그레이드 프로세스 등록
    }

    // Update is called once per frame
    void Update()
    {
        if (OnUpgrade != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnUpgrade.Invoke();
            }
        }
        else
        {
            Debug.Log("등록된 비영구 리스너가 없습니다.");
            //비영구 리스너 : 스크립트를 통해서 (AddListener)를 통해서 추가한 리스너
            //                RemoveListener를 통해서 제거가 가능합니다.
            //영구 리스너 : 인스펙터를 통해서 추가한 리스너
            //              이건 인스펙터를 통해 직접 지워야 합니다.
        }
    }

    IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
