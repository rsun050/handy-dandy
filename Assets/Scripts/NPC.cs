using UnityEngine;

public class NPC : MonoBehaviour
{
	[field: SerializeField] public NPCData npcData { get; private set; }
	private int quest = 0;
	private bool questIP = false;
	// private bool isOfferingQuest = false;
	
	[SerializeField] private QuestData[] quests;
	[SerializeField] private DialogueFragment[] questStartDialogues; // accept a quest
	[SerializeField] private DialogueFragment[] questIPDialogues; // quest active but not complete
	[SerializeField] private DialogueFragment[] questCompleteDialogues; // quest completed
	[SerializeField] private DialogueFragment[] defaultDialogue; // miscellaneous dialogue, unimportant
	[SerializeField] private AudioSource talkSfx;
	[SerializeField] private Animator animator;
	private void Awake() {
		
	}

	private void Start() {
		UpdateQuestIcon();
		if(animator) {animator.SetBool("talking", false); }
	}

	private void UpdateQuestIcon() {
		if(quest >= quests.Length) {
			transform.GetChild(0).gameObject.SetActive(false);
		} else {
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		Vector3 playerpos = GameController.Player.transform.position;
		playerpos.y = transform.position.y;
		transform.LookAt(playerpos); // always face the player
	}

	private void questaccepted(bool accepted) {
		if(accepted) {
			questIP = true;
		}
		// isOfferingQuest = false;
		QuestManager.Instance.questAcceptedE -= questaccepted;
	}

	private void StopYapAnim() {
		animator.SetBool("talking", false);

		DialogueManager.Instance.dialogueEndE -= StopYapAnim;
	}

	public void Interact() {
		if(quests.Length == 0 && defaultDialogue.Length == 0) { // this NPC has no dialogue!
			return;
		}

		talkSfx.Play();

		if(animator != null) {
			animator.SetBool("talking", true );
			DialogueManager.Instance.dialogueEndE += StopYapAnim;			
		}

		if(quest >= quests.Length) { // all quests exhausted, fallback to default dialogues
			if(defaultDialogue.Length > 0) {
				// play a random greeting
				int i = Random.Range(0, defaultDialogue.Length);
				DialogueManager.Instance.StartDialogue(defaultDialogue[i]);
			}
		} else if(!questIP) { // start a new quest
			QuestManager.Instance.questAcceptedE += questaccepted;
			DialogueManager.Instance.StartDialogue(questStartDialogues[quest]);
		} else { // quest in progress, check if the quest is complete
			if(QuestManager.Instance.questIsComplete()) { // quest is complete
				QuestManager.Instance.CompleteQuest();
				DialogueManager.Instance.StartDialogue(questCompleteDialogues[quest]);
				questIP = false;
				quest += 1;
				UpdateQuestIcon();
			} else { // quest NOT complete
				DialogueManager.Instance.StartDialogue(questIPDialogues[quest]);
			}
		}
	}
}