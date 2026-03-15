using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField] private NPCData npcData;
	private int quest = -1;
	
	[SerializeField] private QuestData[] quests;
	[SerializeField] private DialogueFragment[] questStartDialogues; // accept a quest
	[SerializeField] private DialogueFragment[] questIPDialogues; // quest active but not complete
	[SerializeField] private DialogueFragment[] questCompleteDialogues; // quest completed
	[SerializeField] private DialogueFragment[] greetings; // miscellaneous dialogue, unimportant
	private void Awake()
	{
		
	}

	private void Update()
	{
		
	}

	public void Interact()
	{
		// all quests exhausted
		if(quest >= quests.Length) {
			if(greetings.Length > 0)
			{
				
			}
		}
		if(quest < 0) {
			
		}
	}
}