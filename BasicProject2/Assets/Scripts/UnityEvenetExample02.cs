using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnityEvenetExample02 : MonoBehaviour
{
    public UnityEvent Onquest; //퀘스트
    public Text clear_count_text; //클리어 개수
    public Text current_count_text; //현재 개수
    public Text message; //메세지

    public int clear_count = 10;
    public int current_count = 0;

    void countPlus() => current_count++; //개수 증가(1개)

    void countText()
    {
        current_count_text.text = $"{current_count} 개";
        message.text = $"[퀘스트 진행 중...] 남은 개수 {current_count} / {clear_count} 개";
    }

    void QuestClear()
    {
        if(current_count == clear_count)
        {
            clear_count_text.text = "끝";
            current_count_text.text = "끝";
            message.text = "퀘스트 완료";
            clear_count_text.color = Color.gray;
            current_count_text.color = Color.gray;
            message.color = Color.cyan;
            Onquest.RemoveAllListeners(); //등록된 기능 전부 제거
            Onquest = null;
        }
        else
        {
            countText();
        }
        
    }

    void Start()
    {
        //AddListener로 추가한 이벤트는 인스펙터에서는 확인이 불가합니다.
        Onquest.AddListener(countPlus);
        //유니티 이벤트에 countPlus를 등록합니다.
        Onquest.AddListener(QuestClear);
    }

    void Update()
    {
        if(Onquest != null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Onquest.Invoke();
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
}
