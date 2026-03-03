using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItem : Item {
    [field: SerializeField] public InventoryItemData _invData { get; private set; }
    public Dictionary<ItemData, int> _inv { get; private set; }
    public List<InventoryItem> _invItemsHeld { get; private set; }
    [field: SerializeField] public float _roomRemaining { get; private set; }
    public InventoryItem parentInvItem { get; private set; }

    void Awake() {
        _inv = new Dictionary<ItemData, int>();
        _invItemsHeld = new List<InventoryItem>();
        _roomRemaining = _invData.Inventory;
    }

    private bool CanHold(ItemData itemData) {
        foreach(ItemTag holdableTag in _invData.FitsItemsOfType) {
            foreach(ItemTag itemTag in itemData.Tags) {
                if(holdableTag == itemTag) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool SelfHasRoom(ItemData itemData, int quantity = 1)
    {
        if(this.CanHold(itemData))
        {
            return _roomRemaining >= (itemData.Weight * quantity);
        }
        return false;
    }
    private bool HasRoom(ItemData itemData, int quantity = 1)
    {
        return HasRoomHelper(itemData, itemData.Weight * quantity).Item1;
    }

    private Tuple<bool, float> HasRoomHelper(ItemData itemData, float remainingSpaceNeeded) {
        if(remainingSpaceNeeded <= 0) return new Tuple<bool, float>(true, 0);
        if(this.CanHold(itemData) && this._roomRemaining >= itemData.Weight)
        {
            int holding = (int)_roomRemaining / (int)itemData.Weight;
            remainingSpaceNeeded -= holding * itemData.Weight;
        }

        foreach(InventoryItem invItem in _invItemsHeld)
        {
            Tuple<bool, float> ret = invItem.HasRoomHelper(itemData, remainingSpaceNeeded);
            remainingSpaceNeeded = ret.Item2;
        }

        return new Tuple<bool, float>(remainingSpaceNeeded <= 0, remainingSpaceNeeded);
    }

    private Tuple<InventoryItem, int> DeepestInventoryItemThatCanHold(ItemData itemData, int depth = 0)
    {
        InventoryItem deepestInvItem = null;
        int deepestDepth = -1;

        if(this.SelfHasRoom(itemData)) {
            deepestInvItem = this;
            deepestDepth = depth;
        }

        foreach(Item item in _invItemsHeld) {
            Tuple<InventoryItem, int> ret = item.GetComponent<InventoryItem>().DeepestInventoryItemThatCanHold(itemData, depth+1);
            if(ret.Item2 > deepestDepth)
            {
                deepestInvItem = ret.Item1;
                deepestDepth = ret.Item2;
            }
        }

        return new Tuple<InventoryItem, int>(deepestInvItem, deepestDepth);
    }

    // WARNING: does not tag check, assumes item is not an inventory item
    private int _AddItems(ItemData itemData, int quantity) {
        int fits = Math.Min((int)_roomRemaining / (int)itemData.Weight, quantity);
        _roomRemaining -= fits * itemData.Weight;

        if(_inv.ContainsKey(itemData)) {
            _inv[itemData] += fits;
        } else {
            _inv.Add(itemData, fits);
        }

        return fits;
    }

    public bool AddItems(ItemData itemData, int quantity = 1)
    {
        Debug.Assert(!itemData.IsInventoryItem, "InventoryItem.AddItems is for noninventory items only");
        string s = $"{this.gameObject.name} trying to pick up item {itemData.ItemName}";

        if(this.HasRoom(itemData, quantity))
        {
            while(quantity > 0)
            {
                InventoryItem invItem = this.DeepestInventoryItemThatCanHold(itemData).Item1;
                quantity -= invItem._AddItems(itemData, quantity);
            }
            Debug.Log(s + "\nSuccess");
            return true;
        }
        Debug.Log(s + "\nDidn't have room");
        return false;
    }

    public bool AddInventoryItem(Item item)
    {
        Debug.Assert(item.Data.IsInventoryItem);
        string s = $"{this.gameObject.name} trying to pick up invItem {item.gameObject.name}";

        if(this.HasRoom(item.Data))
        {
            if(_inv.ContainsKey(item.Data))
            {
                _inv[item.Data] += 1;
            } else
            {
                _inv.Add(item.Data, 1);
            }

            this._roomRemaining -= item.Data.Weight;

            InventoryItem invItem = item.gameObject.GetComponent<InventoryItem>();
            _invItemsHeld.Add(invItem);
            invItem.parentInvItem = this;

            Debug.Log(s);
            return true;
        }
        Debug.Log(s += $"\ndidn't have room");
        return false;
    }

    public int Quantity(ItemData itemData, bool self = false)
    {
        if(self) {
            return (_inv.ContainsKey(itemData)) ? _inv[itemData] : 0;
        } else {
            return this._QuantityHelper(itemData);
        }
    }

    private int _QuantityHelper(ItemData itemData, int qty = 0) {
        qty = (_inv.ContainsKey(itemData)) ? _inv[itemData] + qty : qty;

        foreach(InventoryItem invItem in _invItemsHeld)
        {
            qty += invItem._QuantityHelper(itemData, qty);
        }

        return qty;
    }
    public string Print(string prefix = "", string suffix = "", bool print = false)
    {
        string s = "{ ";
        foreach(KeyValuePair<ItemData, int> entry in _inv)
        {
            s += entry.Key.ItemName + " : " + entry.Value.ToString() + ", ";
        }

        if(s.Length > 2) { s = s.Substring(0, s.Length - 2) + " }"; }
        else { s += "}"; }

        if(print) { Debug.Log(prefix + s + suffix); }
        return(prefix + s + suffix);
    }
}
