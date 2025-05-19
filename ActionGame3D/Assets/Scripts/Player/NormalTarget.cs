using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    //공격 타겟에 대한 리스트
    public List<Collider> targetList;

    private void Awake()
    {
        targetList = new List<Collider>();
    }

    //몬스터가 공격 반경으로 들어오면, 리스트 추가
    private void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Add(other);
    }

    //몬스터가 공격 반경에서 벗어나면 리스트 제거
    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other) && other.CompareTag("Enemy"))
            targetList.Remove(other);
    }

    //로직 연산(Update) 이후에 잘못된 참조에 대한 유효성 검사
    //오류 방지를 위함, 타겟 검색에 대한 안전함, 유니티 구조 상 콜라이더나 오브젝트 같은 컴포넌트들이
    //Destroy 될 때 이벤트 함수가 호출이 안되는 타이밍이 존재할 수 있음
    private void LateUpdate()
    {
        targetList.RemoveAll(target => target == null);
        //리스트 문법
        //1. targetList.Contains(value)
        // 리스트에 해당 값이 포함되고 있는지를 확인합니다.

        //2. targetList.Remove(value);
        //리스트에 해당 값을 제거합니다.

        //3. targetList.Add(value);
        //리스트에 해당 값을 추가합니다. 추가한 값은 리스트의 마지막 값

        //4. targetList.RemoveAll(Predicate<T>);
        //제거 조건에 대한 대리자를 넣고, 리스트에 대한 전체 제거를 진행합니다.
        //현재 해당 코드의 조건은 콜라이더인 target이 null 상태인지를
        //체크하는 코드입니다. null일 경우 항복만 제거됩니다.
    }
}
