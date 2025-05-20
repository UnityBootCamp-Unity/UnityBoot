using UnityEngine;

namespace Assets.Scripts.Dialog
{    


    public class DialogData
    {
        public DialogType Type { get; set; }

        public DialogData(DialogType type)
        {
            Type = type;
        }
    }
}
