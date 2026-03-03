using UnityEngine;

public enum ItemTag { Small, Medium, Large, TwoHanded, Dangerous, TestTag };

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string Description;
    public ItemTag[] Tags;
    public float Weight;
    public bool IsInventoryItem;
    public bool CanDrop;

}
