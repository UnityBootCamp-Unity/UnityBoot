using UnityEngine;

//오브젝트를 원의 형태로 배치하는 코드

public class Circle : MonoBehaviour
{
    public GameObject prefab;
    public int count = 20;
    public float radius = 5.0f;
    
    void Start()
    {
        //카운트만큼 실행
        for (int i = 0; i < count; i++)
        {
            float radian = i * Mathf.PI * 2 / count; 
            float x = Mathf.Cos(radian) * radius;
            float z = Mathf.Sin(radian) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float degree = -radian * Mathf.Rad2Deg; //오브젝트가 중앙을 바라보도록
            Quaternion rotation = Quaternion.Euler(0, degree, 0);
            Instantiate(prefab, pos, rotation);
        }
    }
}
