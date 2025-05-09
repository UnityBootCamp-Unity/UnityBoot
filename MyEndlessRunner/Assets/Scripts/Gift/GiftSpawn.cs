using System.Collections;
using UnityEngine;

public class GiftSpawn : MonoBehaviour
{


    //투사체 관련
    public GameObject giftPrefab; // 투사체 프리팹
    public Transform spawnPoint; // 생성 위치
    [SerializeField] private float cooldown = 0.5f; // 쿨타임 (초)
    private bool isShooting = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isShooting == false){
            StartCoroutine(CooldownGift());
        }
    }

    public void DropGift()
    {
        Instantiate(giftPrefab, spawnPoint.position, Quaternion.identity);
    }

    IEnumerator CooldownGift()
    {
        DropGift();
        isShooting = true;
        yield return new WaitForSeconds(cooldown);
        isShooting = false;
    }

}
