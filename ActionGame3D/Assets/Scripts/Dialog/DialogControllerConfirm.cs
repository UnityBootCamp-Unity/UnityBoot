using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.Dialog
{
    public class DialogControllerConfirm : DialogController
    {
        //컴포넌트
        //1. 제목(Text)
        //2. 내용(Text)

        public Text LabelTitle;
        public Text LabelContent;

        //프로퍼티
        //DialogDataConfirm
        DialogDataConfirm Data { get; set; }

        //오버라이드
        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            //매니저에 등록
            DialogManager.Instance.Regist(DialogType.Confirm, this);
        }

        public override void Build(DialogData data)
        {
            base.Build(data);

            //데이터 여부 확인
            if (!(data is DialogDataConfirm))
            {
                Debug.LogError("Invalid dialog Data!");
                return;
            }


            //메세지 등록
            Data = data as DialogDataConfirm;
            LabelTitle.text = Data.Title;
            LabelContent.text = Data.Message;
        }

        public void OnYesButtonClick()
        {
            Debug.Log("YES 버튼 눌림");

            //콜백 호출
            if (Data != null && Data.Callback != null)
            {
                Data.Callback(true);
            }

            //Pop
            DialogManager.Instance.Pop();
        }

        public void OnNoButtonClick()
        {
            //콜백 호출
            if (Data != null && Data.Callback != null)
            {
                Data.Callback(false);
            }

            //Pop
            DialogManager.Instance.Pop();
        }

    }

}