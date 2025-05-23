using System;
using System.Collections.Generic;

namespace Assets.Scripts.InventorySystem
{
    [Serializable]
    public class Inventory
    {
        public List<Slot> slots = new List<Slot>();

        //인벤토리 생성 시 count만큼의 슬롯을 추가합니다.
        public Inventory(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Slot slot = new Slot();

                slots.Add(slot);
            }
        }

        //추가 기능(추가 가능 상태에 대한 확인 후 등록)
        public void Add(Harvest harvest)
        {
            //foreach ? or for
            //1. 일반적으로는 for문 추천
            // --> 복붙이 쉬움.
            // --> 배열 사용 시에 가장 보편적으로 고려되는 방식

            //2. foreach
            //: 순서의 개념이 있는 묶음에 대한 작업(문자열, 배열, 특정 컬렉션)
            //  작업은 순차적으로 진행되기 때문에 단순 동일 작업 진행 시 효과적입니다.
            //  인덱스 개념을 쓸 수 없어서, 설계에 불편함이 존재할 수 있습니다.
            //  단 반복 과정에서 그 값에 대한 설정으로 따로 만들면 상관은 없음.

            //1) 아이템 추가 가능 여부 체크한 슬롯 등록
            foreach (var slot in slots)
            {
                if(slot.type == harvest.type && slot.Addable())
                {
                    slot.Add(harvest);
                    return;
                }
            }
            //타입 None에 대한 처리
            foreach (var slot in slots)
            {
                if (slot.type == CollectType.None)
                {
                    slot.Add(harvest);
                    return;
                }
            }
        }

        public void Remove(int idx)
        {
            slots[idx].Remove();
        }
    }

}
