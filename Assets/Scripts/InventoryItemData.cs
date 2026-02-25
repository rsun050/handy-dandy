using UnityEngine;
public enum InventoryItemTag { Hand };

[CreateAssetMenu(fileName = "Inventory Item", menuName = "ScriptableObjects/Inventory Item", order = 1)]
public class InventoryItemData : ScriptableObject
{
	public float Inventory;
}