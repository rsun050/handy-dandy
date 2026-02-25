using System.Runtime.InteropServices;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	private InventoryItem _activeInventoryItem;
	[SerializeField] private InventoryItem[] _inventoryItems;
	
	public bool Add(ItemData item, int quantity)
	{
		return _activeInventoryItem.AddItem(item, quantity);
	}

	// check all inventory items for item
	public bool Has(ItemData item, int quantity)
	{
		int total = 0;

		foreach(InventoryItem inventoryItem in _inventoryItems)
		{
			total += inventoryItem.Quantity(item);
			if(total >= quantity)
			{
				return true;
			}
		}

		return false;
	}

	public int Quantity(ItemData item)
	{
		int total = 0;

		foreach(InventoryItem inventoryItem in _inventoryItems)
		{
			total += inventoryItem.Quantity(item);
		}

		return total;
	}

	public int ActiveQuantity(ItemData item)
	{
		return _activeInventoryItem.Quantity(item);
	}

	public bool ActiveHas(ItemData item, int quantity)
	{
		return _activeInventoryItem.HasItem(item, quantity);
	}

	// remove a number of items (sourced from all inventory items)
	public bool Remove(ItemData item, int quantity)
	{
		if(Has(item, quantity))
		{
			int remaining = quantity;
			foreach(InventoryItem inventoryItem in _inventoryItems)
			{
				if(remaining == 0) { break; }
				else
				{
					int amtRemoved = inventoryItem.Quantity(item);
					inventoryItem.RemoveItem(item, amtRemoved);
					remaining -= amtRemoved;
				}
			}
			return true;
		} else
		{
			return false;
		}
	}

	public bool RemoveActive(ItemData item, int quantity)
	{
		return _activeInventoryItem.RemoveItem(item, quantity);
	}


}