using Newtonsoft.Json;
using SkyDragonHunter.SaveLoad;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        EquipmentUI,
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
        private static bool initialized = false;

        // TODO: LJH TEMP
        private static bool loadedOnce = false;
        // ~TODO

            // SaveDatas
        public static GameSaveDataVC GameData {  get; private set; }
        public static LocalSettingSaveDataVC LocalSettingData { get; private set; }
            // ~SaveDatas

        static SaveLoadMgr()
        {
            jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            jsonSettings.Converters.Add(new Vector3Converter());
            jsonSettings.Converters.Add(new BigNumConverter());
            jsonSettings.Converters.Add(new ItemTableDataConverter());
            jsonSettings.Converters.Add(new CrewTableDataConverter());

            initialized = false;
            loadedOnce = false;

            GameData = new GameSaveDataVC();
            LocalSettingData = new LocalSettingSaveDataVC();
        }
        // Public Methods
        public static void UpdateSaveData()
            => UpdateSaveData((SaveDataTypes[])Enum.GetValues(typeof(SaveDataTypes)));

        public static void UpdateSaveData(params SaveDataTypes[] saveDataTypes)
        {
            if (saveDataTypes == null ||  saveDataTypes.Length == 0)
            {
                Debug.LogError($"Update SaveFile Failed : SaveDataType unavailable");
                return;
            }

            var processed = new HashSet<SaveDataTypes>();
            foreach (var saveDataType in saveDataTypes)
            {
                if (!processed.Add(saveDataType)) // returns false on duplication
                    continue;

                GameData.UpdateData(saveDataType);
                // Debug.Log($"Data type [{saveDataType}] Updated to save data successfully");
            }
        }
        public static void ApplySavedData()
            => ApplySavedData((SaveDataTypes[])Enum.GetValues(typeof(SaveDataTypes)));

        public static void ApplySavedData(params SaveDataTypes[] saveDataTypes)
        {
            if (saveDataTypes == null || saveDataTypes.Length == 0)
            {
                Debug.LogError($"Apply Saved File Failed : SaveDataType unavailable");
                return;
            }

            var processed = new HashSet<SaveDataTypes>();
            foreach (var saveDataType in saveDataTypes)
            {
                if (!processed.Add(saveDataType)) // returns false on duplication
                    continue;

                GameData.ApplySavedData(saveDataType);
                // Debug.Log($"Saved Data type [{saveDataType}] applied to game successfully");
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

            UpdateSaveData();

            GameData.lastSavedTime = DateTime.UtcNow;
            var path = Path.Combine(SaveDirectory, SaveFileName[0]);
            var json = JsonConvert.SerializeObject(GameData, jsonSettings);
            File.WriteAllText(path, json);

            Debug.Log($"Game Data Saved");

            return true;
        }

        public static bool LoadGameData()
        {
            if(loadedOnce)
            {
                Debug.LogWarning($"[TEMP] Already Loaded Once, Cancel repeated loading");
            }
            var path = Path.Combine(SaveDirectory , SaveFileName[0]);
            if(!File.Exists(path))
            {
                Debug.LogWarning($"Load Failed : No Game Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var gameData = JsonConvert.DeserializeObject<GameSaveDataVC>(json, jsonSettings);

            while (gameData.MajorVersion < SaveDataMajorVersion)
            {
                gameData = gameData.VersionUp() as GameSaveDataVC;
            }
            GameData = gameData as GameSaveDataVC;

            ApplySavedData();

            loadedOnce = true;
            return true;
        }

        public static bool LoadGameData(Scene scene)
        {
            Debug.LogError($"using scene argument to Load Game Data Currently not supported");
            LoadGameData();

            return false;
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
                Debug.LogWarning($"Load Failed : No Local Settings Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var localSettingsData = JsonConvert.DeserializeObject<LocalSettingSaveDataVC>(json);

            while (localSettingsData.MajorVersion < SaveDataMajorVersion)
            {
                localSettingsData = localSettingsData.VersionUp() as LocalSettingSaveDataVC;
            }
            LocalSettingData = localSettingsData as LocalSettingSaveDataVC;

            return true;
        }
        
        public static void Init()
        {
            if (initialized)
                return;

            if (!LoadGameData())
            {
                InitializeGameData();
            }

            if(!LoadLocalSettings())
            {
                InitializeLocalSettings();
            }

            initialized = true;
        }

        // Private Methods
        private static void InitializeGameData()
        {
            GameData.Initialize();
            SaveGameData();
        }

        private static void InitializeLocalSettings()
        {
            LocalSettingData.sfxVolume = 0.2f;
            LocalSettingData.bgmVolume = 0.2f;
            LocalSettingData.fpsSetting = 60;
            LocalSettingData.isVibrationEnabled = true;
            SaveLocalSettings();
        }        
    } // Scope by class SaveLoadMgr

} // namespace Root