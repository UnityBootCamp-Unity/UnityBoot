using UnityEngine;

public enum CollectType
{
    None, SEED
}

public class Harvest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerMovement>();
            player.stat.count_of_harvest++;
            Destroy(gameObject);
        }
    }
}
