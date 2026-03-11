using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Quest Data", menuName ="ScriptableObjects/Quest Data", order = 1)]
public class QuestData : ScriptableObject {
	public string QuestName;
	public string QuestDesc;
	public NPCData QuestGiver;
	public List<Dictionary<ItemData, int>> itemsRequired;
}