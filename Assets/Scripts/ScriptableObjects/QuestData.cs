using UnityEngine;

public enum MustPass { Requirements, Optional };
[CreateAssetMenu(fileName = "Quest Data", menuName ="ScriptableObjects/Quest Data", order = 1)]
public class QuestData : ScriptableObject {
	public string QuestName;
	public string QuestDesc;
	public NPCData QuestGiver;

	public MustPass[] passingRequirements;

	[SerializeField, Tooltip("If Requirements is in passingRequirements, all required conditions must pass")]
	public ItemData[] requiredItems;
	public int[] requiredQuantities;
	[SerializeField, Tooltip("If Optional is in passingRequirements, as long as {numberOfOptionalChecksThatMustPass} of these conditions pass, the quest can be completed")]
	public ItemData[] optionalItems;
	public int numberOfOptionalChecksThatMustPass;
	public int[] optionalQuantities;
}