using UnityEngine;
//싱글톤 패턴(Singleton Pattern)
//최초 한번 객체를 '생성'하고, 그 후에는 생성하지 않고 그 객체를
//사용하도록 프로그램 전체에서 돌려쓰기 위한 유일한 객체를 만들어주는 것

//[장점]
//불필요하게 같은 객체를 여러번 만들 필요가 없어짐.(메모리 낭비가 덜한 편)

//[문제점]
//1. 독립적인 테스트를 진행하는데에는 부적합합니다.
//2. 싱글톤의 인스턴스가 변경될 경우, 해당 인스턴스를 참조하는 모든 클래스에 대한
//   수정이 진행될 수 있음.
//3. 상속이 좀 어려움.

//결론 : 싱글톤으로 써먹어도 될법한 값에 사용하자.
//     ex) 유니티의 매니저 코드

public class GameManager : MonoBehaviour
{
    //프로퍼티(Property)로 만들어보는 싱글톤
    
    //1. private static 클래스명 instance;
    private static GameManager instance = null;

    //2. 프로퍼티로 접근
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    //3. 사용할 필드 / 메소드 만들어주기
    public int value = 1;

    //시작 전에 설정
    private void Awake()
    {
        //null에 대한 체크를 진행해서 null이면 등록, 아니면 파괴

        //이미 존재하고 있는 경우
        if(Instance != null)
        {
            Destroy(gameObject); //두 개 이상일 경우 삭제
            return;
        }
        //인스턴스에 자신을 초기화
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

public class Test
{
    void Use()
    {
        //매니저의 기능을 가져와서 사용이 가능합니다.
        GameManager.Instance.value++;
    }
}

public class  BasicSingleton
{
    private static BasicSingleton instance;

    private BasicSingleton()
    {
        //new에 의한 생성이 되지 않게 접근 제한
    }

    //전역 공개 메소드를 통해 싱글톤 생성 및 return
    public static BasicSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new BasicSingleton();
        }
        return instance;
    }
}


