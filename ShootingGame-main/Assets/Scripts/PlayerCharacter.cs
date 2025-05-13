using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{
    public int current_character;

    public int Character
    {
        get
        {
            return current_character;
        }
        set
        {
            current_character = value;
        }
    }

    public static PlayerCharacter Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

}



