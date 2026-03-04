using System;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryStatus { None, TwoHanding };
public class InventoryManager : MonoBehaviour
{
	private InventoryItem _activeInventoryItem; // guaranteed to have an _activeInventoryItem; if this is ever not true we messed up
	[SerializeField] private List<InventoryItem> _inventoryItems; // first two should always be Hand (L) and Hand (R)
	
	public event Action<int> SwitchActiveE;

	private void Awake() {
		_activeInventoryItem = _inventoryItems[0];
	}

	private void Start()
	{
	}
	private void Update()
	{
		
	}

	public void SwitchActive() {
		int newActive = (_activeInventoryItem == _inventoryItems[0]) ? 1 : 0;
		_activeInventoryItem = _inventoryItems[newActive];
		SwitchActiveE?.Invoke(newActive);
	}

	public void PickUp(GameObject obj) {
		Item newItem = obj.GetComponent<Item>();
		InventoryItem newInvItem = obj.GetComponent<InventoryItem>();

		if(newItem.Data.IsInventoryItem) {
			if(_activeInventoryItem.AddInventoryItem(newItem)) { // try to insert in active inventory item
				newItem.PickUp();
				_inventoryItems.Add(newInvItem);
				return;
			} else { // try all others
				foreach(InventoryItem invItem in _inventoryItems) {
					if(invItem != _activeInventoryItem && invItem.AddInventoryItem(newItem)) {
						newItem.PickUp();
						_inventoryItems.Add(newInvItem);
						return;
					}
				}
			}
		} else { // is not inventory item
			if(_activeInventoryItem.AddItem(newItem)) { // try to insert in active inventory item
				newItem.PickUp();
				return;
			} else { // try all others
				foreach(InventoryItem invItem in _inventoryItems) {
					if(invItem != _activeInventoryItem && invItem.AddItem(newItem)) {
						newItem.PickUp();
						return;
					}
				}
			}
		}
	}

	public void Drop() { // drop most recently pickedup item from activeobj 
		_activeInventoryItem.RemoveMostRecent();
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

	public int Quantity(ItemData itemData) {
		int total = 0;

		foreach(InventoryItem inventoryItem in _inventoryItems) {
			total += inventoryItem.Quantity(itemData);
		}

		return total;
	}

	public int ActiveQuantity(ItemData item)
	{
		return _activeInventoryItem.Quantity(item);
	}

	// remove a number of items (sourced from all inventory items, focuses on active inventory item first)
	private bool Remove(ItemData itemData, int quantity) {
		if(Has(itemData, quantity)) {
			int remainingToRemove = quantity;

			int numRemoved = _activeInventoryItem.Remove(itemData, remainingToRemove);
			remainingToRemove -= numRemoved;
			while(remainingToRemove > 0) {
				foreach(InventoryItem invItem in _inventoryItems) {
					if(invItem == _activeInventoryItem) continue;

					numRemoved = invItem.Remove(itemData, remainingToRemove);
					remainingToRemove -= numRemoved;
				}
			}

			return true;
		}
		return false;
	}

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