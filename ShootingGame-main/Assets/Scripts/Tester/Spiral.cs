using UnityEngine;

public class Spiral : MonoBehaviour
{
    public GameObject prefab; //오브젝트
    public int count = 50;    //개수
    public float degree = 10; //한번당 각도
    public float radius = 0.1f; //한번당 반지름
    
    void Start()
    {
        float d = 0.0f; //시작 각도
        float r = 0.0f; //시작 반지름

        for(int i = 0; i < count; i++) //카운트만큼 작업
        {
            var radian = d * Mathf.Deg2Rad; // 라디안 

            var x = Mathf.Cos(radian) * r;   //x
            var y = Mathf.Sin(radian) * r;   //y

            var pos = transform.position + new Vector3(x, y, 0); //계산 각 더해주기
            var go = Instantiate(prefab, pos, Quaternion.identity);
            go.transform.parent = transform;

            d += degree; //설정한 한번 당 각도 추가
            r += radius; //설정한 한번 당 각도 추가 
        }
    }

    
    
}
