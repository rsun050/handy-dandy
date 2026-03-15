using TMPro;
using UnityEngine;

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

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

		_dialogueUI.SetActive(false);
	}

	private void Update() {
		if(_busy && !_showingOptions) { // player proceeds dialogue by mouse click
			if(Input.GetKey(KeyCode.Mouse0)) {
				Next();
			}
		}
	}

	public void StartDialogue(Fragment fragment) {
		if(_busy) return;
		_busy = true;
		_showingOptions = false;
		GameController.Player._canMove = false;

		_currentFragment = fragment;
		ProcessDialogue();
	}

	private void ContinueDialogue(Fragment fragment) {
		_showingOptions = false;
		_currentFragment = fragment;
		ProcessDialogue();
	}

	private void ProcessDialogue() {
		switch(_currentFragment) {
			case DialogueFragment df:
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

				break;
			case null:
				DisplayDialogue();
				break;
			default: // unknown fragment type...
				Debug.Log("DialogueManager: hit an unknown fragment type - should only use DialogueFragment or ItemFragment");
				break;
		}
	}

	private bool ProcessOptions() {
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

	public void ProcessPlayerChoice() {
		if(_showingOptions) {
			Fragment next_frag = _currentFragment.nextFragments[_playerPickedOption];
			ContinueDialogue(next_frag);
		}
	}

	private void DisplayDialogue() { // UI stuff
 		if(_currentFragment == null) {
			EndDialogue();
		} else {
			DialogueFragment df = (DialogueFragment)_currentFragment;
			_dialogueText.text = df.dialogue[_playingDialogueLine];
		}
	}

	private void DisplayOptions() {
		DialogueFragment df = (DialogueFragment)_currentFragment;

		for(int i = 0; i < df.playerOptions.Count; i++) {
			_dialogueOptions[i].gameObject.SetActive(true);
			_dialogueOptions[i].GetComponent<TMP_Text>().text = df.playerOptions[i];
		}
	}

	private void EndDialogue() {
		_busy = false;
		_showingOptions = false;
		_dialogueUI.SetActive(false);
		GameController.Player._canMove = true; // RELEASE ME!!!!
	}

	private void Next() {
		_playingDialogueLine += 1;
		ProcessDialogue();
	}
}