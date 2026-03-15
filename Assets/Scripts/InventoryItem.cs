using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Item {
    [field: SerializeField] public InventoryItemData _invData { get; private set; }
    public Dictionary<ItemData, int> _quantities { get; private set; }
    public Dictionary<ItemData, List<GameObject>> _items { get; private set; } // GameObjects should ONLY be Items of type ItemData
    public List<GameObject> _itemInsertedOrder { get; private set; }
    [field: SerializeField] public float _roomRemaining { get; private set; }
    public InventoryItem parentInvItem { get; private set; }

    void Awake() {
        _quantities = new Dictionary<ItemData, int>();
        _items = new Dictionary<ItemData, List<GameObject>>(); // TODO : update 'add' funcs
        _itemInsertedOrder = new List<GameObject>();
    }

    void Start() {
        _roomRemaining = _invData.Inventory;    
    }

    // is this container able to hold an item based on its tags?
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

    // can this container (alone; disregarding any inventory items it might contain) have room to hold this many of this item? 
    private bool SelfHasRoom(ItemData itemData, int quantity = 1)
    {
        if(this.CanHold(itemData))
        {
            return _roomRemaining >= (itemData.Weight * quantity);
        }
        return false;
    }
    
    // does this container (including any inventory items it might contain) have room to hold this many of this item?
    private bool HasRoom(ItemData itemData, int quantity = 1)
    {
        return HasRoomHelper(itemData, itemData.Weight * quantity).Item1;
    }

    // recursive helper function
    private Tuple<bool, float> HasRoomHelper(ItemData itemData, float remainingSpaceNeeded) {
        if(remainingSpaceNeeded <= 0) return new Tuple<bool, float>(true, 0);
        if(this.CanHold(itemData) && this._roomRemaining >= itemData.Weight)
        {
            int holding = (int)_roomRemaining / (int)itemData.Weight;
            remainingSpaceNeeded -= holding * itemData.Weight;
        }

        foreach(KeyValuePair<ItemData, List<GameObject>> pair in _items) {
            if(pair.Key.IsInventoryItem) {
                foreach(GameObject item in pair.Value) {
                    Tuple<bool, float> ret = item.GetComponent<InventoryItem>().HasRoomHelper(itemData, remainingSpaceNeeded);
                    remainingSpaceNeeded = ret.Item2;                        
                }
            } 
        }

        return new Tuple<bool, float>(remainingSpaceNeeded <= 0, remainingSpaceNeeded);
    }

    // return the deepest container (ie: bag in a bag in a bag...) that can hold at least 1 of this item
    private Tuple<InventoryItem, int> DeepestInventoryItemThatCanHold(ItemData itemData, int depth = 0)
    {
        InventoryItem deepestInvItem = null;
        int deepestDepth = -1;

        if(this.SelfHasRoom(itemData)) {
            deepestInvItem = this;
            deepestDepth = depth;
        }

        foreach(KeyValuePair<ItemData, List<GameObject>> pair in _items) {
            if(pair.Key.IsInventoryItem) {
                foreach(GameObject item in pair.Value) {
                    Tuple<InventoryItem, int> ret = item.GetComponent<InventoryItem>().DeepestInventoryItemThatCanHold(itemData, depth+1);
                    if(ret.Item2 > deepestDepth) {
                        deepestInvItem = ret.Item1;
                        deepestDepth = ret.Item2;
                    }  
                }                  
            }            
        }

        return new Tuple<InventoryItem, int>(deepestInvItem, deepestDepth);
    }

    // return the deepest container that has at least 1 of this item
    private Tuple<InventoryItem, int> DeepestInventoryItemThatHas(ItemData itemData, int depth = 0) {
        InventoryItem deepestInvItem = null;
        int deepestDepth = -1;

        if(this.Quantity(itemData, true) > 0) {
            deepestInvItem = this;
            deepestDepth = depth;
        }

        foreach(KeyValuePair<ItemData, List<GameObject>> pair in _items) {
            if(pair.Key.IsInventoryItem) {
                foreach(GameObject item in pair.Value) {
                    Tuple<InventoryItem, int> ret = item.GetComponent<InventoryItem>().DeepestInventoryItemThatHas(itemData, depth+1);
                    if(ret.Item2 > deepestDepth) {
                        deepestInvItem = ret.Item1;
                        deepestDepth = ret.Item2;
                    }
                }
            }
        }

        return new Tuple<InventoryItem, int>(deepestInvItem, deepestDepth);
    }

    // helper function
    // assumes item is NOT an inventory item
    // inserts into self only
    // WARNING: does not tag check, assumes item is not an inventory item
    private int _AddItemToSelf(Item item) {
        Debug.Assert(!item.Data.IsInventoryItem);
        Debug.Log($"{this.Data.ItemName} picked up {item.Data.ItemName}");

        ItemData itemData = item.Data;
        _roomRemaining -= itemData.Weight;

        if(_quantities.ContainsKey(itemData)) {
            _quantities[itemData] += 1;
        } else {
            _quantities.Add(itemData, 1);
        }

        if(_items.ContainsKey(itemData)) {
            _items[itemData].Add(item.gameObject);
        } else {
            List<GameObject> list = new List<GameObject>();
            list.Add(item.gameObject);
            _items.Add(itemData, list);
        }

        _itemInsertedOrder.Add(item.gameObject);

        return 1;
    }

    // assumes item is NOT an inventory item, inserts into deepest inventory item possible
    public bool AddItem(Item item) {
        ItemData itemData = item.Data;
        Debug.Assert(!itemData.IsInventoryItem, "InventoryItem.AddItems is for noninventory items only");
        string s = $"{this.gameObject.name} trying to pick up item {itemData.ItemName}";

        if(this.HasRoom(itemData, 1)) {
            InventoryItem invItem = this.DeepestInventoryItemThatCanHold(itemData).Item1;
            invItem._AddItemToSelf(item);
            Debug.Log(s + "\nSuccess");
            return true;
        }
        Debug.Log(s + "\nDidn't have room");
        return false;
    }

    // add an inventory item (to self)
    public bool AddInventoryItem(Item item) {
        Debug.Assert(item.Data.IsInventoryItem);
        string s = $"{this.gameObject.name} trying to pick up invItem {item.gameObject.name}";
        ItemData itemData = item.Data;

        if(this.SelfHasRoom(item.Data)) {
            if(_quantities.ContainsKey(item.Data)) {
                _quantities[item.Data] += 1;
            } else {
                _quantities.Add(item.Data, 1);
            }

            this._roomRemaining -= item.Data.Weight;

            if(_items.ContainsKey(itemData)) {
                _items[itemData].Add(item.gameObject);
            } else {
                List<GameObject> list = new List<GameObject>();
                list.Add(item.gameObject);
                _items.Add(itemData, list);
            }

            _itemInsertedOrder.Add(item.gameObject);

            InventoryItem invItem = item.gameObject.GetComponent<InventoryItem>();
            invItem.parentInvItem = this;

            Debug.Log(s);
            return true;
        }
        Debug.Log(s += $"\ndidn't have room");
        return false;
    }

    // get quantity of an item held: can specify if self or recursive search
    public int Quantity(ItemData itemData, bool self = false)
    {
        if(self) {
            return (_quantities.ContainsKey(itemData)) ? _quantities[itemData] : 0;
        } else {
            return this._QuantityHelper(itemData);
        }
    }

    // helper function
    private int _QuantityHelper(ItemData itemData, int qty = 0) {
        qty = (_quantities.ContainsKey(itemData)) ? _quantities[itemData] + qty : qty;

        foreach(KeyValuePair<ItemData, List<GameObject>> pair in _items) {
            if(pair.Key.IsInventoryItem) {
                foreach(GameObject obj in pair.Value) {
                    qty += obj.GetComponent<InventoryItem>()._QuantityHelper(itemData, qty);
                }
            } else { break; }
        }

        return qty;
    }

    private Vector3 playerpos() {
        return GameController.PlayerCam.transform.position + 
            GameController.PlayerCam.transform.GetChild(0).transform.up + 
            GameController.PlayerCam.transform.GetChild(0).transform.forward;
    }

    // does not check if itemData is removable. if qty == -1, removes all instances
    private int _Remove(ItemData itemData, bool self = true, bool destroy = false, int qty = -1) {
        if(!_quantities.ContainsKey(itemData)) return 0;

        if(self) {
            if(qty == -1 || qty >= _quantities[itemData]) { // all instances removed
                int qtyRemoved = _quantities[itemData];
                _quantities.Remove(itemData);
                _roomRemaining += (itemData.Weight * qtyRemoved);

                for(int i = 0; i < _itemInsertedOrder.Count; i++) { // remove all matching items from insertion order
                    if(_itemInsertedOrder[i].GetComponent<Item>().Data == itemData) {
                        _itemInsertedOrder.RemoveAt(i);
                        i--;
                    }
                }
                foreach(GameObject obj in _items[itemData]) { // drop all items of type
                    obj.GetComponent<Item>().Drop(playerpos(), destroy);
                }
                
                _items.Remove(itemData); // remove the dictionary holding this type of item, since it's now empty

                return qtyRemoved;
            } else { // qty < _quantities[itemData], some instances remain
                _quantities[itemData] -= qty;
                _roomRemaining += (itemData.Weight * qty);

                int haveRemoved = 0;
                for(int i = _itemInsertedOrder.Count - 1; i >= 0; i--) {
                    if(haveRemoved >= qty) break;
                    if(_itemInsertedOrder[i].GetComponent<Item>().Data == itemData) { // remove {qty} matching items from insertion order
                        for(int j = _items[itemData].Count - 1; j >= 0; j--) // remove the item from the dict too
                        {
                            if(_items[itemData][j] == _itemInsertedOrder[i])
                            {
                                _items[itemData].RemoveAt(j);
                                break;
                            }
                        }
                        _itemInsertedOrder[i].GetComponent<Item>().Drop(playerpos(), destroy); // drop the gameobject

                        _itemInsertedOrder.RemoveAt(i);                        
                        i++;
                        haveRemoved++;
                    }
                }

                return qty;
            }
        } else {
            Debug.Log("InventoryItem._Remove (recursive): haven't implemented this yet");
            return 0;
        }
    }
    public int Remove(ItemData itemData, int qtyToRemove, bool destroy = false) {
        if(!itemData.CanDrop) return 0;
        int hasQty = this.Quantity(itemData);
        int totalRemoved = 0;

        while(hasQty > 0 && qtyToRemove > 0) {
            InventoryItem invItem = this.DeepestInventoryItemThatHas(itemData).Item1;
            int amtHeld = invItem.Quantity(itemData, true);
            int numRemoved = invItem._Remove(itemData, true, destroy, Math.Max(amtHeld, qtyToRemove));

            hasQty -= numRemoved;
            qtyToRemove -= numRemoved;
            totalRemoved += numRemoved;
        }

        return totalRemoved;
    }

    private bool RemoveFromInsertedOrder(Item itemToDelete, int index, bool destroy = false) {
        if(_itemInsertedOrder[index] != itemToDelete.gameObject) {
            // item not present in this gameobject, in held inventory item
            foreach(GameObject obj in _itemInsertedOrder) {
                InventoryItem invItem = obj.GetComponent<InventoryItem>();
                if(invItem != null) {
                    if(invItem.RemoveFromInsertedOrder(itemToDelete, index, destroy))
                    {
                        return true;
                    }
                }
            }

            return false;
        } else {
            _roomRemaining += itemToDelete.Data.Weight;

            if(_quantities[itemToDelete.Data] == 1) { // removing last instance
                _quantities.Remove(itemToDelete.Data);
                _items.Remove(itemToDelete.Data);
            } else { // some instances remain
                _quantities[itemToDelete.Data] -= 1;

                for(int j = 0; j < _items[itemToDelete.Data].Count; j++) { // remove from _items
                    if(_items[itemToDelete.Data][j] == itemToDelete.gameObject) {
                        _items[itemToDelete.Data].RemoveAt(j);
                    }
                }
            }

            _itemInsertedOrder.RemoveAt(index);
            itemToDelete.Drop(playerpos(), destroy);

            return true;
        }
    }

    private Tuple<Item, int, int> IdentifyDeepestMostRecentRemovable(bool isInventory = false, int depth = 0) {
        // Item, index in insertion order, depth
        Tuple<Item, int, int> deepestMostRecentRemovableItem = null;

        for(int i = _itemInsertedOrder.Count - 1; i >= 0; i--) {
            Item item = _itemInsertedOrder[i].GetComponent<Item>();
            if(item.Data.CanDrop) {
                if(item.Data.IsInventoryItem) {
                    if(isInventory && deepestMostRecentRemovableItem == null) {
                        deepestMostRecentRemovableItem = Tuple.Create<Item, int, int>(item, i, depth);
                    }

                    Tuple<Item, int, int> deeperValues = item.GetComponent<InventoryItem>().IdentifyDeepestMostRecentRemovable(isInventory, depth + 1);
                    if(deeperValues != null && (deepestMostRecentRemovableItem == null || deeperValues.Item3 > deepestMostRecentRemovableItem.Item3)) {
                        deepestMostRecentRemovableItem = deeperValues;
                    }
                } else { // is not inventory item
                    if(!isInventory) { // not looking for inventory items...
                        if(deepestMostRecentRemovableItem == null) {
                            deepestMostRecentRemovableItem = Tuple.Create<Item, int, int>(item, i, depth);
                        }
                    }
                }             
            }
        }

        if(depth == 0) {
            if(deepestMostRecentRemovableItem != null) {
                Debug.Log($"IdentifyDeepestMostRecentRemovable: returning {deepestMostRecentRemovableItem.Item1.Data.ItemName} which is held at a depth of {deepestMostRecentRemovableItem.Item3}");                
            } else {
                Debug.Log($"IdentifyDeepestMostRecentRemovable: returning nothing");
            }
        }
        return deepestMostRecentRemovableItem;
    }

    public void RemoveMostRecent(bool destroy = false) {
        // Item, insertion order, depth
        Tuple<Item, int, int> mostRecentNonInventoryItem = IdentifyDeepestMostRecentRemovable(false);
        Tuple<Item, int, int> mostRecentInventoryItem = IdentifyDeepestMostRecentRemovable(true);

        if(mostRecentNonInventoryItem == null && mostRecentInventoryItem == null) return; // nothing to remove
        if(mostRecentInventoryItem == null) { RemoveFromInsertedOrder(mostRecentNonInventoryItem.Item1, mostRecentNonInventoryItem.Item2, destroy); return; }
        if(mostRecentNonInventoryItem == null) { RemoveFromInsertedOrder(mostRecentInventoryItem.Item1, mostRecentInventoryItem.Item2, destroy); return; }

        // prioritize dropping inventory items first
        if(mostRecentInventoryItem.Item3 >= mostRecentNonInventoryItem.Item3) {
            RemoveFromInsertedOrder(mostRecentInventoryItem.Item1, mostRecentInventoryItem.Item2, destroy);
        } else {
            RemoveFromInsertedOrder(mostRecentNonInventoryItem.Item1, mostRecentNonInventoryItem.Item2, destroy);
        }

    }
    private int SelfTotalItems(bool droppable)
    {
        int items = 0;
        foreach(KeyValuePair<ItemData, int> pair in _quantities) {
            if(!droppable) {
                items += pair.Value;
            } else if(pair.Key.CanDrop) {
                items += pair.Value;
            }
        }

        return items;
    }

    public int TotalItems(bool droppable) {
        int items = this.SelfTotalItems(droppable);

        foreach(KeyValuePair<ItemData, List<GameObject>> pair in _items) {
            if(pair.Key.IsInventoryItem) {
                foreach(GameObject obj in pair.Value) {
                    items += obj.GetComponent<InventoryItem>().TotalItems(droppable);
                }
            }
        }

        return items;
    }

    public bool HasItems(bool droppable)
    {
        return this.TotalItems(droppable) > 0;
    }

    public string Print(string prefix = "", string suffix = "", bool print = false, int tabs = 0)
    {
        if(tabs != 0) { return new string('\t', tabs) + gameObject.name + ","; }
        string s = "{ ";
        string children = "";
        foreach(KeyValuePair<ItemData, List<GameObject>> entry in _items)
        {
            if(!entry.Key.IsInventoryItem) {
                s += entry.Key.ItemName + " : " + entry.Value.Count.ToString() + ", ";                
            } else {
                foreach(GameObject obj in entry.Value)
                {
                    children += obj.GetComponent<InventoryItem>().Print(new string('\t', tabs + 1), "\n", false, tabs + 1);
                }
            }
        }

        if(s.Length > 2) { s = s.Substring(0, s.Length - 2) + " }"; }
        else { s += "}"; }

        if(children != "") {
            s += "\n" + children;
            s = s.Substring(0, s.Length - 1);
        }

        if(print) {
            Debug.Log(prefix + s + suffix);
        }
        return(prefix + s + suffix);
    }
}
