using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Item_Object : MonoBehaviour
{
    public Transform ItemText;
    public Text text; //TMP 쓰시는 분들은 TMP로 설정

    public float angle = 45.0f;
    public float gravity = 9.8f;
    public float range = 2.0f;

    bool ischeck = false;

    //아이템 레어도 별로 처리하는 코드
     void ItemRare()
    {
        ischeck = true;
        transform.rotation = Quaternion.identity; //그대로 값 넘어가도록 회전 값 0
        //아이템 텍스트 활성화
        ItemText.gameObject.SetActive(true);
        ItemText.parent = B_Canvas.instance.GetLayer(2);
        text.text = "아이템"; //아이템 이름 설정
    }

    private void Update()
    {
        if (ischeck == false)
            return;

        ItemText.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        //전달받은 값을 기준으로 그 주변에 위치할 수 있도록 범위 설정
        Vector3 item_pos = new Vector3
            (
            pos.x + (Random.insideUnitSphere.x * range),
            0.0f,   //생성 위치
            pos.z + (Random.insideUnitSphere.z * range)
            );
        //이 기능을 몬스터 쪽의 사망 시 판정에서 작업 진행
        //물체 이동 시작
        StartCoroutine(Simulate(pos));
    }

    IEnumerator Simulate(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);
        float radian = angle * Mathf.Deg2Rad; //라디안 변환 값
        float velocity = Mathf.Sqrt(target_Distance * gravity / Mathf.Sin(2 * radian));

        float vx = velocity * Mathf.Cos(radian);
        float vy = velocity * Mathf.Sin(radian);

        float duration = target_Distance / vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);
        //LookAt 처럼 회전 방향 바라보게 만드는 코드

        float simulate_time = 0.0f;

        while(simulate_time < duration)
        {
            simulate_time += Time.deltaTime;

            //시간이 지날수록 위에서 점점 아래로, 밑변 방향으로 이동
            transform.Translate(0, (vy - (gravity * simulate_time)), vx * Time.deltaTime);

            yield return null;
        }

        //아이템 이동 시뮬레이션이 끝나면 레어도 체크 후 화면에 아이템 이름 띄우기
        ItemRare();

    }
    /*IEnumerator Simulate(Vector3 pos)
    {
        //[타겟의 거리]
        var targetDistance = Vector3.Distance(transform.position, pos);

        //곡선에 대한 설정
        var velocity = targetDistance / (Mathf.Sin(angle * Mathf.Deg2Rad) / gravity);

        //Mathf.Sin : 삼각함수 중에서 Sin 값을 반환하는 기능

        //삼각형 기준으로 가로 세로를 각각 w,h라고할 때 , Sin h / a(빗변)을 계산하는 식
        //유니티에서 각이 45도일 경우 빗변의 길이가 1인 삼각형이 만들어집니다.(유니티 자체 로직)

        //Mathf.Sin(45 * Mathf.Deg2Rad) ==> 빗변의 길이가 1이고 각도가 45도인 삼각형의 높이(h)를 리턴합니다.

        //Mathf.Cos(45 * Mathf.Deg2Rad) ==> 빗변의 길이가 1이고 각도가 45도인 삼각형의 밑변(w)를 리턴합니다.

        //Deg2Rad는 도(Degree) --> 라디안(Radian)으로 변경해주는 코드

        //사용 이유 : 유니티에서 sin, cos 함수를 계산할 때 각도 단위를 라디안(radian)으로 사용하기 때문

        //1. 각도 계산을 진행한다.
        //2. 타겟의 거리와 중력 값을 통해 계산한 값이 천천히 움직이게끔 한다.

        float sx = Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad); //밑변 값
        float sy = Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad); //높이 값

        //시간(밑변의 위치까지 이동할 거리 값)
        float duration = targetDistance / sx; //10

        //로직 적용을 위한 시간 체크
        float time = 0.0f;

        while(time < duration)
        {
            //이 로직 진행 동안 아이템의 위치를 이전시킵니다.
            transform.Translate(0, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }
    }*/
}
