using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public BattleSystem battleSystem; // 배틀 시스템 참조
    public static Typing instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsTyping())
            {
                SkipTyping(); // 타이핑 중이면 스킵 요청
            }
        }
    }

    //GPT꺼 따라쓰기(일단)

    [Header("Settings")]
    public float typingSpeed = 0.1f; // 초당 타이핑 속도

    [Header("Events")]
    public UnityEvent onTypingComplete;
    public UnityEvent ontTypingTurn1;
    public UnityEvent onTypingWin;

    [Header("Audio")]
    public AudioSource typingSound; // 타이핑 소리
    public AudioClip typingClip; // 타이핑 소리 클립

    private Coroutine typingCoroutine;
    private string currentMessage;
    private bool isTyping = false;
    private bool skipRequested = false;

    public void StartTyping(string message, Text targetText)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        currentMessage = message;
        typingCoroutine = StartCoroutine(TypeText(currentMessage, targetText));
    }

    private IEnumerator TypeText(string message, Text targetText)
    {
        isTyping = true;
        skipRequested = false;
        targetText.text = ""; // 타이핑 시작 전에 텍스트 초기화
        for (int i = 0; i < message.Length; i++)
        {
            if (skipRequested)
            {
                targetText.text = message; // 전체 메시지 표시
                break;
            }
            targetText.text += message[i]; // 한 글자씩 추가
            if (typingSound != null && typingClip != null)
            {
                typingSound.PlayOneShot(typingClip); // 타이핑 소리 재생
            }
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도 대기
        }
        isTyping = false;
        StartCoroutine(InputWait()); // 입력 대기 시작
    }

    private IEnumerator InputWait()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (battleSystem.state == BattleTurn.Start)
                {
                    onTypingComplete.Invoke(); // 타이핑 완료 이벤트 호출
                    break;

                }
                else if (battleSystem.state == BattleTurn.Turn1)
                {
                    ontTypingTurn1.Invoke(); // 턴1 타이핑 완료 이벤트 호출
                    break;
                }
                else if (battleSystem.state == BattleTurn.Win)
                {
                    onTypingWin.Invoke();
                    break;
                }
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            skipRequested = true; // 스킵 요청
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
