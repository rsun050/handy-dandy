using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quests/Quests")]
public class Quest : ScriptableObject 
{
    public string questName;
    public string questID;
    public string description;
    public List<QuestObjectives> objectives;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = System.Guid.NewGuid().ToString();
        }

    }

    [System.Serializable]

    public class QuestObjectives
    {
        public string Description; 
        public string objectiveID;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
              
    }
     public enum ObjectiveType
    {
        CollectItem,
        DefeatEnemy,
        ExploreArea,
        TalkToNPC,
        Custom
    }

     [System.Serializable]

     public class QuestProgress
    {
        public Quest quest; 
        public List<QuestObjectives> objectivesProgress;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectivesProgress = new List<QuestObjectives>();


            foreach (var objective in quest.objectives)
            {
                    objectivesProgress.Add(new QuestObjectives
                {
                    Description = objective.Description,
                    objectiveID = objective.objectiveID,
                    type = objective.type,
                    requiredAmount = objective.requiredAmount,
                    currentAmount = 0
                });
            }
        }
        public bool IsCompleted => objectivesProgress.TrueForAll(o => o.IsCompleted);
    }
}
