using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance { get; private set; }
	public QuestData _currentQuest { get; private set; }

	public event Action newQuestReceivedE;
	public event Action<bool> questAcceptedE;

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

		_currentQuest = null; 
	}

	public bool questIsComplete() {
		if(_currentQuest != null) {
			for(int i = 0; i < _currentQuest.requiredItems.Length; i++) {
				if(InventoryManager.Instance.Quantity(_currentQuest.requiredItems[i]) < _currentQuest.requiredQuantities[i]) {
					return false;
				}
			}
		}

		return true;
	}

	public void CompleteQuest() {
		takeQuestItems();
		RemoveQuest();
	}

	private void takeQuestItems() {
		if(_currentQuest != null) {
			for(int i = 0; i < _currentQuest.requiredItems.Length; i++) {
				InventoryManager.Instance.Remove(_currentQuest.requiredItems[i], _currentQuest.requiredQuantities[i], true);
			}
		}
	}

	public void SetNewQuest(QuestData quest)
	{
		_currentQuest = quest;
		newQuestReceivedE?.Invoke();
		questAcceptedE.Invoke(true);
	}

	public void RemoveQuest() {
		_currentQuest = null;
		newQuestReceivedE?.Invoke();
		questAcceptedE?.Invoke(false);
	}
}