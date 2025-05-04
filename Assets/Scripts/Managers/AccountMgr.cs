using SkyDragonHunter.Database;
using SkyDragonHunter.Entities;
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
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{    
    public static class AccountMgr
    {
        // 필드 (Fields)
        private static Dictionary<ItemType, BigNum> s_HeldItems = new();

        private static Dictionary<CanonType, Dictionary<CanonGrade, CanonDummy>> s_HeldCanons = new();
        private static List<CanonDummy> s_SortedCanons = new();

        private static Dictionary<RepairType, Dictionary<RepairGrade, RepairDummy>> s_HeldRepairs = new();
        private static List<RepairDummy> s_SortedRepairs = new();

        private static Dictionary<string, GameObject> s_CollectedCrews; // 인스턴스
        private static Dictionary<MasterySocketType, List<UIMasterySocket>> s_CollectedSockets;

        private static string m_NickName = "Default";

        // 속성 (Properties)
        public static string Nickname
        {
            get => m_NickName;
            set
            {
                m_NickName = value;
                onNicknameChangedEvents?.Invoke(value);
            }
        }
        public static int CurrentStageLevel { get; set; } = 1;
        public static int CurrentStageZoneLevel { get; set; } = 1;
        public static int CurrentLevel => Crystal.CurrentLevel;
        public static CommonStats AccountStats { get; private set; }
        public static CommonStats DefaultGrowthStats { get; set; }
        public static Crystal Crystal { get; private set; }
        public static bool IsMaxLevel => Crystal?.NextLevelId <= 0;
        public static BigNum Coin
        {
            get
            {
                if (!s_HeldItems.ContainsKey(ItemType.Coin))
                    s_HeldItems.Add(ItemType.Coin, 0);
                return s_HeldItems[ItemType.Coin];
            }
            set
            {
                if (!s_HeldItems.ContainsKey(ItemType.Coin))
                    s_HeldItems.Add(ItemType.Coin, 0);
                s_HeldItems[ItemType.Coin] = value;
                if (s_InGameMainFramePanel != null)
                {
                    s_InGameMainFramePanel.CoinText = s_HeldItems[ItemType.Coin].ToUnit();
                }
                onItemCountChangedEvents?.Invoke(ItemType.Coin);
            }
        }

        public static BigNum Diamond
        {
            get
            {
                if (!s_HeldItems.ContainsKey(ItemType.Diamond))
                    s_HeldItems.Add(ItemType.Diamond, 0);
                return s_HeldItems[ItemType.Diamond];
            }
            set
            {
                if (!s_HeldItems.ContainsKey(ItemType.Diamond))
                    s_HeldItems.Add(ItemType.Diamond, 0);
                s_HeldItems[ItemType.Diamond] = value;
                if (s_InGameMainFramePanel != null)
                {
                    s_InGameMainFramePanel.DiamondText = s_HeldItems[ItemType.Diamond].ToString();
                }
                onItemCountChangedEvents?.Invoke(ItemType.Diamond);
            }
        }

        public static BigNum WaveDungeonTicket
        {
            get
            {
                if (!s_HeldItems.ContainsKey(ItemType.WaveDungeonTicket))
                    s_HeldItems.Add(ItemType.WaveDungeonTicket, 0);
                return s_HeldItems[ItemType.WaveDungeonTicket];
            }
            set
            {
                if (!s_HeldItems.ContainsKey(ItemType.WaveDungeonTicket))
                    s_HeldItems.Add(ItemType.WaveDungeonTicket, 0);
                s_HeldItems[ItemType.WaveDungeonTicket] = value;
                onItemCountChangedEvents?.Invoke(ItemType.WaveDungeonTicket);
            }
        }

        public static CanonDummy[] HeldCanons => s_SortedCanons.ToArray();
        public static CanonDummy EquipCannonDummy { get; set; } = null;

        public static RepairDummy[] HeldRepairs => s_SortedRepairs.ToArray();
        public static RepairDummy EquipRepairDummy { get; set; } = null;

        public static bool CanonGradeUp(CanonDummy target)
        {
            if (target.Type == CanonType.Freeze && target.Grade == CanonGrade.Legend)
            {
                DrawableMgr.Dialog("Alert", "최대 등급입니다.");
                return false;
            }

            CanonType targetType = target.Type;
            CanonGrade targetGrade = target.Grade;
            if (targetType != CanonType.Freeze)
            {
                targetType++;
            }
            else
            {
                targetType = CanonType.Normal;
                if (targetGrade != CanonGrade.Legend)
                {
                    targetGrade++;
                }
            }

            target.Count -= 5;
            s_HeldCanons[targetType][targetGrade].Count++;

            return true;
        }

        public static bool RepairGradeUp(RepairDummy target)
        {
            if (target.Type == RepairType.Divine && target.Grade == RepairGrade.Legend)
            {
                DrawableMgr.Dialog("Alert", "최대 등급입니다.");
                return false;
            }

            RepairType targetType = target.Type;
            RepairGrade targetGrade = target.Grade;
            if (targetType != RepairType.Divine)
            {
                targetType++;
            }
            else
            {
                targetType = RepairType.Normal;
                if (targetGrade != RepairGrade.Legend)
                {
                    targetGrade++;
                }
            }

            target.Count -= 5;
            s_HeldRepairs[targetType][targetGrade].Count++;

            return true;
        }

        // 외부 종속성 필드 (External dependencies field)
        private static ICrystalLevelUpHandler[] m_CrystalLevelUpHandlers;
        private static ISaveLoadHandler[] m_SaveLoadHandlers;
        private static UIInGameMainFramePanel s_InGameMainFramePanel;

        // 이벤트 (Events)
        private static event Action onLevelUpEvents;
        private static event Action<ItemType> onItemCountChangedEvents;
        private static event Action<string> onNicknameChangedEvents;

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

            s_CollectedSockets = new Dictionary<MasterySocketType, List<UIMasterySocket>>();
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
            onLevelUpEvents = null;
            onItemCountChangedEvents = null;
            m_CrystalLevelUpHandlers = null;
        }

        public static BigNum ItemCount(ItemType type)
        {
            if (!s_HeldItems.ContainsKey(type))
            {
                s_HeldItems.Add(type, 0);
            }
            return s_HeldItems[type];
        }

        public static void AddItemCount(ItemType type, BigNum count)
        {
            if (!s_HeldItems.ContainsKey(type))
            {
                s_HeldItems.Add(type, 0);
            }
            s_HeldItems[type] += count;
            onItemCountChangedEvents?.Invoke(type);
        }

        public static void SetItemCount(ItemType type, BigNum count)
        {
            if (!s_HeldItems.ContainsKey(type))
            {
                s_HeldItems.Add(type, 0);
            }
            s_HeldItems[type] = count;
            onItemCountChangedEvents?.Invoke(type);
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
            inGameMainFramePanel.AtkText = AccountMgr.Crystal.IncreaseDamage.ToUnit();
            inGameMainFramePanel.HpText = AccountMgr.Crystal.IncreaseHealth.ToUnit();
            #endregion

            #region 레벨 업 핸들러 이벤트 호출
            foreach (var handler in m_CrystalLevelUpHandlers)
            {
                handler.OnCrystalLevelUp();
            }
            #endregion

            SaveUserData();
        }

        public static CommonStats GetSocketStat()
        {
            CommonStats result = new CommonStats();
            result.ResetAllZero();

            var socketMap = s_CollectedSockets;
            foreach (var socketList in socketMap)
            {
                foreach (var socket in socketList.Value)
                {
                    switch (socket.Type)
                    {
                        case MasterySocketType.Damage:
                            // 공격력 증가가 배율일 경우 
                            if (socket.Multiplier > 1 + float.Epsilon)
                                result.SetMaxDamage(result.MaxDamage * socket.Multiplier);
                            else
                                result.SetMaxDamage(socket.Stat);
                            break;
                        case MasterySocketType.Health:
                            if (socket.Multiplier > 1 + float.Epsilon)
                                result.SetMaxHealth(result.MaxHealth * socket.Multiplier);
                            else
                                result.SetMaxHealth(socket.Stat);
                            break;
                        case MasterySocketType.Armor:
                            if (socket.Multiplier > 1 + float.Epsilon)
                                result.SetMaxArmor(result.MaxArmor * socket.Multiplier);
                            else
                                result.SetMaxArmor(socket.Stat);
                            break;
                        case MasterySocketType.Resilient:
                            if (socket.Multiplier > 1 + float.Epsilon)
                                result.SetMaxResilient(result.MaxResilient * socket.Multiplier);
                            else
                                result.SetMaxResilient(socket.Stat);
                            break;
                        case MasterySocketType.CriticalMultiplier:
                            result.SetCriticalMultiplier((float)socket.Multiplier);
                            break;
                        case MasterySocketType.BossDamageMultiplier:
                            result.SetBossDamageMultiplier((float)socket.Multiplier);
                            break;
                        case MasterySocketType.SkillEffectMultiplier:
                            result.SetSkillEffectMultiplier((float)socket.Multiplier);
                            break;
                    }
                }
            }
            return result;
        }

        public static CommonStats GetCanonHoldStats()
        {
            CommonStats stats = new CommonStats();
            stats.ResetAllZero();

            foreach (var canonTypes in s_HeldCanons)
            {
                foreach (var canonGrade in canonTypes.Value)
                {
                    var canon = canonGrade.Value;
                    if (!canon.IsUnlock)
                    {
                        continue;
                    }

                    var canonInstance = canon.GetCanonInstance();
                    if (canonInstance.TryGetComponent<CanonBase>(out var canonBase))
                    {
                        var canonData = canonBase.CanonData;
                        BigNum newHoldATK = new BigNum(canonData.canHoldATK) + new BigNum(canonData.canHoldATKup) * canon.Level;
                        BigNum newHoldDEF = new BigNum(canonData.canHoldDEF) + new BigNum(canonData.canHoldDEFup) * canon.Level;
                        stats.SetMaxDamage(stats.MaxDamage + newHoldATK);
                        stats.SetMaxArmor(stats.MaxArmor + newHoldDEF);
                    }
                }
            }
            return stats;
        }

        public static CommonStats GetRepairHoldStats()
        {
            CommonStats stats = new CommonStats();
            stats.ResetAllZero();

            foreach (var repairTypes in s_HeldRepairs)
            {
                foreach (var repairGrade in repairTypes.Value)
                {
                    var repair = repairGrade.Value;
                    if (!repair.IsUnlock)
                    {
                        continue;
                    }

                    var repairData = repair.GetData();
                    BigNum newRepHoldHP = new BigNum(repairData.RepHoldHP) + new BigNum(repairData.RepHoldHPup) * repair.Level;
                    BigNum newRepHoldREC = new BigNum(repairData.RepHoldREC) + new BigNum(repairData.RepHoldRECup) * repair.Level;
                    stats.SetMaxHealth(stats.MaxHealth + newRepHoldHP);
                    stats.SetMaxResilient(stats.MaxResilient + newRepHoldREC);
                }
            }
            return stats;
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
            inGameMainFramePanel.AtkText = (Crystal.IncreaseDamage + DefaultGrowthStats.MaxDamage).ToUnit();
            inGameMainFramePanel.HpText = (Crystal.IncreaseHealth + DefaultGrowthStats.MaxHealth).ToUnit();
        }

        // Private 메서드
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new Crystal(data);

            // 계정 스탯과 통합
            AccountStats.SetMaxDamage(AccountStats.MaxDamage + Crystal.IncreaseDamage);
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth(AccountStats.MaxHealth + Crystal.IncreaseHealth);
            AccountStats.ResetHealth();
        }

        //public static void RegisterCrew(GameObject crewInstance)
        //{
        //    if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
        //    {
        //        if (!s_CollectedCrews.ContainsKey(provider.Name))
        //        {
        //            s_CollectedCrews.Add(provider.Name, crewInstance);
        //            AddUICrewListNode(crewInstance);
        //            AddCrewUIAssignUnitToFortressPanel(crewInstance);
        //            Debug.Log($"[AccountMgr]: Crew 정보 등록 완료 {provider.Name}");
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"[AccountMgr]: 이미 등록된 Crew입니다. {crewInstance.name}");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"[AccountMgr]: 등록하려는 오브젝트가 Crew가 아닙니다. {crewInstance.name}");
        //    }
        //
        //}

        public static void RegisterCrew(int crewId)
        {
            if (!SaveLoadMgr.GameData.savedCrewData.GetSavedCrew(crewId, out var savedCrew))
            {
                Debug.LogError($"[AccountMgr]: Cannot register crew, no crew found with id '{crewId}'");
                return;
            }
            AddUICrewListNode(crewId);
            AddCrewUIAssignUnitToFortressPanel(crewId);
        }

        public static void RegisterCanon(CanonDummy canonDummy)
        {
            if (!s_HeldCanons.ContainsKey(canonDummy.Type))
            {
                s_HeldCanons.Add(canonDummy.Type, new());
            }
            if (!s_HeldCanons[canonDummy.Type].ContainsKey(canonDummy.Grade))
            {
                s_HeldCanons[canonDummy.Type].Add(canonDummy.Grade, canonDummy);
            }
            s_HeldCanons[canonDummy.Type][canonDummy.Grade].Count = canonDummy.Count;
            s_SortedCanons.Add(canonDummy);
            canonDummy.AddLevelChangedEvent(OnCanonLevelUpEvent);
        }

        public static void RegisterRepair(RepairDummy repairDummy)
        {
            if (!s_HeldRepairs.ContainsKey(repairDummy.Type))
            {
                s_HeldRepairs.Add(repairDummy.Type, new());
            }
            if (!s_HeldRepairs[repairDummy.Type].ContainsKey(repairDummy.Grade))
            {
                s_HeldRepairs[repairDummy.Type].Add(repairDummy.Grade, repairDummy);
            }
            s_HeldRepairs[repairDummy.Type][repairDummy.Grade].Count = repairDummy.Count;
            s_SortedRepairs.Add(repairDummy);
            repairDummy.AddLevelChangedEvent(OnCanonLevelUpEvent);
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

        //public static void AddUICrewListNode(GameObject crewInstance)
        //{
        //    // Crew 정보창에 자신을 등록
        //    GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
        //    if (findPanelGo != null
        //        && findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
        //    {
        //        crewInfoPanel.AddCrewNode(crewInstance);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
        //    }
        //}

        public static void AddUICrewListNode(int crewId)
        {
            // Crew 정보창에 자신을 등록
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo != null
                && findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewId);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
            }
        }

        //public static void AddCrewUIAssignUnitToFortressPanel(GameObject crewInstance)
        //{
        //    GameObject findPanelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
        //    if (findPanelGo != null
        //        && findPanelGo.TryGetComponent<UIAssignUnitTofortressPanel>(out var crewInfoPanel))
        //    {
        //        crewInfoPanel.AddCrewNode(crewInstance);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
        //    }
        //}

        public static void AddCrewUIAssignUnitToFortressPanel(int crewId)
        {
            GameObject findPanelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
            if (findPanelGo != null
                && findPanelGo.TryGetComponent<UIAssignUnitTofortressPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewId);
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
                //foreach (var crew in comp.crewDataPrefabs)
                //{
                //    GameObject instance = GameObject.Instantiate<GameObject>(crew);
                //
                //    // TODO: LJH
                //    if (instance != null)
                //    {
                //        var crewBT = instance.GetComponent<NewCrewControllerBT>();
                //
                //        if (SaveLoadMgr.GameData.savedCrewData.GetCrewLevel(crewBT.ID, out var level))
                //        {
                //            crewBT.SetDataFromTableWithExistingIDTemp(level);
                //            Debug.LogWarning($"Crew prefab found and set : [{crewBT.name}] Lvl({level})");
                //        }
                //        else
                //        {
                //            // TODO :LJH (Need to handle logics on scene change)
                //            Debug.Log($"Crew Not Set To SaveData");
                //        }
                //    }
                //    else
                //    {
                //        Debug.LogError($"Crew Prefab Null");
                //    }
                //    // ~TODO
                //
                //    SyncCrewData(instance);
                //    RegisterCrew(instance);
                //
                //    instance.GetComponent<AccountStatProvider>().Init();       
                //    instance.SetActive(false);
                //}
                #endregion

                #region 대포 정보 불러오기
                s_SortedCanons.Clear();
                s_HeldCanons.Clear();
                foreach (var canon in comp.canonDataPrefabs)
                {
                    RegisterCanon(canon);
                }
                #endregion

                #region 수리공 정보 불러오기
                s_SortedRepairs.Clear();
                s_HeldRepairs.Clear();
                foreach (var repair in comp.repairDatas)
                {
                    RegisterRepair(repair);
                }
                #endregion

                #region 단원 탑승 정보 UI 적용
                //foreach (var crewEquipStorage in comp.airshipEquipSlots)
                //{
                //    var equipment = GameMgr.FindObject<CrewEquipmentController>("Airship");
                //    var panelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
                //    UIAssignUnitTofortressPanel assignUnitTofortressPanel = panelGo?.GetComponent<UIAssignUnitTofortressPanel>();
                //    if (crewEquipStorage.crewPrefab == null)
                //        continue;
                //
                //    if (crewEquipStorage.crewPrefab.TryGetComponent<ICrewInfoProvider>(out var provider))
                //    {
                //        if (s_CollectedCrews.TryGetValue(provider.Name, out var instance))
                //        {
                //            equipment.EquipSlot(crewEquipStorage.slotIndex, instance);
                //            assignUnitTofortressPanel?.EquipCrew(crewEquipStorage.slotIndex, instance);
                //        }
                //    }
                //}
                #endregion

                #region 계정 정보 UI 적용
                AccountMgr.Nickname = comp.nickname;
                var inGameMainFramePanelGo = GameMgr.FindObject("InGameMainFramePanel");
                if (inGameMainFramePanelGo != null && 
                    inGameMainFramePanelGo.TryGetComponent<UIInGameMainFramePanel>(out var inGameMainFramePanel))
                {
                    inGameMainFramePanel.Nickname = comp.nickname;
                    inGameMainFramePanel.Level = AccountMgr.Crystal.CurrentLevel.ToString();
                    inGameMainFramePanel.AtkText = AccountMgr.Crystal.IncreaseDamage.ToUnit();
                    inGameMainFramePanel.HpText = AccountMgr.Crystal.IncreaseHealth.ToUnit();
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
            Debug.Log("[AccountMgr]: 유저 데이터 세이브");
            var tempUserData = GameMgr.FindObject("TempUserData");
            
            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                #region 비공정 탑승 정보 저장
                //GameObject airshipInstance = GameMgr.FindObject("Airship");
                //if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
                //{
                //    var equipSlots = equipController.EquipSlots;
                //    comp.airshipEquipSlots = new List<SaveEquipStorage>();
                //    for (int i = 0; i < equipSlots.Length; ++i)
                //    {
                //        int slotIndex = i;
                //        GameObject crewPrefab = null;
                //        // 크루 프리팹 찾기
                //        foreach (var findGo in comp.crewDataPrefabs)
                //        {
                //            if (equipSlots[i] == null)
                //                continue;
                //            
                //            var equipCrewInfo = equipSlots[i].GetComponent<CrewInfoProvider>();
                //            var findCrewInfo = findGo.GetComponent<CrewInfoProvider>();
                //            if (equipCrewInfo.Name == findCrewInfo.Name)
                //            {
                //                crewPrefab = findGo;
                //                break;
                //            }
                //        }
                //
                //        SaveEquipStorage saveData = new SaveEquipStorage();
                //        saveData.slotIndex = slotIndex;
                //        saveData.crewPrefab = crewPrefab;
                //        comp.airshipEquipSlots.Add(saveData);
                //    }
                //}
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

                #region 대포 정보 저장
                List<CanonDummy> newCanonSaveDummys = new List<CanonDummy>(HeldCanons);
                comp.canonDataPrefabs = newCanonSaveDummys.ToArray();
                #endregion

                #region 수리공 정보 저장
                List<RepairDummy> newRepairSaveDummys = new List<RepairDummy>(HeldRepairs);
                comp.repairDatas = newRepairSaveDummys.ToArray();
                #endregion

                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnSave(comp);
                }
                // 스테이지 정보 최신화
                comp.DirtyStaticData();
                Debug.Log("[AccountMgr]: 유저 데이터 세이브 완료");
            }
        }

        public static void AddLevelUpEvent(Action callback)
        {
            onLevelUpEvents += callback;
        }

        public static void RemoveLevelUpEvent(Action callback)
        {
            onLevelUpEvents -= callback;
        }

        public static void AddItemCountChangedEvent(Action<ItemType> callback)
        {
            onItemCountChangedEvents += callback;
        }

        public static void RemoveItemCountChangedEvent(Action<ItemType> callback)
        {
            onItemCountChangedEvents -= callback;
        }

        public static void AddNicknameChangedEvent(Action<string> callback)
        {
            onNicknameChangedEvents += callback;
        }

        public static void RemoveNicknameChangedEvent(Action<string> callback)
        {
            onNicknameChangedEvents -= callback;
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

        private static void OnCanonLevelUpEvent(int level)
        {
            var airshipProvider = GameMgr.FindObject<AccountStatProvider>("Airship");
            airshipProvider.MergedAccountStatsForCharacter();
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
