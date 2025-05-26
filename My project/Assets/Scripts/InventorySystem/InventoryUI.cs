using System;
using System.Collections.Generic;
using Assets.Scripts.Item2;
using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject inventory; //인벤토리 창

        public Player player; //플레이어 등록

        public List<SlotUI> slots = new List<SlotUI>();

        private void Update()
        {
            //버튼 누르면 인벤토리 키는 기능
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnOFF();
            }

            if (inventory.activeSelf)
            {
                //인벤토리 창이 켜져있을 경우 슬롯 갱신
                SlotRenewal();
            }
        }

        public void OnOFF()
        {
            SlotRenewal();
            //켜짐 여부에 따라 true와 false로 인벤토리를 키거나 끕니다.
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                //켯을 경우 UI에 대한 갱신 진행
            }
            else
            {
                inventory.SetActive(true);
            }
        }

        //슬롯에 대한 갱신
        private void SlotRenewal()
        {
            if(slots.Count == player.Inventory.slots.Count)
            {
                for(int i = 0; i < slots.Count; i++)
                {
                    if (player.Inventory.slots[i].item_name != "")
                    {
                        //슬롯에 이미지와 개수 등을 갱신한다.
                        slots[i].SetSlot(player.Inventory.slots[i]);
                    }
                    else
                    {
                        slots[i].SetEmpty();
                    }
                }
            }
        }

        public void Remove(int slot_idx)
        {
            //수확물
            Item drop = GameManager.instance.ItemManager.
                GetItem(player.Inventory.slots[slot_idx].item_name);

            Debug.Log("Slot Type: " + player.Inventory.slots[slot_idx].item_name);

            if (drop != null)
            {
                player.Drop(drop);
                player.Inventory.Remove(slot_idx);
                SlotRenewal();
            }

        }
    }
}