using UnityEngine;

//T Singleton, MonoSingleton
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = (T)FindAnyObjectByType<T>();
                //씬에서 타입에 맞는 오브젝트를 찾아서 그 값을 return

                //찾은 결과가 null일 경우
                if(_instance == null)
                {
                    GameObject go = new GameObject(); //빈 오브젝트 생성
                    go.name = typeof(T).Name; //넣으려는 형태로 이름 설정
                    go.AddComponent<T>(); //게임 오브젝트에 컴포넌트를 추가하는 스크립트
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    //유니티에서 Awake 할 경우 추가
    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
