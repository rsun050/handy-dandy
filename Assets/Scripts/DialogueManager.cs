using System;
using TMPro;
using UnityEngine;

public enum Choice { Negative, Affirmative };
public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }
	private bool _busy = false;
	private bool _showingOptions = false;
	public int _playerPickedOption = -1;
	private int _playingDialogueLine = 0;
	private Fragment _currentFragment = null;

	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private TMP_Text _dialogueText;
	[SerializeField] private GameObject _optionsUI;
	[SerializeField] private UnityEngine.UI.Button[] _dialogueOptions;

	public event Action toggleCamLockE;
	public event Action<Choice> optionChosenE;

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

		_dialogueUI.SetActive(false);
	}

	private void Start() {
		this.optionChosenE += ProcessPlayerChoice;
	}

	private void Update() {
		if(_busy && !_showingOptions) { // player proceeds dialogue by mouse click
			if(Input.GetKeyDown(KeyCode.Mouse0)) {
				NextDialogueLine();
			}
		}
	}

	public void StartDialogue(Fragment fragment) {
		// Debug.Log("Starting dialogue");

		if(_busy) return;
		_busy = true;
		_showingOptions = false;
		GameController.Player._canMove = false;

		_currentFragment = fragment;
		ProcessDialogue();
	}

	private void ContinueDialogue(Fragment fragment) {
		// Debug.Log("Continuing dialogue");

		_playingDialogueLine = 0;
		_showingOptions = false;
		_currentFragment = fragment;
		ProcessDialogue();
	}

	private void ProcessDialogue() {
		switch(_currentFragment) {
			case DialogueFragment df:
				// Debug.Log("Processing dialogue fragment");
				if(_playingDialogueLine >= df.dialogue.Count) {
					// exhausted NPC dialogue in this fragment, move on to player choices
					ProcessOptions();

					if(!_showingOptions) { // no options to display, proceed to next fragment
						ContinueDialogue(df.nextFragments[0]);
					}
				} else {
					DisplayDialogue();
				}
				break;
			case ItemFragment itf:
				// Debug.Log("Processing item fragment");
				for(int i = 0; i < itf.items.Count; i++) {
					ItemData item = itf.items[i];
					int qty = itf.quantityChange[i];

					if(qty < 0) { // remove and destroy
						InventoryManager.Instance.Remove(item, -1 * qty, true);
					} else { // instantiate new items and add
						for(int j = 0; j < qty; j++) {
							GameObject new_item = Instantiate(item.Prefab);
							if(!InventoryManager.Instance.PickUp(new_item)) {
								// fail to pick up for some reason, just drop it on the ground
								new_item.transform.position = GameController.Player.transform.position + Vector3.forward + Vector3.up;
							}
						}
					}
				}
				ContinueDialogue(itf.nextFragments[0]);
				break;
			case QuestFragment qf:
				// Debug.Log("Processing quest fragment");
				switch(qf.action) {
					case QuestAction.Remove:
						QuestManager.Instance.RemoveQuest();
						break;
					case QuestAction.Add:
						QuestManager.Instance.SetNewQuest(qf.quest);
						break;
				}
				ContinueDialogue(qf.nextFragments[0]);
				break;
			case null:
				DisplayDialogue();
				break;
			default: // unknown fragment type...
				// Debug.Log("DialogueManager: hit an unknown fragment type - should only use DialogueFragment, ItemFragment, or QuestFragment");
				break;
		}
	}

	private bool ProcessOptions() {
		// Debug.Log("Processing options");
		DialogueFragment df = (DialogueFragment)_currentFragment;

		if(df.playerOptions.Count > 0) {
			_showingOptions = true;
			_playerPickedOption = -1;

			DisplayOptions();
			return true;
		} else {
			// player has no options in this fragment, move on to next fragment
			_currentFragment = (df.nextFragments.Count > 0) ? df.nextFragments[0] : null;

			return false;
		}
	}

	public void OptionNegative() {
		optionChosenE?.Invoke(Choice.Negative);
		// _playerPickedOption = 0;
		// ProcessPlayerChoice();
	}

	public void OptionAffirmative() {
		optionChosenE?.Invoke(Choice.Affirmative);
		// _playerPickedOption = 1;
		// ProcessPlayerChoice();
	}

	public void ProcessPlayerChoice(Choice playerChoice) {
		// Debug.Log("Processing choice");
		toggleCamLockE?.Invoke();
		_showingOptions = false;

		Fragment next_frag = _currentFragment.nextFragments[(int)playerChoice];
		ContinueDialogue(next_frag);
	}

	private void DisplayDialogue() { // UI stuff
		// Debug.Log("Displaying dialogue");
 		if(_currentFragment == null) {
			EndDialogue();
		} else {
			DialogueFragment df = (DialogueFragment)_currentFragment;
			_dialogueText.text = df.dialogue[_playingDialogueLine];
			_optionsUI.SetActive(false);
			_dialogueText.gameObject.SetActive(true);
			_dialogueUI.SetActive(true);
		}
	}

	private void DisplayOptions() {
		// Debug.Log("Displaying options");
		DialogueFragment df = (DialogueFragment)_currentFragment;

		_dialogueText.gameObject.SetActive(false);

		for(int i = 0; i < _dialogueOptions.Length; i++) {
			if(i < df.playerOptions.Count)
			{
				_dialogueOptions[i].gameObject.SetActive(true);
				_dialogueOptions[i].GetComponentInChildren<TMP_Text>().text = df.playerOptions[i];				
			} else {
				_dialogueOptions[i].gameObject.SetActive(false);
			}
		}

		_optionsUI.SetActive(true);
		toggleCamLockE?.Invoke();
	}

	private void EndDialogue() {
		// Debug.Log("Dialogue ends");
		_busy = false;
		_showingOptions = false;
		_dialogueUI.SetActive(false);
		_playingDialogueLine = 0;
		GameController.Player._canMove = true; // RELEASE ME!!!!
	}

	private void NextDialogueLine() {
		_playingDialogueLine += 1;
		ProcessDialogue();
	}
}