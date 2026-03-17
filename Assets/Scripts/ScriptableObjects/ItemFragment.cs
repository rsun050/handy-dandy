using System.Collections.Generic;
using UnityEngine;

// any sort of npc interaction that changes the user's inventory
[CreateAssetMenu(fileName = "Item Fragment", menuName = "ScriptableObjects/Item Fragment", order = 1)]
public class ItemFragment : Fragment
{
	public List<ItemData> items;
	public List<int> quantityChange;
}