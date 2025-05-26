using Assets.Scripts.Manager;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private TileManager tileManager;

    private void Start()
    {
        tileManager = GameManager.instance.TileManager;
    }

    //private void Update()
    //{
    //    if (tileManager != null)
    //    {
    //        //월드 좌표 설정
    //        var world = transform.position;

    //        //월드 -> 셀
    //        var grid = tileManager.Interactables.WorldToCell(world);

    //        //var position = new Vector3Int((int)transform.position.x,
    //        //                        (int)transform.position.y, 0);

    //        if (GameManager.instance.TileManager.isInteractable(grid))
    //        {
    //            Debug.Log("check");
    //            GameManager.instance.TileManager.SetGrass(grid);
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Slayer"))
        {
            // 슬레이어와 충돌 시 처리
            Debug.Log("Slayer collided with Slash");
            // 추가적인 로직을 여기에 작성할 수 있습니다.
        }
    }

}
