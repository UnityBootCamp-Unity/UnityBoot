using System;
using UnityEngine;
namespace Assets.Scripts.Dialog
{
    public class DialogDataAlert : DialogData
    {
        //프로퍼티 설명
        //1. 제목
        //2. 메세지
        //3. 버튼 누를 시의 콜백 함수

        //해당 데이터의 프로퍼티들은 클래스 내부에서만 수정이 가능합니다.

        public string Title { get; private set; }

        public string Message { get; private set; }

        public Action Callback { get; private set; }

        //클래스 생성 시 변수 전달)
        public DialogDataAlert(string title, string message, Action callback = null) : base(DialogType.Alert)
        {
                Title = title;
                Message = message;
                Callback = callback;
        }
 
    }

}