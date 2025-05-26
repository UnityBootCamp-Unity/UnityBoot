using Assets.Scripts.Item2;
using Assets.Scripts.Manager;
using UnityEngine;

public enum Grown
{
    Seed, Sprout, Growing, Mature
}

public class Harvest : MonoBehaviour
{
    public Grown grown;
    public Sprite icon; //�̹��� ���

    TileManager tileManager;
    public Sprite[] growns;

    public float time = 0;

    private void Start()
    {
        tileManager = GameManager.instance.TileManager;
    }

    private void Update()
    {
        //�ð� ����
        time += Time.deltaTime;

        //���� �ð����� ����
        if(time >= 30 && (int)grown < 3)
        {
            grown = (Grown)((int)grown + 1); //enum�� 1ĭ �̵�
            icon = growns[(int)grown]; //����� enum�� ������ ������ ����
            time = 0;
        }

        SetHarvest(icon);
    }

    private void SetHarvest(Sprite icon)
    {
        GetComponent<SpriteRenderer>().sprite = icon;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //1. �±� ����
        if (collision.CompareTag("Player"))
        {
            //var player = collision.GetComponent<PlayerMovement>();
            //player.stat.count_of_harvest++;

            //2. �÷��̾� Ŭ���� Ȯ��
            var player = collision.GetComponent<Player>();

            var item = GetComponent<Item>();

            if (item != null)
            {
                //3. �÷��̾ ���� �κ��丮�� �߰�
                player.Inventory.Add(item);
                Destroy(gameObject);
            }
        }
    }
}
