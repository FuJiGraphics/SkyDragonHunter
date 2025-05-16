using SkyDragonHunter.Database;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Temp;
using SkyDragonHunter.Test;
using SkyDraonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.test
{
    public enum PickUpType
    {
        Crew,
        Cannon,
        Repairer
    }

    public class TestRandomPick : MonoBehaviour
    {
        // 필드 (Fields)
        private UiMgr uiMgr;
        public GameObject pickUpInfoPrefab;
        public Transform crewAndMoonStone;
        private int[] crewIds;
        private PickUpType currentPickUpType;

        [SerializeField] private Button crewOnePickButton;
        [SerializeField] private Button crewTenPickButton;
        [SerializeField] private Button cannonOnePickButton;
        [SerializeField] private Button cannonTenPickButton;
        [SerializeField] private Button repairOnePickButton;
        [SerializeField] private Button repairTenPickButton;

        [SerializeField] private Button onePickButton;
        [SerializeField] private Button tenPickButton;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            if(uiMgr == null)
                uiMgr = GameMgr.FindObject("UiMgr").GetComponent<UiMgr>();

            if (crewAndMoonStone == null)
            {
                // "CrewAndMoonStone" 오브젝트를 계층 구조에서 찾아 할당
                crewAndMoonStone = GameObject
                    .Find("InGameUI/Canvas/SafeAreaPanel/SummonPanel/RandomPickCrewInfo/CrewAndMoonStone")
                    ?.transform;

                // 못 찾은 경우 경고 로그 출력
                if (crewAndMoonStone == null)
                {
                    Debug.LogWarning("CrewAndMoonStone을 찾을 수 없습니다. 경로를 확인하세요.");
                }
            }

            crewIds = new int[DataTableMgr.CrewTable.Keys.Count];
            //Debug.LogError($"Crew Ids pool created with Length [{crewIds.Length}]");
            int index = 0;
            foreach(var crewId in DataTableMgr.CrewTable.Keys)
            {
                crewIds[index++] = crewId;
            }

            currentPickUpType = PickUpType.Crew;
            AddListeners();
        }

        // Public 메서드
        public void ApplyPickCrew(int crewId)
        {
            var savedCrew = SaveLoadMgr.GameData.savedCrewData.GetCrewData(crewId);
            ApplyPickCrew(savedCrew);
        }

        public void ApplyPickCrew(SavedCrew crew)
        {
            GameObject prefab = crew.crewData.GetPrefab();
            if (prefab == null)
            {
                Debug.LogWarning($"단원 삽입 실패 : {crew.crewData.UnitName}");
                return;
            }

            var instance = GameObject.Instantiate(prefab);
            if (instance != null)
            {
                if (instance.TryGetComponent<NewCrewControllerBT>(out var btComp))
                {
                    btComp.SetDataFromTableWithExistingIDTemp(crew.level);
                }                
                crew.isUnlocked = true;
                AccountMgr.RegisterCrew(instance);
                instance.GetComponent<CrewAccountStatProvider>().ApplyNewStatus();
                instance.SetActive(false);
            }
            if (!TempCrewLevelExpContainer.TryGetTempCrewData(crew.crewData.ID, out var tempCrewData))
            {
                Debug.LogError($"No Temp Crew Data Found with key [{crew.crewData.ID}]");
                
            }
            tempCrewData.IsUnlocked = true;
        }

        public void RandomPick()
        {
            if (AccountMgr.ItemCount(ItemType.CrewTicket) < 1)
            {
                DrawableMgr.Dialog($"안내", $"단원 소환 티켓이 부족합니다");
                return;
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            int selectedCrew = crewIds[Random.Range(0, crewIds.Length - 1)];

            // 숫자 드롭 생성
            if(SaveLoadMgr.GameData.savedCrewData.GetCrewData(selectedCrew).isUnlocked)
            {
                AccountMgr.AddItemCount(ItemType.MasteryLevelUp, 1);
                CreateMateryResourcePickInfo(1);
            }
            else
            {
                Debug.Log($"Random Pick selected Crew [ID] [{selectedCrew}]");
                ApplyPickCrew(selectedCrew);
                SaveLoadMgr.GameData.savedCrewData.GetCrewData(selectedCrew).isUnlocked = true;
                CreatePickInfo(selectedCrew, 1);
                SaveLoadMgr.GameData.savedShopItemData.crewPickUpExp += 1;
            }
            var crewTicketCount = AccountMgr.ItemCount(ItemType.CrewTicket);
            crewTicketCount -= 1;
            AccountMgr.SetItemCount(ItemType.CrewTicket, crewTicketCount);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Crew;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        private void GiveFirstPickReward()
        {
            SaveLoadMgr.GameData.savedShopItemData.isFirstPickGiven = true;

            int selectedCrew = 14101;
            Debug.Log($"Random Pick selected Crew [ID] [{selectedCrew}]");
            ApplyPickCrew(selectedCrew);
            SaveLoadMgr.GameData.savedCrewData.GetCrewData(selectedCrew).isUnlocked = true;
            CreatePickInfo(selectedCrew, 1);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Crew;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        public void RandomTenPick()
        {
            if(!SaveLoadMgr.GameData.savedShopItemData.isFirstPickGiven)
            {
                GiveFirstPickReward();
                return;                
            }

            if (AccountMgr.ItemCount(ItemType.CrewTicket) < 10)
            {
                DrawableMgr.Dialog($"안내", $"단원 소환 티켓이 부족합니다");
                return;
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 각 숫자의 등장 횟수를 기록할 Dictionary
            Dictionary<int, int> pickUpCounts = new();
            int existingCount = 0;

            // 10번 랜덤 추출 (중복 허용)
            for (int i = 0; i < 10; i++)
            {
                var randIndex = Random.Range(0, crewIds.Length);
                int selected = crewIds[randIndex];
                Debug.Log($"Random Pick selected Crew [index/ID] [{selected}/{randIndex}]");

                // 이미 등장한 숫자면 +1, 아니면 1로 시작
                if (pickUpCounts.ContainsKey(selected))
                {
                    existingCount++;
                    Debug.Log($"key [{selected}] exists already, increasing existingCount : {existingCount}");
                }
                else
                {
                    pickUpCounts.Add(selected, 1);
                }
            }

            // 각 숫자마다 프리팹 생성
            foreach (var kvp in pickUpCounts)
            {
                // kvp.Key: 숫자, kvp.Value: 횟수
                var crewData = SaveLoadMgr.GameData.savedCrewData.GetCrewData(kvp.Key);
                if (crewData.isUnlocked)
                {
                    existingCount++;
                    Debug.Log($"Crew Id [{kvp.Key}] unlocked already, increasing existingCount : {existingCount}");
                }
                else
                {
                    ApplyPickCrew(crewData);
                    crewData.isUnlocked = true;
                    CreatePickInfo(DataTableMgr.CrewTable.Get(kvp.Key).UnitName, 1);
                    SaveLoadMgr.GameData.savedShopItemData.crewPickUpExp += 1;
                }
            }

            if (existingCount > 0)
            {
                CreateMateryResourcePickInfo(existingCount);                
                AccountMgr.AddItemCount(ItemType.MasteryLevelUp, existingCount);
            }
            var crewTicketCount = AccountMgr.ItemCount(ItemType.CrewTicket);
            crewTicketCount -= 10;
            AccountMgr.SetItemCount(ItemType.CrewTicket, crewTicketCount);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Crew;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        // Private 메서드
        private void CreatePickInfo(string crewName, int count)
        {
            // 프리팹 생성 및 부모 오브젝트 지정
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);

            // 프리팹에 붙어 있는 PickUpInfoUI 스크립트 가져오기
            var ui = instance.GetComponent<PickUpInfoUI>();

            // 스크립트가 존재하면 이름/수량 적용
            if (ui != null)
            {
                ui.SetData(crewName, count);
            }
        }

        private void CreatePickInfo(int crewId, int count)
        {
            // 프리팹 생성 및 부모 오브젝트 지정
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);

            // 프리팹에 붙어 있는 PickUpInfoUI 스크립트 가져오기
            var ui = instance.GetComponent<PickUpInfoUI>();

            // 스크립트가 존재하면 이름/수량 적용
            if (ui != null)
            {
                ui.SetDataWithCrewID(crewId, count);
            }
        }

        private void CreatePickInfo(CanonGrade grade, CanonType type, int count = 0)
        {
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);
            var ui = instance.GetComponent<PickUpInfoUI>();

            if (ui != null)
            {
                ui.SetDataWithCannonId(grade, type, count);
            }
        }
        private void CreatePickInfo(RepairGrade grade, RepairType type, int count = 0)
        {
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);
            var ui = instance.GetComponent<PickUpInfoUI>();

            if (ui != null)
            {
                ui.SetDataWithRepairId(grade, type, count);
            }
        }

        private void CreateMateryResourcePickInfo(int count)
        {
            // 프리팹 생성 및 부모 오브젝트 지정
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);

            // 프리팹에 붙어 있는 PickUpInfoUI 스크립트 가져오기
            var ui = instance.GetComponent<PickUpInfoUI>();

            if (ui != null)
            {
                ui.SetDataForMasteryResource(count);
            }
        }
        // Others

        private void AddListeners()
        {
            crewOnePickButton.onClick.AddListener(RandomPick);
            crewTenPickButton.onClick.AddListener(RandomTenPick);
            cannonOnePickButton.onClick.AddListener(OnClickCannonOnePick);
            cannonTenPickButton.onClick.AddListener(OnClickCannonTenPick);
            repairOnePickButton.onClick.AddListener(OnClickReapirOnePick);
            repairTenPickButton.onClick.AddListener(OnClickReapirTenPick);
            onePickButton.onClick.AddListener(OnClickPickUpOne);
            tenPickButton.onClick.AddListener(OnClickPickUpTen);
        }

        private void OnClickCannonOnePick()
        {
            //if (AccountMgr.ItemCount(ItemType.CanonTicket) < 1)
            //{
            //    DrawableMgr.Dialog($"안내", $"캐논 소환 티켓이 부족합니다");
            //    return;
            //}
            
            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var cannonAffinLv = SaveLoadMgr.GameData.savedShopItemData.cannonAffinityLevel;
            var currentAffinId = DataTableMgr.AffinityLevelTable.GetAffinityLevelID(AffinityLevelType.Cannon, cannonAffinLv);
            var pickUpDatas = DataTableMgr.CannonPickUpTable.GetCannonPickUpDatasWithAffinID(currentAffinId);
            (CannonPickUpData data, float weight)[] tuples = new (CannonPickUpData data, float weight)[pickUpDatas.Count];
            for(int i = 0; i < pickUpDatas.Count; ++i)
            {
                tuples[i].data = pickUpDatas[i];
                tuples[i].weight = pickUpDatas[i].DrawRate * 0.01f;
            }
            var randPicked = RandomMgr.RandomWithWeights<CannonPickUpData>(tuples);

            var allCannons = CanonTable.GetAllCanonDummyTypes();
            CanonDummy selected = new();
            foreach(var cannon in allCannons)
            {
                if (cannon.ID == randPicked.CannonID)
                { 
                    selected = cannon;
                    break;
                }
            }

            var saved = SaveLoadMgr.GameData.savedCannonData.GetSavedCannon(selected.Grade, selected.Type);
            //selected.Count += 1;
            saved.count += 1;
            saved.isUnlocked = true;

            CreatePickInfo(selected.Grade, selected.Type);

            var cannonTicketCount = AccountMgr.ItemCount(ItemType.CanonTicket);
            cannonTicketCount -= 1;
            AccountMgr.SetItemCount(ItemType.CanonTicket, cannonTicketCount);
            if(SaveLoadMgr.GameData.savedShopItemData.AddAffinityExp(AffinityLevelType.Cannon, 1) != 0)
            {

            }
            SaveLoadMgr.ApplySavedData(SaveDataTypes.Cannon);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Cannon;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        private void OnClickCannonTenPick()
        {
            //if (AccountMgr.ItemCount(ItemType.CrewTicket) < 10)
            //{
            //    DrawableMgr.Dialog($"안내", $"캐논 소환 티켓이 부족합니다");
            //    return;
            //}
            
            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 각 숫자의 등장 횟수를 기록할 Dictionary
            Dictionary<CanonDummy, int> pickUpCounts = new();
            var allCannons = CanonTable.GetAllCanonDummyTypes();

            var cannonAffinLv = SaveLoadMgr.GameData.savedShopItemData.cannonAffinityLevel;
            var currentAffinId = DataTableMgr.AffinityLevelTable.GetAffinityLevelID(AffinityLevelType.Cannon, cannonAffinLv);
            var pickUpDatas = DataTableMgr.CannonPickUpTable.GetCannonPickUpDatasWithAffinID(currentAffinId);
            (CannonPickUpData data, float weight)[] tuples = new (CannonPickUpData data, float weight)[pickUpDatas.Count];
            for (int i = 0; i < pickUpDatas.Count; ++i)
            {
                tuples[i].data = pickUpDatas[i];
                tuples[i].weight = pickUpDatas[i].DrawRate * 0.01f;
            }

            // 10번 랜덤 추출 (중복 허용)
            for (int i = 0; i < 10; i++)
            {
                var randPicked = RandomMgr.RandomWithWeights<CannonPickUpData>(tuples);
                CanonDummy selected = new();
                foreach (var cannon in allCannons)
                {
                    if (cannon.ID == randPicked.CannonID)
                    {
                        selected = cannon;
                        break;
                    }
                }

                // 이미 등장한 숫자면 +1, 아니면 1로 시작
                if (pickUpCounts.ContainsKey(selected))
                {
                    pickUpCounts[selected]++;                    
                }
                else
                {
                    pickUpCounts.Add(selected, 1);
                }
            }

            // 각 숫자마다 프리팹 생성
            foreach (var kvp in pickUpCounts)
            {
                // kvp.Key: 숫자, kvp.Value: 횟수
                var cannonDummy = kvp.Key;
                var savedCannon = SaveLoadMgr.GameData.savedCannonData.GetSavedCannon(cannonDummy.Grade, cannonDummy.Type);
                //cannonDummy.Count += 1;
                savedCannon.count += 1;
                savedCannon.isUnlocked = true;

                CreatePickInfo(cannonDummy.Grade, cannonDummy.Type, kvp.Value);
            }

            var cannonTicketCount = AccountMgr.ItemCount(ItemType.CanonTicket);
            cannonTicketCount -= 10;
            AccountMgr.SetItemCount(ItemType.CanonTicket, cannonTicketCount);
            if (SaveLoadMgr.GameData.savedShopItemData.AddAffinityExp(AffinityLevelType.Cannon, 10) != 0)
            { 

            }
            SaveLoadMgr.ApplySavedData(SaveDataTypes.Cannon);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Cannon;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        private void OnClickReapirOnePick()
        {
            //if (AccountMgr.ItemCount(ItemType.RepairTicket) < 1)
            //{
            //    DrawableMgr.Dialog($"안내", $"수리공 소환 티켓이 부족합니다");
            //    return;
            //}
            
            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var repairAffinLv = SaveLoadMgr.GameData.savedShopItemData.repairAffinityLevel;
            var currentAffinId = DataTableMgr.AffinityLevelTable.GetAffinityLevelID(AffinityLevelType.Repair, repairAffinLv);
            var pickUpDatas = DataTableMgr.RepairPickUpTable.GetRepairPickUpDatasWithAffinID(currentAffinId);
            (RepairPickUpData data, float weight)[] tuples = new (RepairPickUpData data, float weight)[pickUpDatas.Count];
            for (int i = 0; i < pickUpDatas.Count; ++i)
            {
                tuples[i].data = pickUpDatas[i];
                tuples[i].weight = pickUpDatas[i].RepDrawRate * 0.01f;
            }
            var randPicked = RandomMgr.RandomWithWeights<RepairPickUpData>(tuples);

            var allReps = RepairTableTemplate.GetAllRepairDummyTypes();
            RepairDummy selected = new();
            foreach (var rep in allReps)
            {
                if (rep.ID == randPicked.ItemID)
                {
                    selected = rep;
                    break;
                }
            }

            var saved = SaveLoadMgr.GameData.savedRepairerData.GetSavedRepairer(selected.Grade, selected.Type);
            //selected.Count += 1;
            saved.count += 1;
            saved.isUnlocked = true;

            CreatePickInfo(selected.Grade, selected.Type);

            var cannonTicketCount = AccountMgr.ItemCount(ItemType.RepairTicket);
            cannonTicketCount -= 1;
            AccountMgr.SetItemCount(ItemType.RepairTicket, cannonTicketCount);
            if(SaveLoadMgr.GameData.savedShopItemData.AddAffinityExp(AffinityLevelType.Repair, 1) != 0)
            {

            }
            SaveLoadMgr.ApplySavedData(SaveDataTypes.Repairer);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Repairer;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        private void OnClickReapirTenPick()
        {
            //if (AccountMgr.ItemCount(ItemType.RepairTicket) < 10)
            //{
            //    DrawableMgr.Dialog($"안내", $"수리공 소환 티켓이 부족합니다");
            //    return;
            //}

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 각 숫자의 등장 횟수를 기록할 Dictionary
            var repairAffinLv = SaveLoadMgr.GameData.savedShopItemData.repairAffinityLevel;
            var currentAffinId = DataTableMgr.AffinityLevelTable.GetAffinityLevelID(AffinityLevelType.Repair, repairAffinLv);
            var pickUpDatas = DataTableMgr.RepairPickUpTable.GetRepairPickUpDatasWithAffinID(currentAffinId);
            (RepairPickUpData data, float weight)[] tuples = new (RepairPickUpData data, float weight)[pickUpDatas.Count];
            for (int i = 0; i < pickUpDatas.Count; ++i)
            {
                tuples[i].data = pickUpDatas[i];
                tuples[i].weight = pickUpDatas[i].RepDrawRate * 0.01f;
            }

            Dictionary<RepairDummy, int> pickUpCounts = new();
            var allRepairers = RepairTableTemplate.GetAllRepairDummyTypes();

            // 10번 랜덤 추출 (중복 허용)
            for (int i = 0; i < 10; i++)
            {
                var randPicked = RandomMgr.RandomWithWeights<RepairPickUpData>(tuples);
                var allReps = RepairTableTemplate.GetAllRepairDummyTypes();
                RepairDummy selected = new();
                foreach (var rep in allReps)
                {
                    if (rep.ID == randPicked.ItemID)
                    {
                        selected = rep;
                        break;
                    }
                }
                // 이미 등장한 숫자면 +1, 아니면 1로 시작
                if (pickUpCounts.ContainsKey(selected))
                {
                    pickUpCounts[selected]++;
                }
                else
                {
                    pickUpCounts.Add(selected, 1);
                }
            }

            // 각 숫자마다 프리팹 생성
            foreach (var kvp in pickUpCounts)
            {
                // kvp.Key: 숫자, kvp.Value: 횟수
                var repairerDummy = kvp.Key;
                var savedRepairer = SaveLoadMgr.GameData.savedRepairerData.GetSavedRepairer(repairerDummy.Grade, repairerDummy.Type);
                //repairerDummy.Count += 1;
                savedRepairer.count += 1;
                savedRepairer.isUnlocked = true;

                CreatePickInfo(repairerDummy.Grade, repairerDummy.Type, kvp.Value);
            }

            var repairerTicketCount = AccountMgr.ItemCount(ItemType.RepairTicket);
            repairerTicketCount -= 10;
            AccountMgr.SetItemCount(ItemType.RepairTicket, repairerTicketCount);
            if(SaveLoadMgr.GameData.savedShopItemData.AddAffinityExp(AffinityLevelType.Repair, 10) != 0)
            {

            }
            SaveLoadMgr.ApplySavedData(SaveDataTypes.Repairer);
            SaveLoadMgr.CallSaveGameData();
            currentPickUpType = PickUpType.Repairer;
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        public void SetPickUpTypeCrew()
        {

        }
        public void SetPickUpTypeCannon()
        {

        }
        public void SetPickUpTypeRepairer()
        {

        }

        public void OnClickPickUpOne()
        {
            switch (currentPickUpType)
            {
                case PickUpType.Crew:
                    RandomPick();
                    break;
                case PickUpType.Cannon:
                    OnClickCannonOnePick();
                    break;
                case PickUpType.Repairer:
                    OnClickReapirOnePick();
                    break;
            }
        }
        public void OnClickPickUpTen()
        {
            switch (currentPickUpType)
            {
                case PickUpType.Crew:
                    RandomTenPick();
                    break;
                case PickUpType.Cannon:
                    OnClickCannonTenPick();
                    break;
                case PickUpType.Repairer:
                    OnClickReapirTenPick();
                    break;
            }
        } // Scope by class TestRandomPick

    } // namespace Root
}