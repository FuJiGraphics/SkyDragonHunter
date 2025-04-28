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

    public class SavedQuestProgresses
    {
        QuestType questType;
        public int questID;
        public int accumulatedProcess;
        public bool isCompleted;
        public bool isRewarded;

        public void InitData()
        {

        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedQuestProgresses

} // namespace Root