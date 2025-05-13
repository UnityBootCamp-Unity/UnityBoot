using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CharacterList
{
    Plan1, Plan2, Plan3
}

public class CharacterManager : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;

    public GameObject[] character_list; // ������ ���
    private GameObject current_character; // ���� ���� �����ϴ� ĳ����
    public Transform spawnPoint; // ���� ��ġ ����

    private int ChoiceCharacter;
    private int last_character = System.Enum.GetValues(typeof(CharacterList)).Length - 1; //ĳ���� ī��Ʈ ��

    public void Start()
    {
        ChoiceCharacter = PlayerCharacter.Instance.Character;

        ShowCharacter(ChoiceCharacter);
    }

    public void Update()
    {

        if (ChoiceCharacter == 0)
        {
            leftButton.interactable = false;
        }
        else if (ChoiceCharacter == last_character)
        {
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    public void OnLeftButton()
    {
        ChoiceCharacter--;
        ShowCharacter(ChoiceCharacter);
        //������Ʈ�� �ƴ� ���⼭ ��Ȱ��ȭ�� Ȱ��ȭ ó���� ������ ���� ��
    }

    public void OnRightButton()
    {
        ChoiceCharacter++;
        ShowCharacter(ChoiceCharacter);
    }

    public void OnChoiceButton()
    {
        PlayerCharacter.Instance.Character = ChoiceCharacter;
        SceneManager.LoadScene("GameScene");
    }

    void ShowCharacter(int ChoiceCharacter)
    {
        if (current_character != null)
            Destroy(current_character);

        current_character = Instantiate(character_list[ChoiceCharacter], spawnPoint.position, spawnPoint.rotation);
    }
    
}
