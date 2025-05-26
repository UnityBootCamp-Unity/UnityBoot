/*using UnityEngine;
using UnityEngine.UI;

public class Peach : MonoBehaviour
{
	public Item item;
    public Sprite image;

    private void Start()
    {
        string name = Name.Peach.ToString();
        item = new Item(name, image);
        item.SetItemDescription("복숭아");
        item.SetItemPrice(100);
    }


    #region 싱글톤
    public static Peach Instance = null;

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
    #endregion

}
*/