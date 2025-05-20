using NPOI.SS.Formula.Functions;
using SixLabors.ImageSharp;
using SkyDragonHunter.Database;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

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

        private static Dictionary<ArtifactGrade, List<ArtifactDummy>> s_HeldArtifacts = new();
        private static List<ArtifactDummy> s_SortedArtifacts = new();

        private static Dictionary<string, GameObject> s_CollectedCrews; // 인스턴스
        private static Dictionary<MasterySocketType, List<UIMasterySocket>> s_CollectedSockets;

        private static int s_MaxArtifactSlotCount = 3;
        private static List<ArtifactDummy> s_ArtifactStats = new();
        private static string m_NickName = "Default";
        private static readonly BigNum s_MaxAccountExp = new BigNum("134422977479810000000000000000000");
        private static BigNum s_AccountExp = 0;

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
        public static CommonStats ArtifactStats
        {
            get
            {
                CommonStats commonStats = new CommonStats();
                commonStats.ResetAllZero();
                foreach (var stat in s_ArtifactStats)
                {
                    if (stat != null)
                    {
                        commonStats += stat.CommonStatValue;
                    }
                }
                return commonStats;
            }
        }
        public static ArtifactDummy[] Artifacts => s_SortedArtifacts.ToArray();
        public static Crystal Crystal { get; private set; }
        public static bool IsMaxLevel => Crystal?.NextLevelId <= 0;
        public static BigNum NextExp => Crystal.NeedExp;
        public static BigNum CurrentExp 
        { 
            get
            {
                return s_AccountExp;
            }
            set
            {
                if (value <= s_MaxAccountExp)
                {
                    s_AccountExp = value;
                    onChangedExpEvent?.Invoke(value);
                }
            }
        }

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

        public static BigNum Food
        {
            get
            {
                if (!s_HeldItems.ContainsKey(ItemType.Food))
                    s_HeldItems.Add(ItemType.Food, 0);
                return s_HeldItems[ItemType.Food];
            }
            set
            {
                if (!s_HeldItems.ContainsKey(ItemType.Food))
                    s_HeldItems.Add(ItemType.Food, 0);
                s_HeldItems[ItemType.Food] = value;
                onItemCountChangedEvents?.Invoke(ItemType.Food);
            }
        }        

        public static CanonDummy[] HeldCanons => s_SortedCanons.ToArray();
        public static CanonDummy EquipCannonDummy { get; set; } = null;

        public static RepairDummy[] HeldRepairs => s_SortedRepairs.ToArray();
        public static RepairDummy EquipRepairDummy { get; set; } = null;

        // 외부 종속성 필드 (External dependencies field)
        private static ICrystalLevelUpHandler[] m_CrystalLevelUpHandlers;
        private static ISaveLoadHandler[] m_SaveLoadHandlers;
        private static UIInGameMainFramePanel s_InGameMainFramePanel;
        private static UITreasureEquipmentPanel s_TreasureEquipmentPanel;

        // 이벤트 (Events)
        private static event Action onLevelUpEvents;
        private static event Action<ItemType> onItemCountChangedEvents;
        private static event Action<string> onNicknameChangedEvents;
        private static event Action<BigNum> onChangedExpEvent;

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
            s_ArtifactStats = new();
            for (int i = 0; i < s_MaxArtifactSlotCount; ++i)
            {
                s_ArtifactStats.Add(null);
            }
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
            if (s_TreasureEquipmentPanel == null)
            {
                s_TreasureEquipmentPanel = GameMgr.FindObject<UITreasureEquipmentPanel>("TreasureEquipmentPanel");
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

        public static void AddCannonCount(CanonType type, CanonGrade grade, int count)
        {
            s_HeldCanons[type][grade].Count += count;
        }

        public static void AddRepairCount(RepairType type, RepairGrade grade, int count)
        {
            s_HeldRepairs[type][grade].Count += count;
        }
        
        public static void AddArtifact(ArtifactDummy dummy)
        {
            if (!s_HeldArtifacts.ContainsKey(dummy.Grade))
            {
                s_HeldArtifacts.Add(dummy.Grade, new());
            }
            s_HeldArtifacts[dummy.Grade].Add(dummy);
            s_SortedArtifacts.Add(dummy);

            // 보물 UI에 등록하기
            s_TreasureEquipmentPanel?.AddSlot(dummy);
        }

        public static int ArtifactCount => s_HeldArtifacts.Count;

        public static void RemoveArtifact(ArtifactDummy dummy, bool updateUI = true)
        {
            if (!s_HeldArtifacts.ContainsKey(dummy.Grade))
            {
                DrawableMgr.Dialog("Error", $"[AccountMgr]: 제거할 요소를 찾을 수 없습니다. {dummy}");
                return;
            }

            if (!s_HeldArtifacts[dummy.Grade].Contains(dummy))
            {
                DrawableMgr.Dialog("Error", $"[AccountMgr]: 제거할 요소를 찾을 수 없습니다. {dummy}");
                return;
            }

            s_SortedArtifacts.Remove(dummy);

            if (updateUI)
            {
                var equipPanel = GameMgr.FindObject<UITreasureEquipmentSlotPanel>("UITreasureEquipmentSlotPanel");
                if (equipPanel != null && equipPanel.IsArtifactEquipped(dummy))
                {
                    DrawableMgr.Dialog("Alert", $"장착 중인 보물은 제거할 수 없습니다. {dummy}");
                    return;
                }
                s_TreasureEquipmentPanel?.RemoveSlot(dummy);
                RemoveArtifactSlot(dummy);
            }
            s_HeldArtifacts[dummy.Grade].Remove(dummy);
        }

        public static bool TryGetArtifact(string uuid, out ArtifactDummy dst)
        {
            bool result = false;
            dst = null;
            foreach (var artifactList in s_HeldArtifacts)
            {
                foreach (var artifact in artifactList.Value)
                {
                    if (artifact.UUID == uuid)
                    {
                        result = true;
                        dst = artifact;
                        break;
                    }
                }
            }
            return result;
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

            BigNum needExp = 0;
            for (int i = 0; i < level; ++i)
            {
                prevLevelData = currLevelData;
                currLevelData = DataTableMgr.CrystalLevelTable.Get(currLevelData.NextLvID);
                if (currLevelData == null)
                {
                    break;
                }
                else
                {
                    needExp += new BigNum(currLevelData.NeedEXP);
                }
            }

            if (CurrentExp < needExp)
            {
                DrawableMgr.Dialog("Alert", "레벨업에 필요한 경험치가 부족합니다.");
                return;
            }
            CurrentExp -= needExp;

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

        public static void SetArtifactSlot(ArtifactDummy artifact, int slot)
        {
            if (slot < 0 || slot >= s_ArtifactStats.Count)
            {
                DrawableMgr.Dialog("Error", $"보물 등록 실패! slot 범위를 초과하였습니다. {slot}");
                return;
            }

            s_ArtifactStats[slot] = artifact;
            DirtyAccountAndAirshipStat();
        }

        public static void RemoveArtifactSlot(ArtifactDummy artifact)
        {
            if (artifact == null)
                return;

            for (int i = 0; i < s_ArtifactStats.Count; ++i)
            {
                if (s_ArtifactStats[i] == artifact)
                {
                    s_ArtifactStats[i] = null;
                    DirtyAccountAndAirshipStat();
                    break;
                }
            }
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
            if (inGameMainFramePanel != null)
            {
                inGameMainFramePanel.AtkText = (Crystal.IncreaseDamage + DefaultGrowthStats.MaxDamage).ToUnit();
                inGameMainFramePanel.HpText = (Crystal.IncreaseHealth + DefaultGrowthStats.MaxHealth).ToUnit();
            }
        }

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

        public static void RegisterCrew(GameObject crewInstance)
        {
            if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                if (!s_CollectedCrews.ContainsKey(provider.Title))
                {
                    s_CollectedCrews.Add(provider.Title, crewInstance);
                    AddUICrewListNode(crewInstance);
                    AddCrewUIAssignUnitToFortressPanel(crewInstance);
                    //Debug.Log($"[AccountMgr]: Crew 정보 등록 완료 {provider.Name}");
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

        public static void ClearResgisterCanons()
        {
            s_HeldCanons.Clear();
            s_SortedCanons.Clear();
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

        public static void ClearRegisterRepairs()
        {
            s_SortedRepairs.Clear();
            s_HeldRepairs.Clear();
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
                // Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
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

        public static void AddExpChangedEvent(Action<BigNum> callback)
        {
            onChangedExpEvent += callback;
        }

        public static void RemoveExpChangedEvent(Action<BigNum> callback)
        {
            onChangedExpEvent -= callback;
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

        private static void OnCanonLevelUpEvent(int level)
        {
            var airshipProvider = GameMgr.FindObject<AccountStatProvider>("Airship");
            airshipProvider.MergedAccountStatsForCharacter();
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
