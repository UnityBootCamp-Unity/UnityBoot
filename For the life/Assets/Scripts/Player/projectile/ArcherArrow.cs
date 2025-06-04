using UnityEngine;

public class ArcherArrow : MonoBehaviour
{
    [Header("Ÿ��")]
    public GameObject target;
    [Header("�ӵ�")]
    public float speed = 10f;
    [Header("������")]
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
                // ������ ������ ����
                //enemy.TakeDamage(damage);
                enemy.currentHp -= damage;
            }
            // ȭ�� ����
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private Rigidbody2D rb;

    /*void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, 5f); // ���� ����
    }*/

    private void Update()
    {
        // Ÿ���� ������ ��Ȱ��ȭ
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // Ÿ�� �������� ȸ��
        Vector2 direction = (target.transform.position - transform.position).normalized;
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);*/

        /*float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        // �ӵ��� ���� �̵�
        Vector2 moveDirection = direction * speed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);
    }

    GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //�� ������Ʈ�� ã��
        GameObject closestEnemy = null; //���� ����� �� �ʱ�ȭ
        float closestDistance = Mathf.Infinity; //���� ����� �Ÿ� �ʱ�ȭ
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript == null || enemyScript.currentHp <= 0) //���� ���ų� HP�� 0 ������ ��� ����
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position); //�÷��̾�� �� ������ �Ÿ� ���
            if (distance < closestDistance) //���� ����� ���� ã��
            {
                closestDistance = distance; //���� ����� �Ÿ� ������Ʈ
                closestEnemy = enemy; //���� ����� �� ������Ʈ
            }
        }

        return closestEnemy; //���� ����� �� ��ȯ
    }
}
