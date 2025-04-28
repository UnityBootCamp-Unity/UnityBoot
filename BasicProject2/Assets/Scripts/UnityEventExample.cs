using UnityEngine;
using UnityEngine.Events;

//유니티에서 사용할 수 있는 대리자 기능 중 하나

//Action이나 Func는 C# 형식의 대리자
//이 값들은 인스펙터에 노출되지 않습니다.

public class UnityEventExample : MonoBehaviour
{
    public UnityEvent onSample;

    private void Update()
    {
        //onSample에 실행할 기능이 등록된 상태에서 A 키를 눌렀을 경우
        if (Input.GetKeyDown(KeyCode.A) && onSample != null)
        {
            //그 기능을 실행한다.
            onSample.Invoke();
        }
    }
}
