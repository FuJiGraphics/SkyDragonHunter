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
        // 필드 (Fields)
        public static event Action onLevelUpEvents;

        private static Dictionary<string, GameObject> s_CollectedCrew;

        // 속성 (Properties)
        public static int CurrentLevel { get; set; }
        public static CommonStats AccountStats { get; set; }
        public static CrystalData Crystal { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
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
            Debug.Log($"[AccountMgr] Account Stats 정리 중");
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

            // 크리스탈 등급 증가
            var crystalData = DataTableManager.CrystalLevelTable.Get(Crystal.nextLevelId);
            InitAccountData(crystalData);

            // 이벤트 호출
            onLevelUpEvents?.Invoke();
        }

        // Private 메서드
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

            // 계정 스탯과 통합
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

        public static GameObject[] GetCrewInstanceList()
            => s_CollectedCrew.Values.ToArray();

        public static void AddUICrewListNode(GameObject crewInstance)
        {
            // Crew 정보창에 자신을 등록
            GameObject findPanelGo = GameMgr.FindObject("UICrewInfoPanel");
            if (findPanelGo.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
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
            if (findPanelGo.TryGetComponent<UIAssignUnitTofortressPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.AddCrewNode(crewInstance);
            }
            else
            {
                Debug.LogWarning("[CrewInfoProvider]: Crew Info Panel Node 등록 실패");
            }
        }

        public static void LoadUserData()
        {
            Debug.Log("[AccountMgr]: 계정 데이터 로드");
            Debug.Log("[AccountMgr]: 단원 데이터 로드");
            Debug.Log("[AccountMgr]: 유저 데이터 로드 완료");

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
            Debug.Log("[AccountMgr]: 계정 데이터 세이브");
            Debug.Log("[AccountMgr]: 단원 데이터 세이브");
            Debug.Log("[AccountMgr]: 유저 데이터 세이브 완료");
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

        // Others

    } // Scope by class GameMgr
} // namespace Root
