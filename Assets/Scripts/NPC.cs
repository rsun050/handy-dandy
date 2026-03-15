using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField] private NPCData npcData;
	private int quest = -1;
	private bool questIP = false;
	
	[SerializeField] private QuestData[] quests;
	[SerializeField] private DialogueFragment[] questStartDialogues; // accept a quest
	[SerializeField] private DialogueFragment[] questIPDialogues; // quest active but not complete
	[SerializeField] private DialogueFragment[] questCompleteDialogues; // quest completed
	[SerializeField] private DialogueFragment[] defaultDialogue; // miscellaneous dialogue, unimportant
	private void Awake()
	{
		
	}

	private void Update()
	{
		
	}

	public void Interact()
	{
		Debug.Log($"Interacting with {npcData.NPCName}");
		if(quests.Length == 0 && defaultDialogue.Length == 0)
		{
			// this NPC has no dialogue!
			return;
		}

		if(quest >= quests.Length) {
			// all quests exhausted, fallback to default dialogues
			if(defaultDialogue.Length > 0) {
				// play a random greeting
				int i = Random.Range(0, defaultDialogue.Length - 1);
				DialogueManager.Instance.StartDialogue(defaultDialogue[i]);
			}
		}
		else if(quest < 0 && !questIP && quests.Length > 0) {
			// start a new quest
			quest += 1;
		} else if(questIP) {
			// quest is in progress: check if the quest is complete

			if(questIsComplete()) {
				// play questCompleteDialogue

				questIP = false;
			} else {
				// play questIPDialogue

			}
		}
	}

	private bool questIsComplete() {
		return false; // TODO
	}
}