using System;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private RectTransform _touchPad; //UI 위치는 RectTransform

    private int _touchId = -1; //방향 컨트롤러 영역 안에 있는 입력 구분용 아이디

    private Vector3 _startPos = Vector3.zero; //입력 시작 좌표

    public float _dragRadius = 60.0f; //방향 컨트롤러 원 움직임용 반지름

    public PlayerMovement _player; //방향키 변경에 따라 캐릭터에게 전달

    private bool _buttonPressed = false; //버튼 눌림 여부

    private void Start()
    {
        //오브젝트 연결
        _touchPad = GetComponent<RectTransform>();
        //터치 패드 좌표 (기준 값)
        _startPos = _touchPad.position;
    }

    public void ButtonDown()
    {
        _buttonPressed = true;
    }
    public void ButtonUp() => _buttonPressed = false;

    private void FixedUpdate()
    {
        //크로스 플랫폼 : 플랫폼 따라 코드 나누기

        //PC나 에디터 상에서 실행하는 경우에 대한 설정
        #if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE
        HandleTouchInput(Input.mousePosition);
        HandleKeyboardInput(); // 키보드 입력 처리 추가
#else
            HandleTouchInput();
#endif
    }

    private void HandleKeyboardInput()
    {
        if (_buttonPressed) return; // 터치 중이면 키보드 입력 무시

        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or ←/→
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or ↑/↓

        Vector2 inputDir = new Vector2(horizontal, vertical);

        // 정규화 (대각선 이동 시 속도 보정)
        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        if (_player != null)
        {
            _player.OnStickChanged(inputDir);
        }
    }


    //메소드 오버로딩(Method Overloading)
    //메소드의 매개변수 리스트(시그니처)가 다를 경우, 이름이 같아도 다른 함수로 취급합니다.
    private void HandleTouchInput(Vector3 mousePosition)
    {
        if (_buttonPressed)
        {
            //입력받은 좌표 계산
            Vector3 diff = mousePosition - _startPos;

            //Vector3.Distance(a,b) : 두 점 간의 거리 계산 (정교한 값 계산)
            //sqrMagnitude : 거리의 제곱에 루트한 값 (단순 계산) : Vector 간의 거리 비교
            if(diff.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diff.Normalize(); //방향 벡터 거리 1로 설정

                //방향 콘트롤러 움직이기
                _touchPad.position = _startPos + diff * _dragRadius;
            }
            else
            {
                //입력 지점과 기준 좌표가 최대치보다 크지 않다면 현재 입력 좌표에 방향키 이동
                _touchPad.position = mousePosition;
            }

            //방향키와 기준점의 차이를 계산
            Vector3 distance = _touchPad.position - _startPos;
            //방향만 따로 계산
            Vector2 normalDiff = new Vector3(distance.x / _dragRadius, distance.y / _dragRadius, distance.z / _dragRadius);

            if(_player != null)
            {
                _player.OnStickChanged(normalDiff);
            }
        }
        else if(_buttonPressed == false)
        {
            _touchPad.position = _touchPad.parent.position;
            _player.OnStickChanged(new Vector2(0.0f, 0.0f));
        }
    }

    private void HandleTouchInput()
    {
        Debug.Log("모바일 빌드");
    }
}
