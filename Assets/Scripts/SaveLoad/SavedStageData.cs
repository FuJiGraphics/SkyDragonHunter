using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedStageData
    {
        private const int lastStage = 10;

        public int currentStage;
        public int currentZone;

        public int clearedStage;
        public int clearedZone;

        public SavedStageData()
        {
            InitData();
        }

        public void InitData()
        {
            currentStage = 1;
            currentZone = 1;

            clearedStage = 0;
            clearedZone = 0;
        }
        public void UpdateSavedData()
        {
            currentStage = AccountMgr.CurrentStageLevel;
            currentZone = AccountMgr.CurrentStageZoneLevel;

            ConvertLastTriedStageToClearedStage(AccountMgr.LastStageLevel, AccountMgr.LastZoneLevel, out int newClearedStage, out int newClearedZone);

            clearedStage = newClearedStage;
            clearedZone = newClearedZone;
        }

        public void ApplySavedData()
        {
            ConvertClearedStageToLastTriedStage(clearedStage, clearedZone, out int lastTriedStage, out int lastTriedZone);

            AccountMgr.CurrentStageLevel = currentStage;
            AccountMgr.CurrentStageZoneLevel = currentZone;

            AccountMgr.LastStageLevel = lastTriedStage;
            AccountMgr.LastZoneLevel = lastTriedZone;
        }
        public void LateApplySavedData()
        {

        }

        public bool ConvertLastTriedStageToClearedStage(int lastTriedStage, int lastTriedZone, out int clearedStage, out int clearedZone)
        {
            int newClearedStage = 0;
            int newClearedZone = 0;

            if (lastTriedZone == 1 && lastTriedStage > 1)
            {
                newClearedStage = lastTriedZone - 1;
                newClearedZone = 20;
            }
            else if (lastTriedStage == 1)
            {
                newClearedStage = 0;
                newClearedZone = 0;
            }
            else
            {
                newClearedStage = lastTriedStage;
                newClearedZone = lastTriedZone - 1;
            }

            clearedStage = newClearedStage;
            clearedZone = newClearedZone;

            return true;
        }

        public bool ConvertClearedStageToLastTriedStage(int clearedStage, int clearedZone, out int lastTriedStage, out int lastTriedZone)
        {
            int triedStage = 0;
            int triedZone = 0;

            if (clearedZone >= 20 && clearedStage < lastStage)
            {
                triedStage = clearedStage + 1;
                triedZone = 1;
            }
            else if (clearedStage >= lastStage)
            {
                triedStage = lastStage;
                triedZone = 20;
            }
            else
            {
                triedStage = clearedStage + 1;
                triedZone = clearedZone;
            }

            lastTriedStage = triedStage;
            lastTriedZone = triedZone;

            return true;
        }
    } // Scope by class SavedStageData

} // namespace Root