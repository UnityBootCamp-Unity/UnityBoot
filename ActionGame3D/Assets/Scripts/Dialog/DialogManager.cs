using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialog
{
    public enum DialogType
    {
        Alert, Confirm
    }

    public sealed class DialogManager : MonoBehaviour
    {
        List<DialogData> _dialogList;                       //다이얼로그 데이터 리스트
        Dictionary<DialogType, DialogController> _dialogdict; //다이얼로그 타입에 맞는 컨트롤러(딕셔너리)
        DialogController _currentDialogController;                    //현재의 다이얼로그 컨트롤러

        private static DialogManager instance = new DialogManager();


        public static DialogManager Instance
        {
            get { return instance; }
        }
        //클래스의 인스턴스가 생성될 때, 리스트와 딕셔너리를 초기화합니다.
        public DialogManager()
        {
            _dialogList = new List<DialogData>();
            _dialogdict = new Dictionary<DialogType, DialogController>();
        }

        public void Regist(DialogType type, DialogController controller)
        {
            _dialogdict[type] = controller;
        }

        public void Push(DialogData data)
        {
            _dialogList.Add(data);

            if(_currentDialogController == null)
            {
                ShowNext();
            }
        }
        public void Pop()
        {
            //현재 열려있는 대화창을 닫고, 남아있는 다음 대화창을 보여주는 구조
            if(_currentDialogController != null)
            {
                _currentDialogController.Close(
                    delegate
                    {
                        _currentDialogController = null;

                        if (_dialogList.Count > 0)
                        {
                            ShowNext();
                        }
                    });
            }
        }

        private void ShowNext()
        {
            DialogData next = _dialogList[0]; //리스트의 첫번째 값 지정

            //해당 값을 딕셔너리에 키로써 접근해, 컨트롤러를 조회합니다.
            DialogController controller = _dialogdict[next.Type].GetComponent<DialogController>();

            _currentDialogController = controller;

            _currentDialogController.Build(next);

            _currentDialogController.Show(delegate { } ); //대화창을 띄우되 따로 후처리를 하지 않는다.

            _dialogList.RemoveAt(0); //리스트에서 첫번째 값 제거
        }

        public bool IsShowing()
        {
            return _currentDialogController != null;
        }
    }
}
