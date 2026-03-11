using System.Collections.Generic;
using UnityEngine;

public enum NPCState { NotMet, QuestIP, QuestComplete };
public class NPC : MonoBehaviour
{
	private NPCData npcData;
	private NPCState npcState;
	
	private QuestData[] quests;
	private DialogueFragment[] questStartDialogues;
	private DialogueFragment[] questIPDialogues;
	private DialogueFragment[] questCompleteDialogues;
	private DialogueFragment[] greetings;
	private void Awake()
	{
		
	}

	private void Update()
	{
		
	}

	public void Interact()
	{
		
	}

}