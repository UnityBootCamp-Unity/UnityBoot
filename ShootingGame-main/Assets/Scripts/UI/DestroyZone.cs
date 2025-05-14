using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.gameObject.name.Contains("Bullet"))
        {
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.name.Contains("Enemy"))
        {
            other.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
