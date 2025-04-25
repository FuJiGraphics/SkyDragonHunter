using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public struct SaveEquipStorage
    {
        public int slotIndex;
        public GameObject crewPrefab;
    }

    [System.Serializable]
    public struct SaveItemStorage
    {
        public int count;
        public ItemType itemType;
    }

    public class TempUserData : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Account Data")]
        public string nickname = "Default";
        public int crystalLevelID;

        [Header("Stage Data")]
        public int stageLevel = 1;
        public int stageZoneLevel = 1;

        [Header("Airship Data")]
        public GameObject[] crewDataPrefabs;
        [SerializeField] public CanonDummy[] canonDataPrefabs;
        [SerializeField] public List<SaveEquipStorage> airshipEquipSlots = new List<SaveEquipStorage>();

        [Header("Item Data")]
        [SerializeField] public List<SaveItemStorage> itemData;

        public static string s_Nickname = "default";
        public static int s_StageLevel = 1;
        public static int s_StageZoneLevel = 1;
        public static List<GameObject> s_CrewDataPrefabs;
        public static List<CanonDummy> s_CanonDataPrefabs;
        public static List<SaveEquipStorage> s_AirshipEquipSlots;
        public static List<SaveItemStorage> s_ItemData;
        public static int s_CrystalLevelID;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드

        public void DirtyStaticData()
        {
            s_CrewDataPrefabs = new List<GameObject>(crewDataPrefabs);
            s_CanonDataPrefabs = new List<CanonDummy>(canonDataPrefabs);
            s_AirshipEquipSlots = new List<SaveEquipStorage>(airshipEquipSlots);
            s_CrystalLevelID = crystalLevelID;
            s_StageLevel = stageLevel;
            s_StageZoneLevel = stageZoneLevel;
            s_ItemData = new List<SaveItemStorage>(itemData);
        }

        public void LoadStaticData()
        {
            if (s_CrewDataPrefabs != null)
                crewDataPrefabs = s_CrewDataPrefabs.ToArray();
            if (s_CanonDataPrefabs != null)
                canonDataPrefabs = s_CanonDataPrefabs.ToArray();
            if (s_AirshipEquipSlots != null)
                airshipEquipSlots = new List<SaveEquipStorage>(s_AirshipEquipSlots);
            crystalLevelID = s_CrystalLevelID;
            stageLevel = s_StageLevel;
            stageZoneLevel = s_StageZoneLevel;
            if (s_ItemData != null)
                itemData =  new List<SaveItemStorage>(s_ItemData);
        }

        // Private 메서드
        // Others

    } // Scope by class TempUserData
} // namespace SkyDragonHunter