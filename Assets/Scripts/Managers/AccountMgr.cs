using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.VersionControl;
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class AccountMgr
    {
        // �ʵ� (Fields)
        public static event Action onLevelUpEvents;

        private static Dictionary<string, GameObject> s_CollectedCrew;

        // �Ӽ� (Properties)
        public static int CurrentLevel { get; set; }
        public static CommonStats AccountStats { get; set; }
        public static CrystalData Crystal { get; set; }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            s_CollectedCrew = new Dictionary<string, GameObject>();
            AccountStats = new CommonStats();
            var crystalData = DataTableManager.CrystalLevelTable.First;
            InitAccountData(crystalData);
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats ���� ��");
            s_CollectedCrew = null;
            CurrentLevel = 0;
            AccountStats = null;
            Crystal = new CrystalData();
        }

        public static void LevelUp()
        {
            if (Crystal.nextLevelId == 0)
            {
                Debug.Log("Max Level!!");
                return;
            }

            // ũ����Ż ��� ����
            var crystalData = DataTableManager.CrystalLevelTable.Get(Crystal.nextLevelId);
            InitAccountData(crystalData);

            // �̺�Ʈ ȣ��
            onLevelUpEvents?.Invoke();
        }

        // Private �޼���
        private static void InitAccountData(CrystalLevelData data)
        {
            Crystal = new CrystalData
            {
                level = data.Level,
                needExp = data.NeedEXP,
                atkUp = data.AtkUP,
                hpUp = data.HPUP,
                nextLevelId = data.NextLvID
            };

            // ���� ���Ȱ� ����
            CurrentLevel = Crystal.level;
            AccountStats.SetMaxDamage(AccountStats.MaxDamage.Value + BigInteger.Parse(Crystal.atkUp));
            AccountStats.ResetDamage();
            AccountStats.SetMaxHealth(AccountStats.MaxHealth.Value + BigInteger.Parse(Crystal.atkUp));
            AccountStats.ResetHealth();
        }

        public static void RegisterCrew(GameObject crewInstance)
        {
            if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                if (!s_CollectedCrew.ContainsKey(provider.Name))
                {
                    s_CollectedCrew.Add(provider.Name, crewInstance);
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

        public static GameObject[] GetCrewInstanceList()
            => s_CollectedCrew.Values.ToArray();

        public static void AddUICrewListNode(GameObject crewInstance)
        {
            // Crew ����â�� �ڽ��� ���
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
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
            if (findPanelGo.TryGetComponent<UIAssignUnitTofortressPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewInstance);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node ��� ����");
            }
        }

        public static void LoadUserData()
        {
            Debug.Log("[AccountMgr]: ���� ������ �ε�");
            Debug.Log("[AccountMgr]: �ܿ� ������ �ε�");
            Debug.Log("[AccountMgr]: ���� ������ �ε� �Ϸ�");

            var tempUserData = GameMgr.FindObject("TempUserData");
            if (tempUserData.TryGetComponent<TempUserData>(out var comp))
            {
                foreach (var crew in comp.crewDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(crew);
                    instance.SetActive(false);
                    SyncCrewData(instance);
                    RegisterCrew(instance);
                }
            }
        }

        public static void SaveUserData()
        {
            Debug.Log("[AccountMgr]: ���� ������ ���̺�");
            Debug.Log("[AccountMgr]: �ܿ� ������ ���̺�");
            Debug.Log("[AccountMgr]: ���� ������ ���̺� �Ϸ�");
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

        // Others

    } // Scope by class GameMgr
} // namespace Root
