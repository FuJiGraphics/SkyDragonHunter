using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public enum QuestType
    {
        Repetitive,
        Daily,
        Weekly,
        Achievements,
    }

    public class SavedQuest
    {
        QuestType questType;
        public int questID;
        public int accumulatedProcess;
        public bool isCompleted;
        public bool isRewarded;
    }

    public class SavedQuestProgresses
    {
        public Dictionary<QuestType, Dictionary<int, SavedQuest>> questsDict;

        public void InitData()
        {
            questsDict = new Dictionary<QuestType, Dictionary<int, SavedQuest>>();

        }

        public void UpdateData()
        {

        }

        public void ApplySavedData()
        {

        }

    } // Scope by class SavedQuestProgresses

} // namespace Root