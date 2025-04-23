using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class AccountMgr
    {
        // 필드 (Fields)
        private static Dictionary<string, GameObject> s_CollectedCrews; // 인스턴스
        private static Dictionary<string, GameObject> s_CollectedCanons; // 인스턴스
        private static Dictionary<MasterySockeyType, List<UIMasterySocket>> s_CollectedSockets;

        // 속성 (Properties)
        public static string Nickname { get; set; } = "Default";
        public static int CurrentStageLevel { get; set; } = 1;
        public static int CurrentStageZoneLevel { get; set; } = 1;
        public static int CurrentLevel => Crystal.CurrentLevel;
        public static CommonStats AccountStats { get; private set; }
        public static CommonStats DefaultGrowthStats { get; set; }
        public static Crystal Crystal { get; private set; }
        public static GameObject[] Canons => s_CollectedCanons?.Values.ToArray();
        public static Dictionary<MasterySockeyType, List<UIMasterySocket>> SocketMap => s_CollectedSockets;
        public static AlphaUnit Coin
        {
            get => ItemMgr.GetItem(ItemType.Coin).ItemCount;
            set
            {
                ItemMgr.GetItem(ItemType.Coin).ItemCount = value;
                s_InGameMainFramePanel.CoinText = ItemMgr.GetItem(ItemType.Coin).ItemCount.ToString();
            }
        }
        
        // 외부 종속성 필드 (External dependencies field)
        private static ICrystalLevelUpHandler[] m_CrystalLevelUpHandlers;
        private static ISaveLoadHandler[] m_SaveLoadHandlers;
        private static UIInGameMainFramePanel s_InGameMainFramePanel;

        // 이벤트 (Events)
        public static event Action onLevelUpEvents;

        // Public 메서드
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            s_CollectedCrews = new Dictionary<string, GameObject>();
            AccountStats = new CommonStats();
            var crystalData = DataTableMgr.CrystalLevelTable.First;
            InitAccountData(crystalData);

            DefaultGrowthStats = new CommonStats();
            DefaultGrowthStats.ResetAllZero();

            s_CollectedCanons = new Dictionary<string, GameObject>();
            s_CollectedSockets = new Dictionary<MasterySockeyType, List<UIMasterySocket>>();
        }

        public static void LateInit()
        {
            if (m_CrystalLevelUpHandlers == null)
            {
                m_CrystalLevelUpHandlers = GameMgr.FindObjects<ICrystalLevelUpHandler>();
            }
            if (m_SaveLoadHandlers == null)
            {
                m_SaveLoadHandlers = GameMgr.FindObjects<ISaveLoadHandler>();
            }
            if (s_InGameMainFramePanel == null)
            {
                s_InGameMainFramePanel = GameMgr.FindObject<UIInGameMainFramePanel>("InGameMainFramePanel");
            }
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats 정리 중");
            s_CollectedCrews = null;
            AccountStats = null;
            Crystal = null;
            s_CollectedCanons = null;
            onLevelUpEvents = null;
            m_CrystalLevelUpHandlers = null;
        }

        public static void LoadLevel(int id)
        {
            // 크리스탈 등급 증가
            var crystalData = DataTableMgr.CrystalLevelTable.Get(id);
            if (crystalData == null)
                return;

            InitAccountData(crystalData);

            // 이벤트 호출
            onLevelUpEvents?.Invoke();
        }

        public static void LevelUp(int level)
        {
            if (Crystal.NextLevelId <= 0)
                return;

            int currLevelId = Crystal.CurrLevelId;
            CrystalLevelData currLevelData = DataTableMgr.CrystalLevelTable.Get(currLevelId);
            CrystalLevelData prevLevelData = null;

            if (currLevelData == null)
            {
                Debug.LogError($"[AccountMgr]: LevelUp 실패. {currLevelId}");
                return;
            }

            for (int i = 0; i < level; ++i)
            {
                prevLevelData = currLevelData;
                currLevelData = DataTableMgr.CrystalLevelTable.Get(currLevelData.NextLvID);
                if (currLevelData == null)
                {
                    break;
                }
            }

            if (currLevelData != null)
            {
                InitAccountData(currLevelData);
            }
            else
            {
                InitAccountData(prevLevelData);
            }
            onLevelUpEvents?.Invoke();

            #region Account Info Panel UI에 적용
            var inGameMainFramePanel = GameMgr.FindObject<UIInGameMainFramePanel>("InGameMainFramePanel");
            inGameMainFramePanel.Nickname = AccountMgr.Nickname;
            if (currLevelData != null)
            {
                inGameMainFramePanel.Level = currLevelData.Level.ToString();
            }
            else
            {
                inGameMainFramePanel.Level = prevLevelData.Level.ToString();
            }
            inGameMainFramePanel.AtkText = AccountMgr.Crystal.IncreaseDamage.ToString();
            inGameMainFramePanel.HpText = AccountMgr.Crystal.IncreaseHealth.ToString();
            #endregion

            #region 레벨 업 핸들러 이벤트 호출
            foreach (var handler in m_CrystalLevelUpHandlers)
            {
                handler.OnCrystalLevelUp();
            }
            #endregion

            SaveUserData();
        }

        public static void OnSocketLevelUp()
        {
            // AccountStatProvider의 MergedAccountStatsForCharacter를 호출함
            onLevelUpEvents?.Invoke();
        }

        public static void DirtyAccountAndAirshipStat()
        {
            // AccountStatProvider의 MergedAccountStatsForCharacter를 호출함
            onLevelUpEvents?.Invoke();
            var inGameMainFramePanel = GameMgr.FindObject<UIInGameMainFramePanel>("InGameMainFramePanel");
            inGameMainFramePanel.AtkText = (Crystal.IncreaseDamage + DefaultGrowthStats.MaxDamage).ToString();
            inGameMainFramePanel.HpText = (Crystal.IncreaseHealth + DefaultGrowthStats.MaxHealth).ToString();
        }

        // Private 메서드
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new Crystal(data);

            // 계정 스탯과 통합
            AccountStats.SetMaxDamage((AccountStats.MaxDamage + Crystal.IncreaseDamage).Value);
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth((AccountStats.MaxHealth + Crystal.IncreaseHealth).Value);
            AccountStats.ResetHealth();
        }

        public static void RegisterCrew(GameObject crewInstance)
        {
            if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                if (!s_CollectedCrews.ContainsKey(provider.Name))
                {
                    s_CollectedCrews.Add(provider.Name, crewInstance);
                    AddUICrewListNode(crewInstance);
                    AddCrewUIAssignUnitToFortressPanel(crewInstance);
                    Debug.Log($"[AccountMgr]: Crew 정보 등록 완료 {provider.Name}");
                }
                else
                {
                    Debug.LogWarning($"[AccountMgr]: 이미 등록된 Crew입니다. {crewInstance.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: 등록하려는 오브젝트가 Crew가 아닙니다. {crewInstance.name}");
            }

        }

        public static void RegisterCanon(GameObject canonInstance)
        {
            if (canonInstance.TryGetComponent<CanonBase>(out var canonBase))
            {
                if (!s_CollectedCanons.ContainsKey(canonBase.name))
                {
                    s_CollectedCanons.Add(canonBase.name, canonInstance);

                    // TODO: 캐논 UI 세팅
                    if (canonInstance.TryGetComponent<CanonInfoProvider>(out var canonInfoProvider))
                    {
                        GameObject findPanelGo = GameMgr.FindObject("UICanonEquipmentPanel");
                        if (findPanelGo != null && 
                            findPanelGo.TryGetComponent<UICanonEquipmentPanel>(out var canonEquipPanel))
                        {
                            canonEquipPanel.AddCanonNode(canonInstance);
                        }
                        else
                        {
                            Debug.LogWarning("[CanonInfoProvider]: Canon Info Panel Node 등록 실패");
                        }
                    }

                    Debug.Log($"[AccountMgr]: Canon 정보 등록 완료 {canonInstance.name}");
                }
                else
                {
                    Debug.LogWarning($"[AccountMgr]: 이미 등록된 Canon입니다. {canonInstance.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: 등록하려는 오브젝트가 Canon가 아닙니다. {canonInstance.name}");
            }
        }

        public static void RegisterMasterySocket(UIMasterySocket masterySocketInstance)
        {
            if (masterySocketInstance != null)
            {
                if (!s_CollectedSockets.ContainsKey(masterySocketInstance.Type))
                {
                    s_CollectedSockets.Add(masterySocketInstance.Type, new List<UIMasterySocket>());
                }
                s_CollectedSockets[masterySocketInstance.Type].Add(masterySocketInstance);
                masterySocketInstance.onLevelupEvents += OnSocketLevelUp;
                Debug.Log($"[AccountMgr]: Socket 정보 등록 완료 {masterySocketInstance.ID}");
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: 등록하려는 오브젝트가 null입니다.");
            }
        }

        public static GameObject[] GetCrewInstanceList()
            => s_CollectedCrews.Values.ToArray();

        public static void AddUICrewListNode(GameObject crewInstance)
        {
            // Crew 정보창에 자신을 등록
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo != null
                && findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewInstance);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
            }
        }

        public static void AddCrewUIAssignUnitToFortressPanel(GameObject crewInstance)
        {
            GameObject findPanelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
            if (findPanelGo != null 
                && findPanelGo.TryGetComponent<UIAssignUnitTofortressPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewInstance);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
            }
        }

        public static void LoadUserData(string sceneName)
        {
            Debug.Log("[AccountMgr]: 계정 데이터 로드");
            var tempUserData = GameMgr.FindObject("TempUserData");
            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                comp.LoadStaticData();

                #region 계정 크리스탈 레벨 로드
                LoadLevel(comp.crystalLevelID);
                #endregion

                #region 스테이지 초기화 및 웨이브 컨트롤러 UI 적용
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    CurrentStageLevel = comp.stageLevel;
                    CurrentStageZoneLevel = comp.stageZoneLevel;
                    waveController.Init();
                }
                #endregion

                #region 단원 인스턴스화 및 저장
                foreach (var crew in comp.crewDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(crew);
                    SyncCrewData(instance);
                    RegisterCrew(instance);
                    instance.GetComponent<AccountStatProvider>().Init();
                    instance.SetActive(false);
                }
                #endregion

                #region 대포 인스턴스화 및 저장
                foreach (var canon in comp.canonDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(canon);
                    SyncCanonData(instance);
                    RegisterCanon(instance);
                    instance.SetActive(false);
                }
                #endregion

                #region 단원 탑승 정보 UI 적용
                foreach (var crewEquipStorage in comp.airshipEquipSlots)
                {
                    var equipment = GameMgr.FindObject<CrewEquipmentController>("Airship");
                    var panelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
                    UIAssignUnitTofortressPanel assignUnitTofortressPanel = panelGo?.GetComponent<UIAssignUnitTofortressPanel>();
                    if (crewEquipStorage.crewPrefab == null)
                        continue;

                    if (crewEquipStorage.crewPrefab.TryGetComponent<ICrewInfoProvider>(out var provider))
                    {
                        if (s_CollectedCrews.TryGetValue(provider.Name, out var instance))
                        {
                            equipment.EquipSlot(crewEquipStorage.slotIndex, instance);
                            assignUnitTofortressPanel?.EquipCrew(crewEquipStorage.slotIndex, instance);
                        }
                    }
                }
                #endregion

                #region 계정 정보 UI 적용
                AccountMgr.Nickname = comp.nickname;
                var inGameMainFramePanelGo = GameMgr.FindObject("InGameMainFramePanel");
                if (inGameMainFramePanelGo != null && 
                    inGameMainFramePanelGo.TryGetComponent<UIInGameMainFramePanel>(out var inGameMainFramePanel))
                {
                    inGameMainFramePanel.Nickname = comp.nickname;
                    inGameMainFramePanel.Level = AccountMgr.Crystal.CurrentLevel.ToString();
                    inGameMainFramePanel.AtkText = AccountMgr.Crystal.IncreaseDamage.ToString();
                    inGameMainFramePanel.HpText = AccountMgr.Crystal.IncreaseHealth.ToString();
                }
                #endregion

                #region 아이템 로드
                #endregion

                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnLoad(comp);
                }
            }
            Debug.Log("[AccountMgr]: 계정 데이터 로드 완료");
        }

        public static void SaveUserData()
        {
            Debug.Log("[AccountMgr]: 계정 데이터 세이브");
            Debug.Log("[AccountMgr]: 단원 데이터 세이브");
            Debug.Log("[AccountMgr]: 유저 데이터 세이브 완료");

            var tempUserData = GameMgr.FindObject("TempUserData");

            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                #region 비공정 탑승 정보 저장
                GameObject airshipInstance = GameMgr.FindObject("Airship");
                if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
                {
                    var equipSlots = equipController.EquipSlots;
                    comp.airshipEquipSlots = new List<SaveEquipStorage>();
                    for (int i = 0; i < equipSlots.Length; ++i)
                    {
                        int slotIndex = i;
                        GameObject crewPrefab = null;
                        // 크루 프리팹 찾기
                        foreach (var findGo in comp.crewDataPrefabs)
                        {
                            if (equipSlots[i] == null)
                                continue;

                            var equipCrewInfo = equipSlots[i].GetComponent<CrewInfoProvider>();
                            var findCrewInfo = findGo.GetComponent<CrewInfoProvider>();
                            if (equipCrewInfo.Name == findCrewInfo.Name)
                            {
                                crewPrefab = findGo;
                                break;
                            }
                        }
                        SaveEquipStorage saveData = new SaveEquipStorage();
                        saveData.slotIndex = slotIndex;
                        saveData.crewPrefab = crewPrefab;
                        comp.airshipEquipSlots.Add(saveData);
                    }
                }
                #endregion

                #region 크리스탈 레벨 저장
                comp.crystalLevelID = Crystal.CurrLevelId;
                #endregion

                #region 스테이지 정보 저장 (임시)
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    comp.stageLevel = waveController.CurrentTriedMissionLevel;
                    comp.stageZoneLevel = waveController.CurrentTriedZonelLevel;
                }
                #endregion

                #region 계정 정보 저장
                comp.nickname = AccountMgr.Nickname;
                #endregion

                #region 아이템 정보 저장
                #endregion

                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnSave(comp);
                }
                // 스테이지 정보 최신화
                comp.DirtyStaticData();
            }
        }

        private static void SyncCrewData(GameObject crewInstance)
        {
            if (crewInstance == null)
            {
                Debug.LogError("[AccountMgr]: crewInstance가 null입니다.");
            }

            // TODO: 크루 스탯 정보 등 서버와 동기화
            Debug.Log("[AccountMgr]: 단원 스탯 동기화중");
        }

        private static void SyncCanonData(GameObject canonInstance)
        {
            if (canonInstance == null)
            {
                Debug.LogError("[AccountMgr]: canonInstance가 null입니다.");
            }

            // TODO: 크루 스탯 정보 등 서버와 동기화
            Debug.Log("[AccountMgr]: 캐논 정보 동기화중");
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
