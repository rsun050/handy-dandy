using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Item {
    [SerializeField] private InventoryItemData _inventoryData;
    private Dictionary<ItemData, int> _inventory;
    private float _inventoryTaken;

    void Awake() {
        _inventory = new Dictionary<ItemData, int>();
    }

    public bool HasRoom(ItemData item, int quantity = 1) {
        return (item.Weight * quantity) <= (_inventoryData.Inventory - _inventoryTaken);
    }

    public bool HasItem(ItemData item, int quantity = 1) {
        if(!_inventory.ContainsKey(item)) { return false; }
        return _inventory[item] >= quantity;
    }

    public int Quantity(ItemData item)
    {
        if(!_inventory.ContainsKey(item)) { return 0; }
        else
        {
            return _inventory[item];
        }
    }

    public bool AddItem(ItemData item, int quantity = 1) {
        if(quantity <= 0) { Debug.Log($"AddItem: failure; should add at least 1 {item.ItemName}"); return false; }
        if(HasRoom(item, quantity)) {
            if(!_inventory.ContainsKey(item)) {
                _inventory.Add(item, quantity);
            } else {
                _inventory[item] += quantity;
            }

            _inventoryTaken += item.Weight * quantity;
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveItem(ItemData item, int quantity = 1) {
        if(HasItem(item, quantity)) {
            if(_inventory[item] > quantity) {
                _inventory[item] -= quantity;                
            } else {
                _inventory.Remove(item);
            }
            
            _inventoryTaken = Math.Max(0, _inventoryTaken - item.Weight * quantity);            
            return true;
        } else {
            Debug.Log($"RemoveItem: silent failure; inventory does not have {quantity} {item.ItemName} to remove");
            return false;
        }
    }

    public void Print(string prefix = "", string suffix = "")
    {
        string s = "{ ";
        foreach(KeyValuePair<ItemData, int> entry in _inventory)
        {
            s += entry.Key.ItemName + " : " + entry.Value.ToString() + ", ";
        }

        s = s.Substring(0, s.Length - 2) + " }";

        Debug.Log(prefix + s + suffix);
    }
}
