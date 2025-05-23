using UnityEngine;

public enum Name
{
	Peach,
}
public class Item
{
    public string name;
    public string description;
    public int price;

    private Sprite uiImage;

    public Item(string name, Sprite uiImage)
    {
        this.name = name;
        this.uiImage = uiImage;
    }

    public void SetItemDescription(string description)
    {
        this.description = description;
    }

    public void SetItemPrice(int price)
    {
        this.price = price;
    }

    public Sprite GetImage()
    {
        return uiImage;
    }
}