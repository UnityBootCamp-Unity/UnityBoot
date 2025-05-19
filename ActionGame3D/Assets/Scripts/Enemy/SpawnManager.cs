using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int pollSize = 10;
    public GameObject object_enemy;
    public GameObject[] poll_enemy;

    private float duration = 5.0f;

    private void Start()
    {
        poll_enemy = new GameObject[pollSize];

        for (int i = 0; i < pollSize; i++)
        {
            var enemy = Instantiate(object_enemy);
            poll_enemy[i] = enemy;
            enemy.SetActive(false);
        }
    }

    private void Update()
    {

        if (duration <= 0)
        {
            for (int i = 0; i < pollSize; i++)
            {
                var enemy = poll_enemy[i];

                if (enemy.activeSelf == false)
                {
                    enemy.SetActive(true);

                    enemy.transform.position = this.transform.position;

                    break;
                }
            }
            duration = 5.0f;
        }

        duration -= Time.deltaTime;
    }
}
