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
    public Sprite icon; //이미지 등록

    TileManager tileManager;
    public Sprite[] growns;

    public float time = 0;

    private void Start()
    {
        tileManager = GameManager.instance.TileManager;
    }

    private void Update()
    {
        //시간 측정
        time += Time.deltaTime;

        //일정 시간마다 성장
        if(time >= 30 && (int)grown < 3)
        {
            grown = (Grown)((int)grown + 1); //enum의 1칸 이동
            icon = growns[(int)grown]; //변경된 enum의 값으로 아이콘 설정
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
        //1. 태그 조사
        if (collision.CompareTag("Player"))
        {
            //var player = collision.GetComponent<PlayerMovement>();
            //player.stat.count_of_harvest++;

            //2. 플레이어 클래스 확인
            var player = collision.GetComponent<Player>();

            var item = GetComponent<Item>();

            if (item != null)
            {
                //3. 플레이어가 가진 인벤토리에 추가
                player.Inventory.Add(item);
                Destroy(gameObject);
            }
        }
    }
}
