using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System;

namespace SkyDragonHunter.SaveLoad 
{    
    public abstract class GameSaveData
    {
        public int MajorVersion { get; protected set; }
        public abstract GameSaveData VersionUp();
        public abstract void Initialize();
        public abstract void UpdateSavedData(SaveDataTypes saveDataType);
        public abstract void ApplySavedData(SaveDataTypes saveDataType);
    } // Scope by class SaveData

    public class GameSaveDataV0 : GameSaveData
    {
        // SaveDatas
        public DateTime lastSavedTime;
        public SavedAccountData savedAccountData;
        public SavedStageData savedStageData;
        public SavedCrewData savedCrewData;
        public SavedAirshipData savedAirshipData;
        public SavedCannonData savedCannonData;
        public SavedRepairerData savedRepairerData;
        public SavedItemData savedItemData;
        public SavedDungeonData savedDungeonData;
        public SavedArtifactsData savedArtifactData;
        public SavedUpgradeCounts savedUpgradeCounts;

        // Temp SaveDatas
        public SavedQuestProgresses savedQuestProgresses;

        public GameSaveDataV0()
        {
            Initialize();
        }

        public override void Initialize()
        {
            MajorVersion = 0;
            savedAccountData = new SavedAccountData();
            savedStageData = new SavedStageData();
            savedCrewData = new SavedCrewData();
            savedAirshipData = new SavedAirshipData();
            savedCannonData = new SavedCannonData();
            savedRepairerData = new SavedRepairerData();
            savedItemData = new SavedItemData();
            savedDungeonData = new SavedDungeonData();
            savedArtifactData = new SavedArtifactsData();
            savedUpgradeCounts = new SavedUpgradeCounts();
            savedQuestProgresses = new SavedQuestProgresses();
        }        

        public override void UpdateSavedData(SaveDataTypes saveDataType)
        {
            switch (saveDataType)
            {
                case SaveDataTypes.Account:
                    savedAccountData.UpdateSavedData();
                    break;
                case SaveDataTypes.Stage:
                    savedStageData.UpdateSavedData();
                    break;
                case SaveDataTypes.Crew:
                    savedCrewData.UpdateSavedData();
                    break;
                case SaveDataTypes.Airship:
                    savedAirshipData.UpdateSavedData();
                    break;
                case SaveDataTypes.Cannon:
                    
                    break;
                case SaveDataTypes.Repairer:

                    break;
                case SaveDataTypes.Item:
                    savedItemData.UpdateSavedData();
                    break;
                case SaveDataTypes.Dungeon:

                    break;
                case SaveDataTypes.Artifact:

                    break;
                case SaveDataTypes.UpgradeCount:
                    savedUpgradeCounts.UpdateSavedData();
                    break;
                case SaveDataTypes.QuestProgress:
                    savedQuestProgresses.UpdateSavedData();
                    break;
            }
        }

        public override void ApplySavedData(SaveDataTypes saveDataType)
        {
            switch (saveDataType)
            {
                case SaveDataTypes.Account:
                    savedAccountData.ApplySavedData();
                    break;
                case SaveDataTypes.Stage:
                    savedStageData.ApplySavedData();
                    break;
                case SaveDataTypes.Crew:
                    savedCrewData.ApplySavedData();
                    break;
                case SaveDataTypes.Airship:
                    savedAirshipData.ApplySavedData();
                    break;
                case SaveDataTypes.Cannon:

                    break;
                case SaveDataTypes.Repairer:

                    break;
                case SaveDataTypes.Item:
                    savedItemData.ApplySavedData();
                    break;
                case SaveDataTypes.Dungeon:

                    break;
                case SaveDataTypes.Artifact:

                    break;
                case SaveDataTypes.UpgradeCount:
                    savedUpgradeCounts.ApplySavedData();
                    break;
                case SaveDataTypes.QuestProgress:
                    savedQuestProgresses.ApplySavedData();
                    break;
            }
        }

        public override GameSaveData VersionUp()
        {
            throw new System.NotImplementedException();
        }
    }
} // namespace Root