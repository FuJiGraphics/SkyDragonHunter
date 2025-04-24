using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Managers {

    public static class AccountMgr
    {
        // �ʵ� (Fields)
        private static Dictionary<ItemType, AlphaUnit> s_HeldItems;

        private static Dictionary<string, GameObject> s_CollectedCrews; // �ν��Ͻ�
        private static Dictionary<string, GameObject> s_CollectedCanons; // �ν��Ͻ�
        private static Dictionary<MasterySocketType, List<UIMasterySocket>> s_CollectedSockets;

        // �Ӽ� (Properties)
        public static string Nickname { get; set; } = "Default";
        public static int CurrentStageLevel { get; set; } = 1;
        public static int CurrentStageZoneLevel { get; set; } = 1;
        public static int CurrentLevel => Crystal.CurrentLevel;
        public static CommonStats AccountStats { get; private set; }
        public static CommonStats DefaultGrowthStats { get; set; }
        public static Crystal Crystal { get; private set; }

        public static AlphaUnit Coin
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
            }
        }

        public static AlphaUnit Diamond
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
            }
        }

        public static AlphaUnit Ticket
        {
            get
            {
                if (!s_HeldItems.ContainsKey(ItemType.Ticket))
                    s_HeldItems.Add(ItemType.Ticket, 0);
                return s_HeldItems[ItemType.Ticket];
            }
            set
            {
                if (!s_HeldItems.ContainsKey(ItemType.Ticket))
                    s_HeldItems.Add(ItemType.Ticket, 0);
                s_HeldItems[ItemType.Ticket] = value;
            }
        }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        private static ICrystalLevelUpHandler[] m_CrystalLevelUpHandlers;
        private static ISaveLoadHandler[] m_SaveLoadHandlers;
        private static UIInGameMainFramePanel s_InGameMainFramePanel;

        // �̺�Ʈ (Events)
        public static event Action onLevelUpEvents;

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

            s_CollectedCanons = new Dictionary<string, GameObject>();
            s_CollectedSockets = new Dictionary<MasterySocketType, List<UIMasterySocket>>();

            s_HeldItems = new Dictionary<ItemType, AlphaUnit>();
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
            Debug.Log($"[AccountMgr] Account Stats ���� ��");
            s_CollectedCrews = null;
            AccountStats = null;
            Crystal = null;
            s_CollectedCanons = null;
            onLevelUpEvents = null;
            m_CrystalLevelUpHandlers = null;
        }

        public static AlphaUnit ItemCount(ItemType type)
        {
            if (!s_HeldItems.ContainsKey(type))
            {
                s_HeldItems.Add(type, 0);
            }
            return s_HeldItems[type];
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
                            result.SetMaxDamage(socket.Stat);
                            break;
                        case MasterySocketType.Health:
                            result.SetMaxHealth(socket.Stat);
                            break;
                        case MasterySocketType.Armor:
                            result.SetMaxArmor(socket.Stat);
                            break;
                        case MasterySocketType.Resilient:
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
            var canons = s_CollectedCanons.Values.ToArray();
            CommonStats stats = new CommonStats();
            for (int i = 0; i < canons.Length; ++i)
            {
                if (canons[i].TryGetComponent<CanonBase>(out var canonBase))
                {
                    var data = canonBase.CanonData;
                    if (i == 0)
                    {
                        stats.SetMaxDamage(data.canHoldATK);
                        stats.SetMaxArmor(data.canHoldDEF);
                    }
                    else
                    {
                        stats.SetMaxDamage(stats.MaxDamage.Value + BigInteger.Parse(data.canHoldATK));
                        stats.SetMaxArmor(stats.MaxArmor.Value + BigInteger.Parse(data.canHoldDEF));
                    }
                }
            }
            return stats;
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
            inGameMainFramePanel.AtkText = (Crystal.IncreaseDamage + DefaultGrowthStats.MaxDamage).ToUnit();
            inGameMainFramePanel.HpText = (Crystal.IncreaseHealth + DefaultGrowthStats.MaxHealth).ToUnit();
        }

        // Private �޼���
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new Crystal(data);

            // ���� ���Ȱ� ����
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
                    Debug.Log($"[AccountMgr]: Crew ���� ��� �Ϸ� {provider.Name}");
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

        public static void RegisterCanon(GameObject canonInstance)
        {
            if (canonInstance.TryGetComponent<CanonBase>(out var canonBase))
            {
                if (!s_CollectedCanons.ContainsKey(canonBase.name))
                {
                    s_CollectedCanons.Add(canonBase.name, canonInstance);

                    // TODO: ĳ�� UI ����
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
                            Debug.LogWarning("[CanonInfoProvider]: Canon Info Panel Node ��� ����");
                        }
                    }

                    Debug.Log($"[AccountMgr]: Canon ���� ��� �Ϸ� {canonInstance.name}");
                }
                else
                {
                    Debug.LogWarning($"[AccountMgr]: �̹� ��ϵ� Canon�Դϴ�. {canonInstance.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: ����Ϸ��� ������Ʈ�� Canon�� �ƴմϴ�. {canonInstance.name}");
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
                #endregion

                #region �������� �ʱ�ȭ �� ���̺� ��Ʈ�ѷ� UI ����
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    CurrentStageLevel = comp.stageLevel;
                    CurrentStageZoneLevel = comp.stageZoneLevel;
                    waveController.Init();
                }
                #endregion

                #region �ܿ� �ν��Ͻ�ȭ �� ����
                foreach (var crew in comp.crewDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(crew);
                    SyncCrewData(instance);
                    RegisterCrew(instance);
                    instance.GetComponent<AccountStatProvider>().Init();
                    instance.SetActive(false);
                }
                #endregion

                #region ���� �ν��Ͻ�ȭ �� ����
                foreach (var canon in comp.canonDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(canon);
                    SyncCanonData(instance);
                    RegisterCanon(instance);
                    instance.SetActive(false);
                }
                #endregion

                #region �ܿ� ž�� ���� UI ����
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
            Debug.Log("[AccountMgr]: �ܿ� ������ ���̺�");
            Debug.Log("[AccountMgr]: ���� ������ ���̺� �Ϸ�");

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
                GameObject stageGo = GameMgr.FindObject("WaveController");
                var waveController = stageGo?.GetComponent<TestWaveController>();
                if (waveController != null)
                {
                    comp.stageLevel = waveController.CurrentTriedMissionLevel;
                    comp.stageZoneLevel = waveController.CurrentTriedZonelLevel;
                }
                #endregion

                #region ���� ���� ����
                comp.nickname = AccountMgr.Nickname;
                #endregion

                #region ������ ���� ����
                #endregion

                foreach (var handler in m_SaveLoadHandlers)
                {
                    handler.OnSave(comp);
                }
                // �������� ���� �ֽ�ȭ
                comp.DirtyStaticData();
            }
        }

        private static void SyncCrewData(GameObject crewInstance)
        {
            if (crewInstance == null)
            {
                Debug.LogError("[AccountMgr]: crewInstance�� null�Դϴ�.");
            }

            // TODO: ũ�� ���� ���� �� ������ ����ȭ
            Debug.Log("[AccountMgr]: �ܿ� ���� ����ȭ��");
        }

        private static void SyncCanonData(GameObject canonInstance)
        {
            if (canonInstance == null)
            {
                Debug.LogError("[AccountMgr]: canonInstance�� null�Դϴ�.");
            }

            // TODO: ũ�� ���� ���� �� ������ ����ȭ
            Debug.Log("[AccountMgr]: ĳ�� ���� ����ȭ��");
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
