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
    //        //���� ��ǥ ����
    //        var world = transform.position;

    //        //���� -> ��
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
            // �����̾�� �浹 �� ó��
            Debug.Log("Slayer collided with Slash");
            // �߰����� ������ ���⿡ �ۼ��� �� �ֽ��ϴ�.
        }
    }

}
