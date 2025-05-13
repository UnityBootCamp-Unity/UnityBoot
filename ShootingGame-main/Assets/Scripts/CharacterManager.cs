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

    public GameObject[] character_list; // 프리팹 목록
    private GameObject current_character; // 현재 씬에 존재하는 캐릭터
    public Transform spawnPoint; // 생성 위치 지정

    private int ChoiceCharacter;
    private int last_character = System.Enum.GetValues(typeof(CharacterList)).Length - 1; //캐릭터 카운트 수

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
        //업데이트가 아닌 여기서 비활성화랑 활성화 처리도 나쁘지 않을 듯
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
