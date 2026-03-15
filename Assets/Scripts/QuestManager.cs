using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance { get; private set; }
	public QuestData _currentQuest { get; private set; }

	public event Action newQuestReceivedE;

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

		_currentQuest = null;
	}

	public void SetNewQuest(QuestData quest)
	{
		_currentQuest = quest;
		newQuestReceivedE?.Invoke();
	}
}