using System.Collections.Generic;
using UnityEngine;
//1. 타일이 생성됩니다.
//2. 플레이어가 이동합니다.
//3. 기존에 있던 (이미 건넌) 타일을 제거합니다.
//4. 월드에 타일의 개수가 균일하게 유지됩니다.

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;            //등록할 타일
    private List<GameObject> tiles;             //타일 리스트

    private Transform player_transform;         //플레이어 위치
    private float spawnZ = 0.0f;                //스폰(Z축)
    private float tileLength = 55.0f;            //타일의 길이
    private float passZone = 80.0f;             //타일 유지 거리
    private int tile_on_screen = 7;             //화면에 배치할 타일 개수



    void Start()
    {
        tiles = new List<GameObject>();
        //타일 리스트 생성
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        //플레이어 씬에서 태그 검색해서 트랜스폼 적용

        for (int i = 0; i < tile_on_screen; i++)
        {
            Spawn();
        }
    }

    void Update()
    {
        //플레이어가 일정 거리 이상 이동하게 되면 타일을 생성하고 , 지나갓던 타일을 제거합니다.
        if (player_transform.position.z - passZone > (spawnZ - tile_on_screen * tileLength))
        {
            Spawn();
            Release();
        }
    }

    private void Spawn()
    {
        var go = Instantiate(tilePrefabs[0]);
        //고정된 값을 생성(이후에는 랜덤이나 패턴화로 변경)
        go.transform.SetParent(transform);
        //만들어진 타일은 타일 매니저의 자식 오브젝트가 됩니다.
        go.transform.position = new Vector3(0, -0.5f, spawnZ);
        //만들어진 타일의 위치를 설정합니다.
        spawnZ += tileLength;
        //생성 위치가 타일 길이 기준으로 계속 증가(크기에 맞게 생성)
        tiles.Add(go);
        //타일 리스트에 등록
    }

    private void Release()
    {
        Destroy(tiles[0]);
        //가장 앞에 있는 타일을 제거합니다.
        tiles.RemoveAt(0);
        //타일 리스트의 맨 앞의 값을 제거합니다.
    }
}
