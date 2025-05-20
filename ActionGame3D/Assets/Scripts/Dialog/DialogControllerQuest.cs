using Assets.Scripts.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class DialogControllerQuest : DialogController
{
    public Text LabelTitle;
    public Text LabelContent;

    DialogDataQuest Data { get; set; }

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        DialogManager.Instance.Regist(DialogType.Quest, this);
    }

    public override void Build(DialogData data)
    {
        base.Build(data);

        //데이터 여부 확인
        if (!(data is DialogDataQuest))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }


        //메세지 등록
        Data = data as DialogDataQuest;
        LabelTitle.text = Data.Title;
        LabelContent.text = Data.Message;
    }

    public void OnOKButtonClick()
    {
        //데이터도 있고, 콜백도 요쳥했다면
        if (Data != null && Data.Callback != null)
        {
            Data.Callback();
        }

        //콜백 이후, 매니저에서 제거
        DialogManager.Instance.Pop();
    }

}
