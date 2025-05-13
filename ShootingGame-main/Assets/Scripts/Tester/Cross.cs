using UnityEngine;

public class Cross : MonoBehaviour
{
    public GameObject prefab;

    public int length;
    public float space = 1.0f;

    private void Start()
    {

        for (int i = -length; i <= length; i++)
        {
            Vector3 pos = transform.position + new Vector3(i * space, 0, 0);
            var go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.parent = transform;
        }

        for (int i = -length; i <= length; i++)
        {
            Vector3 pos = transform.position + new Vector3(0, i * space,0);
            var go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.parent = transform;
        }

    }
}
