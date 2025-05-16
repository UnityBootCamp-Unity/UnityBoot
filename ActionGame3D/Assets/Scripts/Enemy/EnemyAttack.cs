using UnityEditor;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject player;              //플레이어(추적)
    PlayerHealth playerHealth;      //플레이어 체력(데미지 전달)
    EnemyHealth enemyHealth;        //슬라임 체력(데미지 전달)
    bool playerInRange;             //플레이어와의 거리
    float timer;                    //시간 체크

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        //시간 체크
        timer += Time.deltaTime;

        //체크한 시간이 대기시간 이상이고 플레이어가 범위 내에 있고 내 체력이 남아있다면
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth >0 )
        {
            Attack(); //공격!!!
        }
    }

    private void Attack()
    {
        timer = 0; //타임 리셋(다시 공격할 수 있도록)

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }
}
