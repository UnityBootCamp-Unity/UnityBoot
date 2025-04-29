using UnityEngine;
using UnityEngine.InputSystem;

public class PlayManager : Singleton<PlayManager>
{
    public int hp = 50;
    
    public override void Awake()
    {
        base.Awake();
    }
    public void hpPlus(int hp)
    {
        this.hp += hp;
    }
}

public class GoldenApple : MonoBehaviour
{
    public int value;
    public void Use()
    {
        PlayManager.Instance.hpPlus(value);
    }
}
