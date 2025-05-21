using Newtonsoft.Json;
using SkyDragonHunter;
using SkyDragonHunter.SaveLoad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameSaveDataVC = SkyDragonHunter.SaveLoad.GameSaveDataV0;
using LocalSettingSaveDataVC = SkyDragonHunter.SaveLoad.LocalSettingSaveDataV0;

#if UNITY_EDITOR
    using UnityEditor;
#endif

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
        Tutorial,
        Mastery,
        Growth,
        ShopItem,
        Facility,
    }

    public class SaveLoadMgr
    {
        // Fields
        private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
        private static readonly string[] SaveFileName =
        {
            "SDH_SavedGameDataV3.json",
            "SDH_SavedLocalSettingDataV3.json",
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
            if (!Application.isPlaying)
                return;

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

        public static void ResetLoaded() => loadedOnce = false;
        public static void UpdateSaveData()
            => UpdateSaveData((SaveDataTypes[])Enum.GetValues(typeof(SaveDataTypes)));

        public static void UpdateSaveData(params SaveDataTypes[] saveDataTypes)
        {
            if (saveDataTypes == null ||  saveDataTypes.Length == 0)
            {
                // Debug.LogError($"Update SaveFile Failed : SaveDataType unavailable");
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
                // Debug.LogError($"Apply Saved File Failed : SaveDataType unavailable");
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

#if UNITY_EDITOR
        [MenuItem("SkyDragonHunter/SaveLoad/Save User Data #s")]
#endif
        public static bool SaveGameData()
        {
            if (!Application.isPlaying)
                return false;

            if (GameData == null)
            {
                // Debug.LogError($"Save Failed : GameData Null");
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

            Debug.Log($"[SaveLoadMgr]: User Data Save {path}");
            return true;
        }

#if UNITY_EDITOR
        [MenuItem("SkyDragonHunter/SaveLoad/Load User Data")]
#endif
        public static bool LoadGameData()
        {
            if (!Application.isPlaying)
                return false;

            if(loadedOnce)
            {
                // Debug.LogWarning($"[TEMP] Already Loaded Once, Cancel repeated loading");
                return false;
            }
            var path = Path.Combine(SaveDirectory , SaveFileName[0]);
            if(!File.Exists(path))
            {
                // Debug.LogError($"Load Failed : No Game Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var gameData = JsonConvert.DeserializeObject<GameSaveDataVC>(json, jsonSettings);

            while (gameData.MajorVersion < SaveDataMajorVersion)
            {
                gameData = gameData.VersionUp() as GameSaveDataVC;
            }
            GameData = gameData as GameSaveDataVC;

            // TODO: ccj -> 로드 중간에 세이브가 섞이지 않게 막음
            {
                GameData.IsLoadedDone = false;
                ApplySavedData();
                GameData.IsLoadedDone = true;
            }

            // Debug.LogWarning($"LoadGameData Completed");

            loadedOnce = true;

            Debug.Log($"[SaveLoadMgr]: User Data Load Done. {path}");
            return true;
        }

#if UNITY_EDITOR
        [ContextMenu("Remove User Data")]
        [MenuItem("SkyDragonHunter/SaveLoad/Remove User Data")]
#endif
        public static void RemoveSaveData()
        {
            string[] saveFilePaths = Directory.GetFiles(SaveDirectory, "*", SearchOption.TopDirectoryOnly);
            if (saveFilePaths == null || saveFilePaths.Length <= 0)
                return;

            foreach (string saveFilePath in saveFilePaths)
            {
                string fileName = Path.GetFileName(saveFilePath);
                if (fileName.StartsWith("SDH_SavedGameData"))
                {
                    Debug.Log($"[SaveLoadMgr]: Deleted Save File: {fileName}" );
                    File.Delete(saveFilePath);
                }
            }

            Debug.Log("[SaveLoadMgr]: Successed remove user uata");
        }

        public static bool LoadGameData(Scene scene)
        {
            // Debug.LogError($"using scene argument to Load Game Data Currently not supported");
            LoadGameData();

            return false;
        }

        public static bool SaveLocalSettings()
        {
            if (LocalSettingData == null)
            {
                // Debug.LogError($"Save Failed : Local Save Data Null");
                return false;
            }
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var path = Path.Combine(SaveDirectory, SaveFileName[1]);
            var json = JsonConvert.SerializeObject(LocalSettingData, jsonSettings);
            File.WriteAllText(path, json);

            // Debug.Log($"Local Setting Data Saved");

            return true;
        }

        public static bool LoadLocalSettings()
        {
            var path = Path.Combine(SaveDirectory, SaveFileName[1]);
            if (!File.Exists(path))
            {
                // Debug.LogWarning($"Load Failed : No Local Settings Data");
                return false;
            }

            var json = File.ReadAllText(path);
            var localSettingsData = JsonConvert.DeserializeObject<LocalSettingSaveDataVC>(json, jsonSettings);

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
            {
                // Debug.Log($"SaveLoadMgr Init failed : initialized once");
                return;
            }

            if (!LoadGameData())
            {
                InitializeGameData();                
            }

            if(!LoadLocalSettings())
            {
                InitializeLocalSettings();
            }

            SceneChangeMgr.beforeSceneUnloaded += CallSaveGameData;
            // Debug.LogWarning($"SaveLoadMgr Init completed");

            initialized = true;
        }

        // Private Methods
        public static void CallSaveGameData()
        {
            // Debug.LogError($"called SaveGameData");
            var tuto = GameMgr.FindObject<TutorialMgr>("TutorialMgr");
            if (tuto != null && !tuto.TutorialEnd)
                return;

            SaveGameData();
            SaveLocalSettings();
        }

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