using System;
using System.Collections;
using UnityEditor.PackageManager.UI;
using UnityEngine;
namespace Assets.Scripts.Dialog
{
    public class DialogController : MonoBehaviour
    {
        public Transform window; //ÆË¾÷Ã¢ÀÇ Transform

        public bool Visible
        {
            get
            {
                return window.gameObject.activeSelf;
            }
            private set
            {window.gameObject.SetActive(value);

            }
        }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Build(DialogData data) { }

        public void Show(Action callback)
        {
            StartCoroutine(OnEnter(callback));
        }

        public void Close(Action callback)
        {
            StartCoroutine(OnExit(callback));
        }

        IEnumerator OnEnter(Action callback)
        {
            Visible = true;

            if(callback != null)
            {
                callback();
            }
            yield break;
        }

        IEnumerator OnExit(Action callback)
        {
            Visible = false;

            if (callback != null)
            {
                callback();
            }
            yield break;
        }
    }
}