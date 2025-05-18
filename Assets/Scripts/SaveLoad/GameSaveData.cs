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
        public SavedShopItemsData savedShopItemData;
        public SavedMasteryData savedMasteryData;
        public SavedGrowthData savedGrowthData;
        public SavedFacilityData savedFacilityData;

        // Temp SaveDatas
        public SavedQuestProgresses savedQuestProgresses;

        public bool IsLoadedDone { get; set; } = false;

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
            savedTutorialData = new SavedTutorialData();
            savedMasteryData = new SavedMasteryData();
            savedGrowthData = new SavedGrowthData();
            savedTutorialData = new();
            savedShopItemData = new SavedShopItemsData();
            savedFacilityData = new SavedFacilityData();

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
            savedMasteryData.InitData();
            savedGrowthData.InitData();
            savedShopItemData.InitData();
            savedFacilityData.InitData();
        }

        public override void Initialize()
        {
            
        }        

        public override void UpdateData(SaveDataTypes saveDataType)
        {
            if (!IsLoadedDone)
                return;

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
                    savedArtifactData.UpdateSavedData();
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
                case SaveDataTypes.Mastery:
                    savedMasteryData.UpdateSavedData();
                    break;
                case SaveDataTypes.Growth:
                    savedGrowthData.UpdateSavedData();
                    break;
                case SaveDataTypes.ShopItem:
                    savedShopItemData.UpdateSavedData();
                    break;
                case SaveDataTypes.Facility:
                    savedFacilityData.UpdateSavedData();
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
                    savedArtifactData.ApplySavedData();
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
                case SaveDataTypes.Mastery:
                    savedMasteryData.ApplySavedData();
                    break;
                case SaveDataTypes.Growth:
                    savedGrowthData.ApplySavedData();
                    break;
                case SaveDataTypes.ShopItem:
                    savedShopItemData.ApplySavedData();
                    break;
                case SaveDataTypes.Facility:
                    savedFacilityData.ApplySavedData();
                    break;
            }
        }

        public override GameSaveData VersionUp()
        {
            throw new System.NotImplementedException();
        }
    }
} // namespace Root