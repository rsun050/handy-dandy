using UnityEngine;

public enum ItemTag { Small, Large, TwoHanded, Dangerous, TestTag };

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string Description;
    public ItemTag[] Tags;
    public float Weight;

}
