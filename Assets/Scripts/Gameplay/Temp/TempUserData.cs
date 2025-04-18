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
        public GameObject[] canonDataPrefabs;
        [SerializeField] public List<SaveEquipStorage> airshipEquipSlots = new List<SaveEquipStorage>();

        public static string s_Nickname = "default";
        public static int s_StageLevel = 1;
        public static int s_StageZoneLevel = 1;
        public static List<GameObject> s_CrewDataPrefabs;
        public static List<GameObject> s_CanonDataPrefabs;
        public static List<SaveEquipStorage> s_AirshipEquipSlots;
        public static int s_CrystalLevelID;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드

        public void DirtyStaticData()
        {
            s_CrewDataPrefabs = new List<GameObject>(crewDataPrefabs);
            s_CanonDataPrefabs = new List<GameObject>(canonDataPrefabs);
            s_AirshipEquipSlots = new List<SaveEquipStorage>(airshipEquipSlots);
            s_CrystalLevelID = crystalLevelID;
            s_StageLevel = stageLevel;
            s_StageZoneLevel = stageZoneLevel;
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
        }

        // Private 메서드
        // Others

    } // Scope by class TempUserData
} // namespace SkyDragonHunter