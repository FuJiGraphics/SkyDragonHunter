using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkyDragonHunter.Managers
{
    public static class AccountMgr
    {
        // �ʵ� (Fields)
        public static event Action onLevelUpEvents;

        private static Dictionary<string, GameObject> s_CollectedCrews; // �ν��Ͻ�
        private static Dictionary<string, GameObject> s_CollectedCanons; // �ν��Ͻ�

        // �Ӽ� (Properties)
        public static int CurrentLevel => Crystal.CurrentLevel;
        public static CommonStats AccountStats { get; private set; }
        public static Crystal Crystal { get; private set; }
        public static GameObject[] Canons => s_CollectedCanons?.Values.ToArray();

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        public static void Init()
        {
            Debug.Log("AccountMgr Init");
            s_CollectedCrews = new Dictionary<string, GameObject>();
            AccountStats = new CommonStats();
            var crystalData = DataTableManager.CrystalLevelTable.First;
            InitAccountData(crystalData);

            s_CollectedCanons = new Dictionary<string, GameObject>();
        }

        public static void Release()
        {
            Debug.Log($"[AccountMgr] Account Stats ���� ��");
            s_CollectedCrews = null;
            AccountStats = null;
            Crystal = null;
            s_CollectedCanons = null;
        }

        public static void LevelUp()
        {
            if (Crystal.NextLevelId == 0)
            {
                Debug.Log("Max Level!!");
                return;
            }

            // ũ����Ż ��� ����
            var crystalData = DataTableManager.CrystalLevelTable.Get(Crystal.NextLevelId);
            InitAccountData(crystalData);

            // �̺�Ʈ ȣ��
            onLevelUpEvents?.Invoke();
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
                        if (findPanelGo.TryGetComponent<UICanonEquipmentPanel>(out var canonEquipPanel))
                        {
                            canonEquipPanel.AddCanonNode(canonInstance);
                        }
                        else
                        {
                            Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node ��� ����");
                        }
                    }

                    Debug.Log($"[AccountMgr]: Crew ���� ��� �Ϸ� {canonInstance.name}");
                }
                else
                {
                    Debug.LogWarning($"[AccountMgr]: �̹� ��ϵ� Crew�Դϴ�. {canonInstance.name}");
                }
            }
            else
            {
                Debug.LogWarning($"[AccountMgr]: ����Ϸ��� ������Ʈ�� Crew�� �ƴմϴ�. {canonInstance.name}");
            }
        }

        public static GameObject[] GetCrewInstanceList()
            => s_CollectedCrews.Values.ToArray();

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
                foreach (var canon in comp.canonDataPrefabs)
                {
                    GameObject instance = GameObject.Instantiate<GameObject>(canon);
                    instance.SetActive(false);
                    SyncCanonData(instance);
                    RegisterCanon(instance);
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
