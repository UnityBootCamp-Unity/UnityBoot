using UnityEngine;

public class GiftCollider : MonoBehaviour
{

    public ScoreManager scoreManager;

    //Collider isTrigger ��Ȱ��ȭ
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("chimney"))
        {
            gameObject.SetActive(false);
        }

    }*/

    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        }
    }

    //Collider isTrigger Ȱ��ȭ
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("chimney"))
        {
            Destroy(gameObject);
            scoreManager.UpScore();
            //gameObject.SetActive(false);
        }
    }
}
