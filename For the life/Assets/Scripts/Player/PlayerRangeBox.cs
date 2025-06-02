using UnityEngine;

public class PlayerRangeBox : MonoBehaviour
{
    private Player player; //플레이어 스크립트 참조
    public bool isRange = false; // 타겟 범위 존재
    public bool isLongReach = true; // false : 단거리, true : 원거리

    private void Awake()
    {
        player = GetComponentInParent<Player>(); //부모 오브젝트에서 Player 스크립트를 찾음
    }

    private void OnEnable()
    {
        isRange = false; //범위 초기화
        isLongReach = true; //원거리 여부 초기화
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (player.target == null)
        {
            if(other.gameObject.CompareTag("Enemy")) //Enemy 태그를 가진 오브젝트와 충돌했을 때
            {
                isRange = true; //타겟 범위 존재 여부를 true로 설정
                //여기 자체로 타겟을 정하는 것도 좋았을 듯
            }
        }

        if (other.gameObject == player.target) //Enemy 태그를 가진 오브젝트와 충돌했을 때
        {
            isRange = true; //타겟 범위 존재 여부를 true로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player.target) //Enemy 태그를 가진 오브젝트와 충돌이 끝났을 때
        {
            isRange = false; //타겟 범위 존재 여부를 false로 설정
        }
    }
}
