using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance; //�̱��� �ν��Ͻ�

    public AudioClip bgmClip; //��� ���� Ŭ��
    private AudioSource audioSource; //����� �ҽ� ������Ʈ


    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //�� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); //�̹� �ν��Ͻ��� �����ϸ� ���� ������Ʈ ����
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip; //��� ���� Ŭ�� ����
        audioSource.loop = true; //�ݺ� ��� ����
        audioSource.playOnAwake = true; //Awake �� �ڵ� ��� ����
        audioSource.volume = 0.5f; //���� ���� (0.0f ~ 1.0f ����)
        audioSource.Play(); //��� ���� ��� ����
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
