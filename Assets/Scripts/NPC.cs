using UnityEngine;

public class NPC : MonoBehaviour
{
	[field: SerializeField] public NPCData npcData { get; private set; }
	private int quest = -1;
	private bool questIP = false;
	// private bool isOfferingQuest = false;
	
	[SerializeField] private QuestData[] quests;
	[SerializeField] private DialogueFragment[] questStartDialogues; // accept a quest
	[SerializeField] private DialogueFragment[] questIPDialogues; // quest active but not complete
	[SerializeField] private DialogueFragment[] questCompleteDialogues; // quest completed
	[SerializeField] private DialogueFragment[] defaultDialogue; // miscellaneous dialogue, unimportant
	[SerializeField] private AudioSource talkSfx;
	private void Awake()
	{
		
	}

	private void Update()
	{
		Vector3 playerpos = GameController.Player.transform.position;
		playerpos.y = transform.position.y;
		transform.LookAt(playerpos); // always face the player
	}

	private void questaccepted(bool accepted) {
		if(accepted) {
			quest += 1;
			questIP = true;
		}
		// isOfferingQuest = false;
		QuestManager.Instance.questAcceptedE -= questaccepted;
	}

	public void Interact() {
		if(quests.Length == 0 && defaultDialogue.Length == 0) {
			// this NPC has no dialogue!
			// Debug.Log("This NPC has no dialogue");
			return;
		}

		talkSfx.Play();

		if(quest >= quests.Length) {
			// all quests exhausted, fallback to default dialogues
			// Debug.Log("NPC dialogue exhausted, using default");
			if(defaultDialogue.Length > 0) {
				// play a random greeting
				int i = Random.Range(0, defaultDialogue.Length);
				DialogueManager.Instance.StartDialogue(defaultDialogue[i]);
			}
		}
		else if(!questIP) {
			// start a new quest
			// Debug.Log("Offering quest");
			// isOfferingQuest = true;
			QuestManager.Instance.questAcceptedE += questaccepted;
			DialogueManager.Instance.StartDialogue(questStartDialogues[quest + 1]);
		} else { // questIP
			// quest is in progress: check if the quest is complete
			if(QuestManager.Instance.questIsComplete()) {
				// play questCompleteDialogue
				// Debug.Log("Quest completed");

				questIP = false;
				QuestManager.Instance.CompleteQuest();
				DialogueManager.Instance.StartDialogue(questCompleteDialogues[quest]);
			} else {
				// Debug.Log("Quest IP, not complete");
				// play questIPDialogue
				DialogueManager.Instance.StartDialogue(questIPDialogues[quest]);
			}
		}
	}
}