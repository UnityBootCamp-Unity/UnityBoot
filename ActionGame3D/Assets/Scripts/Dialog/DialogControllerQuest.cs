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

        //������ ���� Ȯ��
        if (!(data is DialogDataQuest))
        {
            Debug.LogError("Invalid dialog Data!");
            return;
        }


        //�޼��� ���
        Data = data as DialogDataQuest;
        LabelTitle.text = Data.Title;
        LabelContent.text = Data.Message;
    }

    public void OnOKButtonClick()
    {
        //�����͵� �ְ�, �ݹ鵵 �䫊�ߴٸ�
        if (Data != null && Data.Callback != null)
        {
            Data.Callback();
        }

        //�ݹ� ����, �Ŵ������� ����
        DialogManager.Instance.Pop();
    }

}
