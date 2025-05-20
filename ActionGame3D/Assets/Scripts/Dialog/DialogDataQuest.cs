using System;
using Assets.Scripts.Dialog;
using UnityEngine;

public class DialogDataQuest : DialogData
{

    public string Title { get; private set; }
    public string Message { get; private set; }
    public Action Callback { get; private set; }

    public DialogDataQuest(string title, string message, Action callback = null) : base(DialogType.Quest)
    {
        Title = title;
        Message = message;
        Callback = callback;
    }
}
