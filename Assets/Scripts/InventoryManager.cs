using System;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryStatus { None, TwoHanding };
public class InventoryManager : MonoBehaviour
{
	private InventoryItem _activeInventoryItem; // guaranteed to have an _activeInventoryItem; if this is ever not true we messed up
	[SerializeField] private List<InventoryItem> _inventoryItems;
	
	private void Awake() {
		_activeInventoryItem = _inventoryItems[0];
	}
	private void Update()
	{
		
	}

	public void PickUp(GameObject obj) {
		Item newItem = obj.GetComponent<Item>();
		InventoryItem newInvItem = obj.GetComponent<InventoryItem>();

		if(newItem.Data.IsInventoryItem) {
			if(_activeInventoryItem.AddInventoryItem(newItem))
			{
				newItem.PickUp();
				_inventoryItems.Add(newInvItem);
				return;
			} else {
				foreach(InventoryItem invItem in _inventoryItems) {
					if(invItem != _activeInventoryItem && invItem.AddInventoryItem(newItem)) {
						newItem.PickUp();
						_inventoryItems.Add(newInvItem);
						return;
					}
				}
			}
		} else {
			if(_activeInventoryItem.AddItems(newItem.Data)) {
				newItem.PickUp();
				return;
			} else {
				foreach(InventoryItem invItem in _inventoryItems) {
					if(invItem != _activeInventoryItem && invItem.AddItems(newItem.Data)) {
						newItem.PickUp();
						return;
					}
				}
			}
		}
	}

	// check all inventory items for item
	public bool Has(ItemData item, int quantity) {
		int total = 0;

		foreach(InventoryItem inventoryItem in _inventoryItems) {
			total += inventoryItem.Quantity(item);
			if(total >= quantity) { return true; }
		}

		return false;
	}

	public bool ActiveHas(ItemData item, int quantity = 1) {
		return _activeInventoryItem.Quantity(item) >= quantity;
	}

	public int Quantity(ItemData item) {
		int total = 0;

		foreach(InventoryItem inventoryItem in _inventoryItems) {
			total += inventoryItem.Quantity(item);
		}

		return total;
	}

	public int ActiveQuantity(ItemData item)
	{
		return _activeInventoryItem.Quantity(item);
	}

	// remove a number of items (sourced from all inventory items)
	// private bool Remove(ItemData item, int quantity) {
	// 	if(Has(item, quantity)) {
	// 		int remaining = quantity;
	// 		foreach(InventoryItem inventoryItem in _inventoryItems) {
	// 			if(remaining == 0) { break; }
	// 			else {
	// 				int amtRemoved = inventoryItem.Quantity(item);
	// 				inventoryItem.RemoveItem(item, amtRemoved);
	// 				remaining -= amtRemoved;
	// 			}
	// 		}
	// 		return true;
	// 	} else {
	// 		return false;
	// 	}
	// }

	// private bool RemoveActive(ItemData item, int quantity) {
	// 	return _activeInventoryItem.RemoveItem(item, quantity);
	// }

	// only drop from active inventory item; drops first item in inventoryitem. if no items in inventoryitem and inventoryitem can be dropped, drop inventoryitem. if inventoryitem cannot be dropped, this does nothing.
	// public void Drop() {
	// 	foreach(KeyValuePair<ItemData, ValueTuple<int, List<Item>>> entry in _activeInventoryItem._inventory)
	// 	{
			
	// 	}
	// }

	public string Print() {
		string s = "INVENTORY MANAGER:\n";
		s += $"active inventory item: {_activeInventoryItem.name}\n";
		s += $"all inventory:\n";
		foreach(InventoryItem item in _inventoryItems) {
			s += item.Print($"{item.gameObject.name} ({item._invData.Inventory - item._roomRemaining}/{item._invData.Inventory}): ", $",\n");
		}

		return s;
	}
}