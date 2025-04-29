using Newtonsoft.Json;
using SkyDragonHunter.SaveLoad;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameSaveDataVC = SkyDragonHunter.SaveLoad.GameSaveDataV0;
using LocalSettingSaveDataVC = SkyDragonHunter.SaveLoad.LocalSettingSaveDataV0;

namespace SkyDragonHunter.Managers 
{
    public enum SaveDataTypes
    {
        Account,
        Stage,
        Airship,
        Cannon,
        Repairer,
        Item,
        Crew,
        Dungeon,
        Artifact,
        UpgradeCount,
        QuestProgress,
    }

    public class SaveLoadMgr
    {
        // Fields
        private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
        private static readonly string[] SaveFileName =
        {
            "SDH_SavedGameData.json",
            "SDH_SavedLocalSettingData.json",
        };

        private static JsonSerializerSettings jsonSettings;

        // Properties
        public static int SaveDataMajorVersion { get; private set; } = 0;

            // SaveDatas
        public static GameSaveDataVC GameData {  get; private set; }
        public static LocalSettingSaveDataVC LocalSettingData { get; private set; }
            // ~SaveDatas

        static SaveLoadMgr()
        {
            Init();
        }
        // Public Methods
        public static void UpdateSaveData(params SaveDataTypes[] saveDataTypes)
        {
            if(saveDataTypes == null ||  saveDataTypes.Length == 0)
            {
                Debug.LogError($"Update SaveFile Failed : SaveDataType inavailable");
                return;
            }

            var processed = new HashSet<SaveDataTypes>();
            foreach (var saveDataType in saveDataTypes)
            {
                if (!processed.Add(saveDataType)) // returns false on duplication
                    continue;

                GameData.UpdateData(saveDataType);
                Debug.Log($"Data type [{saveDataType}] Updated to save data successfully");
            }
        }

        public static bool SaveGameData()
        {
            if(GameData == null)
            {
                Debug.LogError($"Save Failed : GameData Null");
                return false;
            }
            if(!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            GameData.lastSavedTime = DateTime.UtcNow;
            var path = Path.Combine(SaveDirectory, SaveFileName[0]);
            var json = JsonConvert.SerializeObject(GameData, jsonSettings);
            File.WriteAllText(path, json);

            Debug.Log($"Game Data Saved");

            return true;
        }

        public static bool LoadGameData()
        {
            var path = Path.Combine(SaveDirectory , SaveFileName[0]);
            if(!File.Exists(path))
            {
                Debug.LogError($"Load Failed : No Game Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var gameData = JsonConvert.DeserializeObject<GameSaveData>(json);

            while (gameData.MajorVersion < SaveDataMajorVersion)
            {
                gameData = gameData.VersionUp();
            }
            GameData = gameData as GameSaveDataVC;

            return true;
        }

        public static bool SaveLocalSettings()
        {
            if (LocalSettingData == null)
            {
                Debug.LogError($"Save Failed : Local Save Data Null");
                return false;
            }
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var path = Path.Combine(SaveDirectory, SaveFileName[1]);
            var json = JsonConvert.SerializeObject(LocalSettingData, jsonSettings);
            File.WriteAllText(path, json);

            Debug.Log($"Local Setting Data Saved");

            return true;
        }

        public static bool LoadLocalSettings()
        {
            var path = Path.Combine(SaveDirectory, SaveFileName[1]);
            if (!File.Exists(path))
            {
                Debug.LogError($"Load Failed : No Local Settings Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var localSettingsData = JsonConvert.DeserializeObject<LocalSettingSaveData>(json);

            while (localSettingsData.MajorVersion < SaveDataMajorVersion)
            {
                localSettingsData = localSettingsData.VersionUp();
            }
            LocalSettingData = localSettingsData as LocalSettingSaveDataVC;

            return true;
        }

        // Private Methods
        private static void Init()
        {
            jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
            };

            if(!LoadGameData())
            {
                InitializeGameData();
            }

            if(!LoadLocalSettings())
            {
                InitializeLocalSettings();
            }
        }

        private static void InitializeGameData()
        {
            GameData = new GameSaveDataVC();
            GameData.Initialize();
            SaveGameData();
        }

        private static void InitializeLocalSettings()
        {
            LocalSettingData = new LocalSettingSaveDataVC();
            LocalSettingData.sfxVolume = 0.2f;
            LocalSettingData.bgmVolume = 0.2f;
            LocalSettingData.fpsSetting = 60;
            LocalSettingData.isVibrationEnabled = true;
            SaveLocalSettings();
        }
    } // Scope by class SaveLoadMgr

} // namespace Root