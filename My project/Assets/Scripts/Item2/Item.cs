using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Item2
{
	public class Item : MonoBehaviour
	{
        public ItemData data;

        //public 기능을 쓰면서 에디터에서는 보이지 않게 처리
        [HideInInspector] public Rigidbody2D rbody;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }
    }
}