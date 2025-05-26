using UnityEngine;

namespace Assets.Scripts.Item2
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
    }

}