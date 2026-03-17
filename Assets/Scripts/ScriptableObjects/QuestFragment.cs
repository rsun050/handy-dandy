using UnityEngine;

public enum QuestAction {Remove, Add};
// any sort of npc interaction that changes the user's quests
[CreateAssetMenu(fileName = "Quest Fragment", menuName = "ScriptableObjects/Quest Fragment", order = 1)]
public class QuestFragment : Fragment
{
	public QuestData quest;
	public QuestAction action;	
}