using NPOI.SS.UserModel;
using SkyDragonHunter.Database;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Temp;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using SkyDraonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.test
{
    public class TestRandomPick : MonoBehaviour
    {
        // 필드 (Fields)
        private UiMgr uiMgr;
        public GameObject pickUpInfoPrefab;
        public Transform crewAndMoonStone;
        private int[] crewIds;
        private int count = 17;
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
        }

        // Public 메서드
        public void ApplyPickCrew(int crewId)
        {
            var savedCrew = SaveLoadMgr.GameData.savedCrewData.GetCrewData(crewId);
            ApplyPickCrew(savedCrew);
        }

        public void ApplyPickCrew(SavedCrew crew)
        {
            GameObject instance = crew.crewData.GetInstance();
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

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.01f;

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
            }
            var crewTicketCount = AccountMgr.ItemCount(ItemType.CrewTicket);
            crewTicketCount -= 1;
            AccountMgr.SetItemCount(ItemType.CrewTicket, crewTicketCount);
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Crew);
        }

        public void RandomTenPick()
        {
            if (!SaveLoadMgr.GameData.savedShopItemData.isFirstPickGiven)
            {
                GiveFirstPickReward();
                return;
            }

            if (AccountMgr.ItemCount(ItemType.CrewTicket) < 10)
            {
                DrawableMgr.Dialog($"안내", $"단원 소환 티켓이 부족합니다");
                return;
            }


            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.1f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 각 숫자의 등장 횟수를 기록할 Dictionary
            //Dictionary<int, int> pickUpCounts = new();
            //int existingCount = 0;

            //// 10번 랜덤 추출 (중복 허용)
            //for (int i = 0; i < 10; i++)
            //{
            //    var randIndex = Random.Range(0, crewIds.Length);
            //    int selected = crewIds[randIndex];
            //    Debug.Log($"Random Pick selected Crew [index/ID] [{selected}/{randIndex}]");

            //    // 이미 등장한 숫자면 +1, 아니면 1로 시작
            //    if (pickUpCounts.ContainsKey(selected))
            //    {
            //        existingCount++;
            //        Debug.Log($"key [{selected}] exists already, increasing existingCount : {existingCount}");
            //    }
            //    else
            //    {
            //        pickUpCounts.Add(selected, 1);
            //    }
            //}


            // 각 숫자마다 프리팹 생성
            var idList = crewIds.ToList();
            for (int i = 0; i < 10; ++i)
            {
                // kvp.Key: 숫자, kvp.Value: 횟수
                var ranId = RandomMgr.Random<int>(idList);
                var crewData = SaveLoadMgr.GameData.savedCrewData.GetCrewData(ranId);
                if (crewData.isUnlocked)
                {
                    AccountMgr.AddItemCount(ItemType.MasteryLevelUp, 1);
                    CreatePickInfo(ItemType.MasteryLevelUp.GetDescription(),
                        1, DataTableMgr.ItemTable.Get(ItemType.MasteryLevelUp).Icon);
                }
                else
                {
                    ApplyPickCrew(crewData);
                    crewData.isUnlocked = true;
                    CreatePickInfo(ranId, 1);
                }
            }

            //if (existingCount > 0)
            //{
            //    CreateMateryResourcePickInfo(existingCount);                
            //    AccountMgr.AddItemCount(ItemType.MasteryLevelUp, existingCount);
            //}

            var crewTicketCount = AccountMgr.ItemCount(ItemType.CrewTicket);
            crewTicketCount -= 10;
            AccountMgr.SetItemCount(ItemType.CrewTicket, crewTicketCount);
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var ui = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            ui.SetType(PickCrewInfoType.Crew);
        }

        public void RandomFreePick()
        {
            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel"); 
            if (!uiSummon.IsDone)
            {
                DrawableMgr.Dialog($"안내", $"보너스 경험치가 부족합니다.");
                return;
            }
            else
            {
                uiSummon.ResetExp();
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            int selectedCrew = crewIds[Random.Range(0, crewIds.Length - 1)];

            // 숫자 드롭 생성
            if (SaveLoadMgr.GameData.savedCrewData.GetCrewData(selectedCrew).isUnlocked)
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
            }

            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Crew);
        }

        public void RandomPickCannon()
        {
            if (AccountMgr.ItemCount(ItemType.CanonTicket) < 1)
            {
                DrawableMgr.Dialog($"안내", $"대포 소환 티켓이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.CanonTicket);
            count = count - 1;
            AccountMgr.SetItemCount(ItemType.CanonTicket, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.01f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<CanonType>(
                            (CanonType.Normal,   0.65f),
                            (CanonType.Repeater, 0.2f),
                            (CanonType.Burn,     0.1f),
                            (CanonType.Slow,     0.07f),
                            (CanonType.Freeze,   0.03f));
            var ranGrade = RandomMgr.RandomWithWeights<CanonGrade>(
                            (CanonGrade.Normal,  0.85f),
                            (CanonGrade.Rare,    0.1f),
                            (CanonGrade.Unique,  0.04f),
                            (CanonGrade.Legend,  0.01f));

            AccountMgr.AddCannonCount(ranType, ranGrade, 1);

            string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
            Sprite icon = CanonTable.GetIcon(ranType);
            var ui = CreatePickInfo(name, 1, icon);
            ui.SetPanel(CanonTable.GetGradeOutline(ranGrade));
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Cannon);
        }

        public void RandomTenPickCannon()
        {
            if (AccountMgr.ItemCount(ItemType.CanonTicket) < 10)
            {
                DrawableMgr.Dialog($"안내", $"대포 소환 티켓이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.CanonTicket);
            count = count - 10;
            AccountMgr.SetItemCount(ItemType.CanonTicket, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.1f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            for (int i = 0; i < 10; ++i)
            {
                var ranType = RandomMgr.RandomWithWeights<CanonType>(
                                (CanonType.Normal, 0.65f),
                                (CanonType.Repeater, 0.2f),
                                (CanonType.Burn, 0.1f),
                                (CanonType.Slow, 0.07f),
                                (CanonType.Freeze, 0.03f));
                var ranGrade = RandomMgr.RandomWithWeights<CanonGrade>(
                                (CanonGrade.Normal, 0.85f),
                                (CanonGrade.Rare, 0.1f),
                                (CanonGrade.Unique, 0.04f),
                                (CanonGrade.Legend, 0.01f));
                AccountMgr.AddCannonCount(ranType, ranGrade, 1);

                string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
                Sprite icon = CanonTable.GetIcon(ranType);
                var ui = CreatePickInfo(name, 1, icon);
                ui.SetPanel(CanonTable.GetGradeOutline(ranGrade));
            }

            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Cannon);
        }

        public void RandomFreePickCannon()
        {
            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            if (!uiSummon.IsDone)
            {
                DrawableMgr.Dialog($"안내", $"보너스 경험치가 부족합니다.");
                return;
            }
            else
            {
                uiSummon.ResetExp();
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<CanonType>(
                            (CanonType.Normal, 0.65f),
                            (CanonType.Repeater, 0.2f),
                            (CanonType.Burn, 0.1f),
                            (CanonType.Slow, 0.07f),
                            (CanonType.Freeze, 0.03f));
            var ranGrade = RandomMgr.RandomWithWeights<CanonGrade>(
                            (CanonGrade.Normal, 0.85f),
                            (CanonGrade.Rare, 0.1f),
                            (CanonGrade.Unique, 0.04f),
                            (CanonGrade.Legend, 0.01f));

            AccountMgr.AddCannonCount(ranType, ranGrade, 1);

            string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
            Sprite icon = CanonTable.GetIcon(ranType);
            var ui = CreatePickInfo(name, 1, icon);
            ui.SetPanel(CanonTable.GetGradeOutline(ranGrade));
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Cannon);
        }

        public void RandomPickRepairer()
        {
            if (AccountMgr.ItemCount(ItemType.RepairTicket) < 1)
            {
                DrawableMgr.Dialog($"안내", $"수리공 소환 티켓이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.RepairTicket);
            count = count - 1;
            AccountMgr.SetItemCount(ItemType.RepairTicket, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.01f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<RepairType>(
                            (RepairType.Normal, 0.65f),
                            (RepairType.Elite, 0.2f),
                            (RepairType.Shield, 0.1f),
                            (RepairType.Healer, 0.07f),
                            (RepairType.Divine, 0.03f));
            var ranGrade = RandomMgr.RandomWithWeights<RepairGrade>(
                            (RepairGrade.Normal, 0.85f),
                            (RepairGrade.Rare, 0.1f),
                            (RepairGrade.Unique, 0.04f),
                            (RepairGrade.Legend, 0.01f));
            AccountMgr.AddRepairCount(ranType, ranGrade, 1);

            string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
            Sprite icon = RepairTableTemplate.GetIcon(ranType);
            var ui = CreatePickInfo(name, 1, icon);
            ui.SetPanel(RepairTableTemplate.GetGradeOutline(ranGrade));
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Repairer);
        }

        public void RandomTenPickRepairer()
        {
            if (AccountMgr.ItemCount(ItemType.RepairTicket) < 10)
            {
                DrawableMgr.Dialog($"안내", $"수리공 소환 티켓이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.RepairTicket);
            count = count - 10;
            AccountMgr.SetItemCount(ItemType.RepairTicket, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.1f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            List<KeyValuePair<RepairType, RepairGrade>> m_RanList = new();
            for (int i = 0; i < 10; ++i)
            {
                var ranType = RandomMgr.RandomWithWeights<RepairType>(
                                (RepairType.Normal, 0.65f),
                                (RepairType.Elite, 0.2f),
                                (RepairType.Shield, 0.1f),
                                (RepairType.Healer, 0.07f),
                                (RepairType.Divine, 0.03f));
                var ranGrade = RandomMgr.RandomWithWeights<RepairGrade>(
                                (RepairGrade.Normal, 0.85f),
                                (RepairGrade.Rare, 0.1f),
                                (RepairGrade.Unique, 0.04f),
                                (RepairGrade.Legend, 0.01f));
                m_RanList.Add(new(ranType, ranGrade));
                AccountMgr.AddRepairCount(ranType, ranGrade, 1);

                string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
                Sprite icon = RepairTableTemplate.GetIcon(ranType);
                var ui = CreatePickInfo(name, 1, icon);
                ui.SetPanel(RepairTableTemplate.GetGradeOutline(ranGrade));
            }

            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Repairer);
        }

        public void RandomFreePickRepairer()
        {
            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            if (!uiSummon.IsDone)
            {
                DrawableMgr.Dialog($"안내", $"보너스 경험치가 부족합니다.");
                return;
            }
            else
            {
                uiSummon.ResetExp();
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<RepairType>(
                            (RepairType.Normal, 0.65f),
                            (RepairType.Elite, 0.2f),
                            (RepairType.Shield, 0.1f),
                            (RepairType.Healer, 0.07f),
                            (RepairType.Divine, 0.03f));
            var ranGrade = RandomMgr.RandomWithWeights<RepairGrade>(
                            (RepairGrade.Normal, 0.85f),
                            (RepairGrade.Rare, 0.1f),
                            (RepairGrade.Unique, 0.04f),
                            (RepairGrade.Legend, 0.01f));
            AccountMgr.AddRepairCount(ranType, ranGrade, 1);

            string name = ranType.GetDescription() + "/" + ranGrade.GetDescription();
            Sprite icon = RepairTableTemplate.GetIcon(ranType);
            var ui = CreatePickInfo(name, 1, icon);
            ui.SetPanel(RepairTableTemplate.GetGradeOutline(ranGrade));
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var rewardPanel = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            rewardPanel.SetType(PickCrewInfoType.Repairer);
        }

        public void RandomPickSpoils()
        {
            if (AccountMgr.ItemCount(ItemType.Spoils) < 1)
            {
                DrawableMgr.Dialog($"안내", $"소환에 필요한 전리품이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.Spoils);
            count = count - 1;
            AccountMgr.SetItemCount(ItemType.Spoils, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.01f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<ItemType>(
                            (ItemType.Food, 0.15f),
                            (ItemType.Wood, 0.15f),
                            (ItemType.CanonTicket, 0.1f),
                            (ItemType.RepairTicket, 0.1f),
                            (ItemType.Steel, 0.1f),
                            (ItemType.Brick, 0.1f),
                            (ItemType.BossDungeonTicket, 0.1f),
                            (ItemType.WaveDungeonTicket, 0.1f),
                            (ItemType.SandbagDungeonTicket, 0.1f));

            int ranCount = 0;
            if (ItemType.Food == ranType || ItemType.Wood == ranType)
            {
                ranCount = Random.Range(1, 5);
            }
            else if (ItemType.CanonTicket == ranType || ItemType.RepairTicket == ranType || ItemType.Brick == ranType)
            {
                ranCount = Random.Range(1, 3);
            }
            else
            {
                ranCount = 1;
            }
            AccountMgr.AddItemCount(ranType, ranCount);

            CreatePickInfo(ranType.GetDescription(), ranCount, DataTableMgr.ItemTable.Get(ranType).Icon);
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var ui = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            ui.SetType(PickCrewInfoType.Spoils);
        }

        public void RandomTenPickSpoils()
        {
            if (AccountMgr.ItemCount(ItemType.Spoils) < 10)
            {
                DrawableMgr.Dialog($"안내", $"소환에 필요한 전리품이 부족합니다");
                return;
            }
            BigNum count = AccountMgr.ItemCount(ItemType.Spoils);
            count = count - 10;
            AccountMgr.SetItemCount(ItemType.Spoils, count);

            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            uiSummon.Exp += 0.1f;

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            for (int i = 0; i < 10; ++i)
            {
                var ranType = RandomMgr.RandomWithWeights<ItemType>(
                                (ItemType.Food, 0.15f),
                                (ItemType.Wood, 0.15f),
                                (ItemType.CanonTicket, 0.1f),
                                (ItemType.RepairTicket, 0.1f),
                                (ItemType.Steel, 0.1f),
                                (ItemType.Brick, 0.1f),
                                (ItemType.BossDungeonTicket, 0.1f),
                                (ItemType.WaveDungeonTicket, 0.1f),
                                (ItemType.SandbagDungeonTicket, 0.1f));

                int ranCount = 0;
                if (ItemType.Food == ranType || ItemType.Wood == ranType)
                {
                    ranCount = Random.Range(1, 5);
                }
                else if (ItemType.CanonTicket == ranType || ItemType.RepairTicket == ranType || ItemType.Brick == ranType)
                {
                    ranCount = Random.Range(1, 3);
                }
                else
                {
                    ranCount = 1;
                }
                AccountMgr.AddItemCount(ranType, ranCount);
                CreatePickInfo(ranType.GetDescription(), ranCount, DataTableMgr.ItemTable.Get(ranType).Icon);
            }

            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();


            var ui = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            ui.SetType(PickCrewInfoType.Spoils);
        }

        public void RandomFreePickSpoils()
        {
            var uiSummon = GameMgr.FindObject<UISummonPanel>("UISummonPanel");
            if (!uiSummon.IsDone)
            {
                DrawableMgr.Dialog($"안내", $"보너스 경험치가 부족합니다.");
                return;
            }
            else
            {
                uiSummon.ResetExp();
            }

            foreach (Transform child in crewAndMoonStone)
            {
                Destroy(child.gameObject);
            }

            // 1개 랜덤 선택
            var ranType = RandomMgr.RandomWithWeights<ItemType>(
                            (ItemType.Food, 0.15f),
                            (ItemType.Wood, 0.15f),
                            (ItemType.CanonTicket, 0.1f),
                            (ItemType.RepairTicket, 0.1f),
                            (ItemType.Steel, 0.1f),
                            (ItemType.Brick, 0.1f),
                            (ItemType.BossDungeonTicket, 0.1f),
                            (ItemType.WaveDungeonTicket, 0.1f),
                            (ItemType.SandbagDungeonTicket, 0.1f));

            int ranCount = 0;
            if (ItemType.Food == ranType || ItemType.Wood == ranType)
            {
                ranCount = Random.Range(1, 5);
            }
            else if (ItemType.CanonTicket == ranType || ItemType.RepairTicket == ranType || ItemType.Brick == ranType)
            {
                ranCount = Random.Range(1, 3);
            }
            else
            {
                ranCount = 1;
            }
            AccountMgr.AddItemCount(ranType, ranCount);

            CreatePickInfo(ranType.GetDescription(), ranCount, DataTableMgr.ItemTable.Get(ranType).Icon);
            SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();

            var ui = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            ui.SetType(PickCrewInfoType.Spoils);
        }

        private void GiveFirstPickReward()
        {
            SaveLoadMgr.GameData.savedShopItemData.isFirstPickGiven = true;

            var ui = GameMgr.FindObject<UIRandomPickCrewInfo>("UIRandomPickCrewInfo");
            ui.SetType(PickCrewInfoType.Crew);

            int selectedCrew = 14101;
            Debug.Log($"Random Pick selected Crew [ID] [{selectedCrew}]");
            ApplyPickCrew(selectedCrew);
            SaveLoadMgr.GameData.savedCrewData.GetCrewData(selectedCrew).isUnlocked = true;
            CreatePickInfo(selectedCrew, 1);
            // SaveLoadMgr.CallSaveGameData();
            uiMgr.OnOffRandomCrewPickUpInfo();
        }

        // Private 메서드
        private PickUpInfoUI CreatePickInfo(string name, int count, Sprite icon)
        {
            // 프리팹 생성 및 부모 오브젝트 지정
            GameObject instance = Instantiate(pickUpInfoPrefab, crewAndMoonStone);

            // 프리팹에 붙어 있는 PickUpInfoUI 스크립트 가져오기
            var ui = instance.GetComponent<PickUpInfoUI>();

            // 스크립트가 존재하면 이름/수량 적용
            if (ui != null)
            {
                ui.SetData(name, count);
                ui.SetIcon(icon);
            }
            return ui;
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

    } // Scope by class TestRandomPick

} // namespace Root