using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; private set; }
	private bool _busy = false;
	private Fragment _currentFragment = null;

	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private TMP_Text _dialogueText;
	[SerializeField] private UnityEngine.UI.Button _dialogueOption1;
	[SerializeField] private UnityEngine.UI.Button _dialogueOption2;

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

		_dialogueUI.SetActive(false);
	}

	private void Update() {
		
	}

	public void StartDialogue(Fragment fragment) {
		if(_busy) return;
		_busy = true;

		_currentFragment = fragment;
		ProcessDialogue();
	}

	private void ProcessDialogue() {
		switch(_currentFragment) {
			case DialogueFragment df:

				break;
			case ItemFragment itf:
				for(int i = 0; i < itf.items.Count; i++) {
					ItemData item = itf.items[i];
					int qty = itf.quantityChange[i];

					if(qty < 0) { // remove and destroy
						InventoryManager.Instance.Remove(item, -1 * qty, true);
					} else { // instantiate new items and add
						
					}
				}

				break;
			case null:
				break;
			default: // unknown fragment type...
				Debug.Log("DialogueManager: hit an unknown fragment type - should only use DialogueFragment or ItemFragment");
				break;
		}
	}

	private void Next() {
		
	}
}