using System.Collections;
using UnityEngine;

public class GiftSpawn : MonoBehaviour
{


    //����ü ����
    public GameObject giftPrefab; // ����ü ������
    public Transform spawnPoint; // ���� ��ġ
    [SerializeField] private float cooldown = 0.5f; // ��Ÿ�� (��)
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
