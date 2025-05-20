using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    public class DialogControllerAlert : DialogController
    {
        //컴포넌트 연결
        //1. 타이틀 Text
        //2. 내용 Text
        public Text LabelTitle;
        public Text LabelContent;

        //프로퍼티
        //알림 창의 데이터
        DialogDataAlert Data { get; set; }

        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            //매니저에 정보 전달(타입 : Alert, 컨트롤러 : 자기 자신(Controller Alert))
            DialogManager.Instance.Regist(DialogType.Alert, this);
        }

        public override void Build(DialogData data)
        {
            base.Build(data);

            //데이터가 DialogDataAlert 형태가 아닌 상태에서 빌드를 진행할 경우
            if(! (data is DialogDataAlert))
            {
                Debug.LogError("Invalid dialog Data!");
                return;
            }

            Data = data as DialogDataAlert;
            LabelTitle.text = Data.Title;
            LabelContent.text = Data.Message;
        }

        public void OnOKButtonClick()
        {
            //데이터도 있고, 콜백도 요쳥했다면
            if(Data != null && Data.Callback != null)
            {
                Data.Callback();
            }

            //콜백 이후, 매니저에서 제거
            DialogManager.Instance.Pop();
        }

    }
}