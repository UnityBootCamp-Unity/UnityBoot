using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    Transform player; //플레이어 위치
    NavMeshAgent nav; //네비메쉬 에이전트

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(player.position);
        }
    }
}
