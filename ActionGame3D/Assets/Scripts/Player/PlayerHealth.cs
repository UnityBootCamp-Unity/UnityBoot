using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 100;       //�÷��̾��� ���� ü��
    public int currentHealth = 100;     //�÷��̾��� ���� ü��
    public Slider healthSlider;         //ü�� UI�� ����
    public Image damageImage;           //������ ���� ��쿡 ���� �̹���
    public AudioClip deathClip;         //�÷��̾� ������ ���� �� �� �����

    public float flashSpeed = 5.0f;     //ȭ�� ���� ���ϰ� �ٽ� ���ư��� �ӵ�
    public Color flashColor = new Color(1f, 0, 0, 0.1f); //����� ��

    Animator anim;                      //�ִϸ�����
    AudioSource playerAudio;            //����� �ҽ�
    PlayerMovement playerMovement;      //�÷��̾� ������
    bool isDead;                        //���� Ȯ�ο� ����
    bool damaged;                       //������ Ȯ�ο� ����

    private void Awake()
    {
        //������Ʈ����
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        //ü�� ����
        currentHealth = startHealth;
    }

    //�÷��̾��� ������ ���� ������ �� ��ȯ
    private void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    //�÷��̾ �������� �޾��� �� ȣ���� �Լ�
    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerMovement.Stop();

        if(currentHealth <=0 && !isDead)
        {
            Death();
        }
        else
        {
            anim.SetTrigger("Damage");
        }
    }

    void Death()
    {
        StageController.Instance.FinishGame();
        isDead = true;
        anim.SetTrigger("Damage");
        anim.SetTrigger("Die");
        playerMovement.enabled = false; //PlayerMovement�� ���� ��Ȱ��ȭ
    }
}
