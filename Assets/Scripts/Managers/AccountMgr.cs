using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace SkyDragonHunter.Managers
{
    public static class AccountMgr
    {
        // 필드 (Fields)
        private static Dictionary<string, GameObject> s_CollectedCrews; // 인스턴스
        private static Dictionary<string, GameObject> s_CollectedCanons; // 인스턴스
        private static Dictionary<MasterySockeyType, List<UIMasterySocket>> s_CollectedSockets;

        // 속성 (Properties)
        public static int CurrentStageLevel { get; set; } = 1;
        public static int CurrentStageZoneLevel { get; set; } = 1;
        public static int CurrentLevel => Crystal.CurrentLevel;
        public static CommonStats AccountStats { get; private set; }
        public static Crystal Crystal { get; private set; }
        public static GameObject[] Canons => s_CollectedCanons?.Values.ToArray();
        public static Dictionary<MasterySockeyType, List<UIMasterySocket>> SocketMap => s_CollectedSockets;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        public static event Action onLevelUpEvents;
        public static event Action onSocketUpdateEvents; // 소켓 쪽과 의존성 엮여있음

        // Public 메서드
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            s_CollectedCrews = new Dictionary<string, GameObject>();
            AccountStats = new CommonStats();
            var crystalData = DataTableMgr.CrystalLevelTable.First;
            InitAccountData(crystalData);

            s_CollectedCanons = new Dictionary<string, GameObject>();
            s_CollectedSockets = new Dictionary<MasterySockeyType, List<UIMasterySocket>>();
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats 정리 중");
            s_CollectedCrews = null;
            AccountStats = null;
            Crystal = null;
            s_CollectedCanons = null;
            onLevelUpEvents = null;
            onSocketUpdateEvents = null;
        }

        public static void SetLevel(int id)
        {
            // 크리스탈 등급 증가
            var crystalData = DataTableMgr.CrystalLevelTable.Get(id);
            if (crystalData == null)
                return;

            InitAccountData(crystalData);

            // 이벤트 호출
            onLevelUpEvents?.Invoke();
        }

        public static void LevelUp()
        {
            if (Crystal.NextLevelId == 0)
            {
                Debug.Log("Max Level!!");
                return;
            }

            // 크리스탈 등급 증가
            var crystalData = DataTableMgr.CrystalLevelTable.Get(Crystal.NextLevelId);
            InitAccountData(crystalData);

            // 이벤트 호출
            onLevelUpEvents?.Invoke();
            SaveUserData();
        }

        public static void OnSocketLevelUp()
        {
            // AccountStatProvider의 MergedAccountStatsForCharacter를 호출함
            onSocketUpdateEvents.Invoke();
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
            Debug.Log("[AccountMgr]: 단원 데이터 로드");
            Debug.Log("[AccountMgr]: 유저 데이터 로드 완료");

            var tempUserData = GameMgr.FindObject("TempUserData");
            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                comp.LoadStaticData();

                // 크리스탈 레벨 로드
                SetLevel(comp.crystalLevelID);

                // 스테이지 초기화
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    CurrentStageLevel = comp.stageLevel;
                    CurrentStageZoneLevel = comp.stageZoneLevel;
                    waveController.Init();
                }
                

                foreach (var crew in comp.crewDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(crew);
                    SyncCrewData(instance);
                    RegisterCrew(instance);
                    instance.GetComponent<AccountStatProvider>().Init();
                    instance.SetActive(false);
                }
                foreach (var canon in comp.canonDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(canon);
                    SyncCanonData(instance);
                    RegisterCanon(instance);
                    instance.SetActive(false);
                }

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

            }
        }

        public static void SaveUserData()
        {
            Debug.Log("[AccountMgr]: 계정 데이터 세이브");
            Debug.Log("[AccountMgr]: 단원 데이터 세이브");
            Debug.Log("[AccountMgr]: 유저 데이터 세이브 완료");

            var tempUserData = GameMgr.FindObject("TempUserData");

            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                // 비공정 탑승 정보 저장
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

                // 크리스탈 레벨 저장
                comp.crystalLevelID = Crystal.CurrLevelId;

                // TODO: 스테이지 정보 저장 (임시)
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    comp.stageLevel = waveController.CurrentTriedMissionLevel;
                    comp.stageZoneLevel = waveController.CurrentTriedZonelLevel;
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
