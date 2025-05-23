using UnityEngine;
using Assets.Scripts.InventorySystem;

public class Player : MonoBehaviour
{
    public Inventory Inventory;

    private void Awake()
    {
        //기본 인벤토리 4개 제송
        Inventory = new Inventory(4);
    }
}