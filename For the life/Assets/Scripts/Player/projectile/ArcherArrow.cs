using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    [Header("타겟")]
    public GameObject target;
    [Header("속도")]
    public float speed = 10f;
    [Header("데미지")]
    public int damage = 5;

    private void OnEnable()
    {
        Player player = GetComponentInParent<Player>();

        Debug.Log("target: " + target);
        if (target == null)
        {
            Debug.Log("target is null -> trying to find closest target");
            target = FindClosestTarget();
        }
        else
        {
            Debug.Log("target is NOT null: " + target.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                // 적에게 데미지 적용
                //enemy.TakeDamage(damage);
                enemy.currentHp -= damage;
            }
            // 화살 제거
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private Rigidbody2D rb;

    /*void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, 5f); // 수명 제한
    }*/

    private void Update()
    {
        // 타겟이 없으면 비활성화
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // 타겟 방향으로 회전
        Vector2 direction = (target.transform.position - transform.position).normalized;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);*/

        /*float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        // 속도에 따라 이동
        Vector2 moveDirection = direction * speed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);
    }

    GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //적 오브젝트를 찾음
        GameObject closestEnemy = null; //가장 가까운 적 초기화
        float closestDistance = Mathf.Infinity; //가장 가까운 거리 초기화
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript == null || enemyScript.currentHp <= 0) //적이 없거나 HP가 0 이하인 경우 무시
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position); //플레이어와 적 사이의 거리 계산
            if (distance < closestDistance) //가장 가까운 적을 찾음
            {
                closestDistance = distance; //가장 가까운 거리 업데이트
                closestEnemy = enemy; //가장 가까운 적 업데이트
            }
        }

        return closestEnemy; //가장 가까운 적 반환
    }
}
