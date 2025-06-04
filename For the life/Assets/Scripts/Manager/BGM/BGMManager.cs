using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance; //싱글톤 인스턴스

    public AudioClip bgmClip; //배경 음악 클립
    private AudioSource audioSource; //오디오 소스 컴포넌트


    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); //이미 인스턴스가 존재하면 현재 오브젝트 삭제
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip; //배경 음악 클립 설정
        audioSource.loop = true; //반복 재생 설정
        audioSource.playOnAwake = true; //Awake 시 자동 재생 설정
        audioSource.volume = 0.5f; //볼륨 설정 (0.0f ~ 1.0f 범위)
        audioSource.Play(); //배경 음악 재생 시작
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
