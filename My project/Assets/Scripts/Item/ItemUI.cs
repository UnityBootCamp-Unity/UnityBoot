using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Sprite iconImage;
    public Text nameText;
    public Text descriptionText;

    public static ItemUI Instance = null;
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
    public void Show(Item item)
    {
        iconImage = item.GetImage();
        nameText.text = item.name;
        descriptionText.text = item.description;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}