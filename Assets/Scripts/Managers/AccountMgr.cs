using NPOI.SS.Formula.Functions;
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
        // �ʵ� (Fields)
        private static Dictionary<ItemType, BigNum> s_HeldItems = new();

        private static Dictionary<CanonType, Dictionary<CanonGrade, CanonDummy>> s_HeldCanons = new();
        private static List<CanonDummy> s_SortedCanons = new();

        private static Dictionary<RepairType, Dictionary<RepairGrade, RepairDummy>> s_HeldRepairs = new();
        private static List<RepairDummy> s_SortedRepairs = new();

        private static Dictionary<ArtifactGrade, List<ArtifactDummy>> s_HeldArtifacts = new();
        private static List<ArtifactDummy> s_SortedArtifacts = new();

        private static Dictionary<string, GameObject> s_CollectedCrews; // �ν��Ͻ�
        private static Dictionary<MasterySocketType, List<UIMasterySocket>> s_CollectedSockets;

        private static int s_MaxArtifactSlotCount = 3;
        private static List<ArtifactDummy> s_ArtifactStats = new();
        private static string m_NickName = "Default";
        private static readonly BigNum s_MaxAccountExp = new BigNum("134422977479810000000000000000000");
        private static BigNum s_AccountExp = 0;

        // �Ӽ� (Properties)
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

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        private static ICrystalLevelUpHandler[] m_CrystalLevelUpHandlers;
        private static ISaveLoadHandler[] m_SaveLoadHandlers;
        private static UIInGameMainFramePanel s_InGameMainFramePanel;
        private static UITreasureEquipmentPanel s_TreasureEquipmentPanel;

        // �̺�Ʈ (Events)
        private static event Action onLevelUpEvents;
        private static event Action<ItemType> onItemCountChangedEvents;
        private static event Action<string> onNicknameChangedEvents;
        private static event Action<BigNum> onChangedExpEvent;

        // Public �޼���
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
            Debug.Log($"[AccountMgr] Account Stats ���� ��");
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
        
        public static void AddArtifact(ArtifactDummy dummy)
        {
            if (!s_HeldArtifacts.ContainsKey(dummy.Grade))
            {
                s_HeldArtifacts.Add(dummy.Grade, new());
            }
            s_HeldArtifacts[dummy.Grade].Add(dummy);
            s_SortedArtifacts.Add(dummy);

            // ���� UI�� ����ϱ�
            s_TreasureEquipmentPanel?.AddSlot(dummy);
        }

        public static void RemoveArtifact(ArtifactDummy dummy, bool updateUI = true)
        {
            if (!s_HeldArtifacts.ContainsKey(dummy.Grade))
            {
                DrawableMgr.Dialog("Error", $"[AccountMgr]: ������ ��Ҹ� ã�� �� �����ϴ�. {dummy}");
                return;
            }

            if (!s_HeldArtifacts[dummy.Grade].Contains(dummy))
            {
                DrawableMgr.Dialog("Error", $"[AccountMgr]: ������ ��Ҹ� ã�� �� �����ϴ�. {dummy}");
                return;
            }

            s_SortedArtifacts.Remove(dummy);

            if (updateUI)
            {
                var equipPanel = GameMgr.FindObject<UITreasureEquipmentSlotPanel>("UITreasureEquipmentSlotPanel");
                if (equipPanel != null && equipPanel.IsArtifactEquipped(dummy))
                {
                    DrawableMgr.Dialog("Alert", $"���� ���� ������ ������ �� �����ϴ�. {dummy}");
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
            // ũ����Ż ��� ����
            var crystalData = DataTableMgr.CrystalLevelTable.Get(id);
            if (crystalData == null)
                return;

            InitAccountData(crystalData);

            // �̺�Ʈ ȣ��
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
                Debug.LogError($"[AccountMgr]: LevelUp ����. {currLevelId}");
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
                DrawableMgr.Dialog("Alert", "�������� �ʿ��� ����ġ�� �����մϴ�.");
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

            #region Account Info Panel UI�� ����
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

            #region ���� �� �ڵ鷯 �̺�Ʈ ȣ��
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
                            // ���ݷ� ������ ������ ��� 
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
                DrawableMgr.Dialog("Error", $"���� ��� ����! slot ������ �ʰ��Ͽ����ϴ�. {slot}");
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
            // AccountStatProvider�� MergedAccountStatsForCharacter�� ȣ����
            onLevelUpEvents?.Invoke();
        }

        public static void DirtyAccountAndAirshipStat()
        {
            // AccountStatProvider�� MergedAccountStatsForCharacter�� ȣ����
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
                DrawableMgr.Dialog("Alert", "�ִ� ����Դϴ�.");
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
                DrawableMgr.Dialog("Alert", "�ִ� ����Դϴ�.");
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
                if (!s_CollectedCrews.ContainsKey(provider.Name))
                {
                    s_CollectedCrews.Add(provider.Name, crewInstance);
                    AddUICrewListNode(crewInstance);
                    AddCrewUIAssignUnitToFortressPanel(crewInstance);
                    //Debug.Log($"[AccountMgr]: Crew ���� ��� �Ϸ� {provider.Name}");
                }
                else
                {
                    Debug.LogWarning($"[AccountMgr]: �̹� ��ϵ� Crew�Դϴ�. {crewInstance.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: ����Ϸ��� ������Ʈ�� Crew�� �ƴմϴ�. {crewInstance.name}");
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
                Debug.Log($"[AccountMgr]: Socket ���� ��� �Ϸ� {masterySocketInstance.ID}");
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: ����Ϸ��� ������Ʈ�� null�Դϴ�.");
            }
        }

        public static GameObject[] GetCrewInstanceList()
            => s_CollectedCrews.Values.ToArray();

        public static void AddUICrewListNode(GameObject crewInstance)
        {
            // Crew ����â�� �ڽ��� ���
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo != null
                && findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewInstance);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node ��� ����");
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
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node ��� ����");
            }
        }

        public static void LoadUserData(string sceneName)
        {
            Debug.Log("[AccountMgr]: ���� ������ �ε�");
            var tempUserData = GameMgr.FindObject("TempUserData");
            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                comp.LoadStaticData();

                #region ���� ũ����Ż ���� �ε�
                LoadLevel(comp.crystalLevelID);
                // TODO: LJH LoadLevel TempUserData.s_CrystalLevelID �� ����� ���� ����
                #endregion

                #region �������� �ʱ�ȭ �� ���̺� ��Ʈ�ѷ� UI ����
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    // TODO: LJH
                    //CurrentStageLevel = comp.stageLevel;
                    //CurrentStageZoneLevel = comp.stageZoneLevel;
                    CurrentStageLevel = SaveLoadMgr.GameData.savedStageData.currentStage;
                    CurrentStageZoneLevel = SaveLoadMgr.GameData.savedStageData.currentZone;
                    waveController.Init();
                    // ~TODO
                }
                #endregion

                #region �ܿ� �ν��Ͻ�ȭ �� ����
                foreach (var crew in comp.crewDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(crew);

                    // TODO: LJH
                    if (instance != null)
                    {
                        var crewBT = instance.GetComponent<NewCrewControllerBT>();

                        if (SaveLoadMgr.GameData.savedCrewData.GetCrewLevel(crewBT.ID, out var level))
                        {
                            crewBT.SetDataFromTableWithExistingIDTemp(level);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Crew Prefab Null");
                    }
                    // ~TODO

                    SyncCrewData(instance);
                    RegisterCrew(instance);                    

                    // TODO: LJH
                    //instance.GetComponent<AccountStatProvider>().Init();
                    instance.GetComponent<CrewAccountStatProvider>().ApplyNewStatus();
                    // ~TODO
                    instance.SetActive(false);
                }
                #endregion

                #region ���� ���� �ҷ�����
                s_SortedCanons.Clear();
                s_HeldCanons.Clear();
                foreach (var canon in comp.canonDataPrefabs)
                {
                    // TODO: LJH
                    var savedCannon = SaveLoadMgr.GameData.savedCannonData.GetSavedCannon(canon.Grade, canon.Type);
                    if(savedCannon != null)
                    {
                        //canon.ID = savedCannon.id;
                        canon.Count = savedCannon.count;
                        canon.Level = savedCannon.level;
                        canon.IsUnlock = savedCannon.isUnlocked;
                        canon.IsEquip = savedCannon.isEquipped;
                    }
                    else
                    {
                        Debug.LogError($"Cannot find saved cannon with keys [{canon.Grade}/{canon.Type}]");
                    }
                    // ~TODO
                    RegisterCanon(canon);
                }
                #endregion

                #region ������ ���� �ҷ�����
                s_SortedRepairs.Clear();
                s_HeldRepairs.Clear();
                foreach (var repair in comp.repairDatas)
                {
                    // TODO: LJH
                    var savedCannon = SaveLoadMgr.GameData.savedRepairerData.GetSavedRepairer(repair.Grade, repair.Type);
                    if (savedCannon != null)
                    {
                        //repair.ID = savedCannon.id;
                        repair.Count = savedCannon.count;
                        repair.Level = savedCannon.level;
                        repair.IsUnlock = savedCannon.isUnlocked;
                        repair.IsEquip = savedCannon.isEquipped;
                    }
                    else
                    {
                        Debug.LogError($"Cannot find saved cannon with keys [{repair.Grade}/{repair.Type}]");
                    }
                    // ~TODO
                    RegisterRepair(repair);
                }
                #endregion

                #region �ܿ� ž�� ���� UI ����
                // TODO: LJH
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

                var equipController = GameMgr.FindObject<CrewEquipmentController>("Airship");
                var fortressPanelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
                var assignUnitToFortressPanel = fortressPanelGo?.GetComponent<UIAssignUnitTofortressPanel>();
                var mountedCrewIDs = SaveLoadMgr.GameData.savedAirshipData.mountedCrewIDs;

                for (int i = 0; i < mountedCrewIDs.Length; ++i)
                {
                    if (equipController == null)
                    {
                        Debug.LogError($"equipController or null");
                        break;
                    }

                    if (mountedCrewIDs[i] == 0)
                    {
                        continue;
                    }
                    foreach (var crew in s_CollectedCrews.Values)
                    {
                        if(!crew.TryGetComponent<NewCrewControllerBT>(out var crewBT))
                        {
                            Debug.LogError($"Cannot find CrewControllerBT from collected crew [{crew.name}]");
                            continue;
                        }
                        if(crewBT.ID == mountedCrewIDs[i])
                        {
                            equipController.EquipSlot(i, crew);
                            assignUnitToFortressPanel?.EquipCrew(i, crew);
                        }
                    }
                }
                // ~TODO
                #endregion

                #region ���� ���� UI ����
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

                #region ������ �ε�
                #endregion

                // ��Ƽ��Ʈ ���
                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnLoad(comp);
                }
            }
            Debug.Log("[AccountMgr]: ���� ������ �ε� �Ϸ�");
        }

        public static void SaveUserData()
        {
            Debug.Log("[AccountMgr]: ���� ������ ���̺�");
            var tempUserData = GameMgr.FindObject("TempUserData");

            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                #region ����� ž�� ���� ����
                GameObject airshipInstance = GameMgr.FindObject("Airship");
                if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
                {
                    var equipSlots = equipController.EquipSlots;
                    comp.airshipEquipSlots = new List<SaveEquipStorage>();
                    for (int i = 0; i < equipSlots.Length; ++i)
                    {
                        int slotIndex = i;
                        GameObject crewPrefab = null;
                        // ũ�� ������ ã��
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

                #region ũ����Ż ���� ����
                comp.crystalLevelID = Crystal.CurrLevelId;
                #endregion

                #region �������� ���� ���� (�ӽ�)
                //GameObject stageGo = GameMgr.FindObject("WaveController");
                //var waveController = stageGo?.GetComponent<TestWaveController>();
                //if (waveController != null)
                //{
                //    comp.stageLevel = waveController.CurrentTriedMissionLevel;
                //    comp.stageZoneLevel = waveController.CurrentTriedZonelLevel;
                //}
                #endregion

                #region ���� ���� ����
                comp.nickname = AccountMgr.Nickname;
                #endregion

                #region ������ ���� ����
                #endregion

                #region ���� ���� ����
                List<CanonDummy> newCanonSaveDummys = new List<CanonDummy>(HeldCanons);
                comp.canonDataPrefabs = newCanonSaveDummys.ToArray();
                #endregion

                #region ������ ���� ����
                List<RepairDummy> newRepairSaveDummys = new List<RepairDummy>(HeldRepairs);
                comp.repairDatas = newRepairSaveDummys.ToArray();
                #endregion

                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnSave(comp);
                }
                // �������� ���� �ֽ�ȭ
                comp.DirtyStaticData();
                Debug.Log("[AccountMgr]: ���� ������ ���̺� �Ϸ�");
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

        // Private �޼���
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new Crystal(data);

            // ���� ���Ȱ� ����
            AccountStats.SetMaxDamage(AccountStats.MaxDamage + Crystal.IncreaseDamage);
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth(AccountStats.MaxHealth + Crystal.IncreaseHealth);
            AccountStats.ResetHealth();
        }

        private static void SyncCrewData(GameObject crewInstance)
        {
            if (crewInstance == null)
            {
                Debug.LogError("[AccountMgr]: crewInstance�� null�Դϴ�.");
            }

            // TODO: ũ�� ���� ���� �� ������ ����ȭ
            //Debug.Log("[AccountMgr]: �ܿ� ���� ����ȭ��");
        }

        private static void OnCanonLevelUpEvent(int level)
        {
            var airshipProvider = GameMgr.FindObject<AccountStatProvider>("Airship");
            airshipProvider.MergedAccountStatsForCharacter();
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
