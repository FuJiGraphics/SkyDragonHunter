using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIStageSelectPanel : MonoBehaviour
    {
        [SerializeField] private int selectedMission;
        [SerializeField] private int selectedZone;
        private const string missionZoneFormat = "{0} - {1}";

        [SerializeField] private Button[] missionButtons;
        [SerializeField] private Button[] zoneButtons;

        [SerializeField] private UIStageInfoSlot prefab;

        [SerializeField] private Transform monsterContents;
        [SerializeField] private Transform monsterRewardContents;
        [SerializeField] private Transform clearRewardContents;

        private void Start()
        {
            if (selectedMission == 0)
                selectedMission = SaveLoadMgr.GameData.savedStageData.currentStage;
            if (selectedZone == 0)
                selectedZone = SaveLoadMgr.GameData.savedStageData.currentZone;
            AddListseners();
            SetMissionButtonInteractables();
            OnClickMissionButton(selectedMission);
        }

        private void OnEnable()
        {
            if (selectedMission == 0)
                selectedMission = SaveLoadMgr.GameData.savedStageData.currentStage;
            if (selectedZone == 0)
                selectedZone = SaveLoadMgr.GameData.savedStageData.currentZone;
            SetMissionButtonInteractables();
            OnClickMissionButton(selectedMission);
        }

        private void AddListseners()
        {
            for(int i = 0; i < missionButtons.Length; ++i)
            {
                int capturedMission = i + 1;
                missionButtons[i].onClick.AddListener(() =>
                {                    
                    OnClickMissionButton(capturedMission);
                });
            }
            for(int i = 0; i < zoneButtons.Length; ++i)
            {
                int capturedZone = i + 1;
                zoneButtons[i].onClick.AddListener(() =>
                {
                    OnClickZoneButton(capturedZone);
                });
            }
        }

        private void OnClickMissionButton(int mission)
        {
            selectedMission = mission;
            int triedMission = SaveLoadMgr.GameData.savedStageData.GetTriedMission();
            int triedZone = SaveLoadMgr.GameData.savedStageData.GetTriedZone();
            for (int i = 0; i < zoneButtons.Length; ++i)
            {
                var buttonText = zoneButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = string.Format(missionZoneFormat, mission, i + 1);

                if(selectedMission < triedMission)
                {
                    zoneButtons[i].interactable = true;
                }
                else if (selectedMission == triedMission)
                {
                    if(i < triedZone)
                    {
                        zoneButtons[i].interactable = true;
                    }
                    else
                    {
                        zoneButtons[i].interactable = false;
                    }
                }
                else
                {
                    zoneButtons[i].interactable = false;
                }
            }

            ClearSlotContents();

            if (selectedMission < triedMission)
            {
                Debug.LogWarning($"s{selectedMission}, t{triedMission}");
                OnClickZoneButton(20);
            }
            else if (selectedMission == triedMission)
            {
                OnClickZoneButton(triedZone);
            }
        }

        private void ClearSlotContents()
        {
            foreach (Transform child in monsterContents)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in monsterRewardContents)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in clearRewardContents)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnClickZoneButton(int zone)
        {
            selectedZone = zone;

            ClearSlotContents();

            // TODO: LJH Assign images to Null
            var stageData = DataTableMgr.StageTable.Get(selectedMission, selectedZone);
            if (stageData == null)
            {
                Debug.LogWarning($"Cannot find stageData with m/z [{selectedMission}/{selectedZone}]");
            }

            var waveData = DataTableMgr.WaveTable.Get(stageData.WaveTableID);

            var monsterIDs = waveData.MonsterIDs;
            for(int i = 0; i < monsterIDs.Length; ++i)
            {
                var monsterSlot = Instantiate(prefab, monsterContents);
                var monsterIcon = DataTableMgr.MonsterTable.Get(monsterIDs[i]).Icon;
                monsterSlot.SetSlot(monsterIcon, waveData.MonsterCounts[i]);
            }
            var bossSlot = Instantiate(prefab, monsterContents);
            var bossIcon = DataTableMgr.BossTable.Get(stageData.ChallengeBossID).Icon;
            bossSlot.SetSlot(bossIcon, 0);

            var goldSlot = Instantiate(prefab, monsterRewardContents);
            goldSlot.SetSlot(DataTableMgr.ItemTable.Get(ItemType.Coin).Icon, stageData.MonsterGOLD);
            var expSlot = Instantiate(prefab, monsterRewardContents);
            var expIcon = ResourcesMgr.Load<Sprite>("UI_Atlas[UI_Atlas_198]");
            expSlot.SetSlot(expIcon, stageData.MonsterEXP);

            var clearGoldSlot = Instantiate(prefab, clearRewardContents);
            clearGoldSlot.SetSlot(DataTableMgr.ItemTable.Get(ItemType.Coin).Icon, stageData.ClearRewardGold);
            var clearExpSlot = Instantiate(prefab, clearRewardContents);
            clearExpSlot.SetSlot(expIcon, stageData.ClearRewardEXP);
            var clearDiaSlot = Instantiate(prefab, clearRewardContents);
            clearDiaSlot.SetSlot(DataTableMgr.ItemTable.Get(ItemType.Diamond).Icon, stageData.ClearRewardDiamond);

            var contentsRect = clearRewardContents.GetComponent<RectTransform>();
            contentsRect.anchoredPosition = Vector2.zero;
            contentsRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            // ~TODO
        }

        private void SetMissionButtonInteractables()
        {
            int triedMission = SaveLoadMgr.GameData.savedStageData.GetTriedMission();
            for(int i = 0; i < missionButtons.Length; ++i)
            {
                if(i < triedMission)
                {
                    missionButtons[i].interactable = true;
                }
                else
                {
                    missionButtons[i].interactable = false;
                }
            }
        }
    } // Scope by class UIStageSelectPanel

} // namespace Root