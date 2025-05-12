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
        public abstract void UpdateData(SaveDataTypes saveDataType);
        public abstract void ApplySavedData(SaveDataTypes saveDataType);
    } // Scope by class SaveData

    public class GameSaveDataV0 : GameSaveData
    {
        // SaveDatas
        public DateTime lastSavedTime;
        public SavedAccountData savedAccountData;
        public SavedStageData savedStageData;
        public SavedAirshipData savedAirshipData;
        public SavedCannonData savedCannonData;
        public SavedRepairerData savedRepairerData;
        public SavedItemData savedItemData;
        public SavedCrewData savedCrewData;
        public SavedDungeonData savedDungeonData;
        public SavedArtifactsData savedArtifactData;
        public SavedUpgradeCounts savedUpgradeCounts;
        public SavedTutorialData savedTutorialData;

        // Temp SaveDatas
        public SavedQuestProgresses savedQuestProgresses;

        public GameSaveDataV0()
        {
            MajorVersion = 0;
            savedAccountData = new SavedAccountData();
            savedStageData = new SavedStageData();
            savedAirshipData = new SavedAirshipData();
            savedCannonData = new SavedCannonData();
            savedRepairerData = new SavedRepairerData();
            savedItemData = new SavedItemData();
            savedCrewData = new SavedCrewData();
            savedDungeonData = new SavedDungeonData();
            savedArtifactData = new SavedArtifactsData();
            savedUpgradeCounts = new SavedUpgradeCounts();
            savedQuestProgresses = new SavedQuestProgresses();
            savedTutorialData = new();

            savedAccountData.InitData();
            savedStageData.InitData();
            savedAirshipData.InitData();
            savedCannonData.InitData();
            savedRepairerData.InitData();
            savedItemData.InitData();
            savedCrewData.InitData();
            savedDungeonData.InitData();
            savedArtifactData.InitData();
            savedUpgradeCounts.InitData();
            savedQuestProgresses.InitData();
            savedTutorialData.InitData();
        }

        public override void Initialize()
        {
            
        }        

        public override void UpdateData(SaveDataTypes saveDataType)
        {
            switch (saveDataType)
            {
                case SaveDataTypes.Account:
                    savedAccountData.UpdateSavedData();
                    break;
                case SaveDataTypes.Stage:
                    savedStageData.UpdateSavedData();
                    break;
                case SaveDataTypes.Airship:
                    savedAirshipData.UpdateSavedData();
                    break;
                case SaveDataTypes.Cannon:
                    savedCannonData.UpdateSavedData();
                    break;
                case SaveDataTypes.Repairer:
                    savedRepairerData.UpdateSavedData();
                    break;
                case SaveDataTypes.Item:
                    savedItemData.UpdateSavedData();
                    break;
                case SaveDataTypes.Crew:
                    savedCrewData.UpdateSavedData();
                    break;
                case SaveDataTypes.Dungeon:
                    savedDungeonData.UpdateSavedData();
                    break;
                case SaveDataTypes.Artifact:

                    break;
                case SaveDataTypes.UpgradeCount:
                    savedUpgradeCounts.UpdateData();
                    break;
                case SaveDataTypes.QuestProgress:
                    savedQuestProgresses.UpdateData();
                    break;
                case SaveDataTypes.Tutorial:
                    savedTutorialData.UpdateSavedData();
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
                case SaveDataTypes.Airship:
                    savedAirshipData.ApplySavedData();
                    break;
                case SaveDataTypes.Cannon:
                    savedCannonData.ApplySavedData();
                    break;
                case SaveDataTypes.Repairer:
                    savedRepairerData.ApplySavedData();
                    break;
                case SaveDataTypes.Item:
                    savedItemData.ApplySavedData();
                    break;
                case SaveDataTypes.Crew:
                    savedCrewData.ApplySavedData();
                    break;
                case SaveDataTypes.Dungeon:
                    savedDungeonData.ApplySavedData();
                    break;
                case SaveDataTypes.Artifact:

                    break;
                case SaveDataTypes.UpgradeCount:
                    savedUpgradeCounts.ApplySavedData();
                    break;
                case SaveDataTypes.QuestProgress:
                    savedQuestProgresses.ApplySavedData();
                    break;
                case SaveDataTypes.Tutorial:
                    savedTutorialData.ApplySavedData();
                    break;
            }
        }

        public override GameSaveData VersionUp()
        {
            throw new System.NotImplementedException();
        }
    }
} // namespace Root