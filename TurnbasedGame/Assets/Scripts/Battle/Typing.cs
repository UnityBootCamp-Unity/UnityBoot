using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    public BattleSystem battleSystem; // ��Ʋ �ý��� ����
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
                SkipTyping(); // Ÿ���� ���̸� ��ŵ ��û
            }
        }
    }

    //GPT�� ���󾲱�(�ϴ�)

    [Header("Settings")]
    public float typingSpeed = 0.1f; // �ʴ� Ÿ���� �ӵ�

    [Header("Events")]
    public UnityEvent onTypingComplete;
    public UnityEvent ontTypingTurn1;
    public UnityEvent onTypingWin;

    [Header("Audio")]
    public AudioSource typingSound; // Ÿ���� �Ҹ�
    public AudioClip typingClip; // Ÿ���� �Ҹ� Ŭ��

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
        targetText.text = ""; // Ÿ���� ���� ���� �ؽ�Ʈ �ʱ�ȭ
        for (int i = 0; i < message.Length; i++)
        {
            if (skipRequested)
            {
                targetText.text = message; // ��ü �޽��� ǥ��
                break;
            }
            targetText.text += message[i]; // �� ���ھ� �߰�
            if (typingSound != null && typingClip != null)
            {
                typingSound.PlayOneShot(typingClip); // Ÿ���� �Ҹ� ���
            }
            yield return new WaitForSeconds(typingSpeed); // Ÿ���� �ӵ� ���
        }
        isTyping = false;
        StartCoroutine(InputWait()); // �Է� ��� ����
    }

    private IEnumerator InputWait()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (battleSystem.state == BattleTurn.Start)
                {
                    onTypingComplete.Invoke(); // Ÿ���� �Ϸ� �̺�Ʈ ȣ��
                    break;

                }
                else if (battleSystem.state == BattleTurn.Turn1)
                {
                    ontTypingTurn1.Invoke(); // ��1 Ÿ���� �Ϸ� �̺�Ʈ ȣ��
                    break;
                }
                else if (battleSystem.state == BattleTurn.Win)
                {
                    onTypingWin.Invoke();
                    break;
                }
            }
            yield return null; // ���� �����ӱ��� ���
        }
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            skipRequested = true; // ��ŵ ��û
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
