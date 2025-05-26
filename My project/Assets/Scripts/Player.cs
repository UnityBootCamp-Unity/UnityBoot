using UnityEngine;
using Assets.Scripts.InventorySystem;
using Assets.Scripts.Manager;
using Assets.Scripts.Item2;

public class Player : MonoBehaviour
{
    public Inventory Inventory;
    private TileManager tileManager;
    private void Awake()
    {
        //기본 인벤토리 4개 제송
        Inventory = new Inventory(4);
        tileManager = GameManager.instance.TileManager;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(tileManager != null)
            {
                //월드 좌표 설정
                var world = transform.position;

                //월드 -> 셀
                var grid = tileManager.Interactables.WorldToCell(world);

                //var position = new Vector3Int((int)transform.position.x,
                //                        (int)transform.position.y, 0);

                if (GameManager.instance.TileManager.isInteractable(grid))
                {
                    Debug.Log("check");
                    GameManager.instance.TileManager.SetInteract(grid);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //채집 기능
        }
    }

    public void Drop(Item item)
    {
        //위치 설정
        var spawn = transform.position;

        //던지는 범위
        float x = 5.0f;
        Vector3 offset = new Vector3(x, 0, 0);

        //오브젝트 생성
        var go = Instantiate(item, spawn + offset, Quaternion.identity);

        //오브젝트에 대한 물리적인 힘 작용
        //go.rbody.AddForce(spawn * 2f, ForceMode2D.Impulse);
    }
}