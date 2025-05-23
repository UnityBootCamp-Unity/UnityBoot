using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.InventorySystem
{
    public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image slot_icon;
        public Text slot_count_text;

        private Slot currentSlot;

        //슬롯 적용
        public void SetSlot(Slot slot)
        {
            currentSlot = slot;

            if (slot != null)
            {
                slot_icon.sprite = slot.icon;
                slot_count_text.text = slot.count.ToString();
            }
        }

        //빈 슬롯 설정
        public void SetEmpty()
        {
            currentSlot = null;
            slot_icon.sprite = null;
            slot_count_text.text = "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentSlot != null)
            {
                ItemUI.Instance.Show(currentSlot.item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemUI.Instance.Hide();
        }
    }
}
