using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIDungeonEntryPanel : MonoBehaviour
    {
        // Static Fields
        private const string StagePrefabName = "StagePrefab";
        private static readonly int[] m_DungeonStageCounts = { 10, 10, 10 };

        // Fields
        [SerializeField] private Button[] m_DungeonTypeButtons;
        [SerializeField] private DungeonType m_SelectedDungeonType;
        [SerializeField] private int m_SelectedDungeonIndex;

        [SerializeField] private Transform m_ScrollViewContent;
        [SerializeField] private UIDungeonStages m_DungeonStagePrefab;
        [SerializeField] private List<UIDungeonStages> m_StageList;

        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Button m_EnterButton;

        [SerializeField] private TextMeshProUGUI attackTmp;
        [SerializeField] private TextMeshProUGUI defTmp;
        [SerializeField] private TextMeshProUGUI clearCondTmp;

        [SerializeField] private UIStageInfoSlot infoPrefab;
        [SerializeField] private Transform monstersContents;
        [SerializeField] private Transform clearRewardContents;

        // Unity Methods
        private void Start()
        {
            Initiate();
        }
        private void OnEnable()
        {
            ResetUI();            
        }

        // Private Methods
        private void Initiate()
        {
            AddListeners();
            OnClickDungoenType(0);
        }

        private void ResetUI()
        {
            if (m_SelectedDungeonType == DungeonType.Wave)
            {
                OnClickDungoenType(0);
            }
            attackTmp.text = AccountMgr.AccountStats.MaxDamage.ToUnit();
            defTmp.text = AccountMgr.AccountStats.MaxArmor.ToUnit();
        }

        private void AddListeners()
        {
            for (int i = 0; i < (int)DungeonType.Count; ++i)
            {
                int captured = i;
                m_DungeonTypeButtons[i].onClick.AddListener(() => OnClickDungoenType(captured));
            }
            m_EnterButton.onClick.AddListener(OnClickDungeonEnterButton);
            m_CloseButton.onClick.AddListener(OnClickCloseButton);
        }

        private void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }

        private void OnClickDungeonEnterButton()
        {
            DungeonMgr.EnterDungeon(m_SelectedDungeonType, m_SelectedDungeonIndex);
        }

        private void OnClickDungoenType(int dungeonTypeIndex)
        {
            m_EnterButton.interactable = false;
            if (m_SelectedDungeonType != (DungeonType)dungeonTypeIndex)
            {
                ChangeDungeonType((DungeonType)dungeonTypeIndex);
            }
            InstantiateStagePrefabs();
        }

        private void ChangeDungeonType(DungeonType dungeonType)
        {
            m_SelectedDungeonType = dungeonType;
            switch (m_SelectedDungeonType)
            {
                case DungeonType.Wave:
                    clearCondTmp.text = $"제한시간 안에 모든 적 격파";
                    break;
                case DungeonType.Boss:
                    clearCondTmp.text = $"제한시간 안에 보스 격파";
                    break;
                case DungeonType.SandBag:
                    clearCondTmp.text = $"제한시간 안에 샌드백 격파";
                    break;               
            }
        }

        private void InstantiateStagePrefabs()
        {
            if (m_StageList == null)
                m_StageList = new List<UIDungeonStages>();
            foreach (var stagePrefab in m_StageList)
            {
                Destroy(stagePrefab.gameObject);
            }
            m_StageList.Clear();

            for (int i = 0; i < m_DungeonStageCounts[(int)m_SelectedDungeonType]; ++i)
            {
                var stageButton = Instantiate(m_DungeonStagePrefab, m_ScrollViewContent);

                StringBuilder sb = new StringBuilder();
                sb.Append(StagePrefabName);
                sb.Append(i + 1);
                stageButton.name = sb.ToString();
                stageButton.SetLevel(m_SelectedDungeonType, i + 1);
                stageButton.AddListener(() =>
                {
                    m_SelectedDungeonIndex = stageButton.Level;
                    OnSelectLevel();
                });
                stageButton.OnSelectStage(0);
                m_StageList.Add(stageButton);
            }
            var pos = m_ScrollViewContent.position;
            pos.y = 0f;
            m_ScrollViewContent.position = pos;
        }

        private void OnSelectLevel()
        {
            foreach (var stage in m_StageList)
            {
                stage.OnSelectStage(m_SelectedDungeonIndex);
            }

            m_EnterButton.interactable = true;
            //switch (m_SelectedDungeonType)
            //{
            //    case DungeonType.Wave:
            //        if (AccountMgr.ItemCount(ItemType.BossDungeonTicket) > 0)
            //            m_EnterButton.interactable = true;
            //        else
            //            m_EnterButton.interactable = false;
            //        break;
            //    case DungeonType.Boss:
            //        if (AccountMgr.ItemCount(ItemType.WaveDungeonTicket) > 0)
            //            m_EnterButton.interactable = true;
            //        else
            //            m_EnterButton.interactable = false;
            //        break;
            //    case DungeonType.SandBag:
            //        if (AccountMgr.ItemCount(ItemType.SandbagDungeonTicket) > 0)
            //            m_EnterButton.interactable = true;
            //        else
            //            m_EnterButton.interactable = false;
            //        break;
            //}

            SetStageInfoSlots();
        }

        private void ClearInfoSlots()
        {
            foreach (Transform child in monstersContents)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in clearRewardContents)
            {
                Destroy(child.gameObject);
            }
        }

        private void SetStageInfoSlots()
        {
            ClearInfoSlots();
            var dungeonData = DataTableMgr.DungeonTable.Get(m_SelectedDungeonType, m_SelectedDungeonIndex);
            SetMonsterInfoSlots(dungeonData);
            SetRewardInfoSlots(dungeonData);
        }

        private void SetMonsterInfoSlots(DungeonTableData dungeonData)
        {            
            // TODO: LJH Assign icon to null
            switch (m_SelectedDungeonType)
            {
                case DungeonType.Wave:                    
                    var waveData = DataTableMgr.WaveTable.Get(dungeonData.MonsterWaveID);
                    try
                    {
                        for (int i = 0; i < waveData.MonsterIDs.Length; ++i)
                        {
                            var waveMonsterSlot = Instantiate(infoPrefab, monstersContents);
                            waveMonsterSlot.SetSlot(null, waveData.MonsterCounts[i]);
                        }
                    }
                    catch
                    {
                        Debug.LogError($"monsterWaveID{dungeonData.MonsterWaveID}");
                        if(waveData.MonsterIDs == null)
                        {
                            Debug.LogError($"monsterIDs null");
                        }
                        else
                        {
                            Debug.LogError($"MonsterIDS not null, {waveData.MonsterIDs.Length}");
                        }
                    }
                    break;
                case DungeonType.Boss:
                    var bossSlot = Instantiate(infoPrefab, monstersContents);
                    bossSlot.SetSlot(null, 0);
                    break;
                case DungeonType.SandBag:
                    var sandbagSlot = Instantiate(infoPrefab, monstersContents);
                    sandbagSlot.SetSlot(null, 0);
                    break;
            }
        } 
        private void SetRewardInfoSlots(DungeonTableData dungeonData)
        {
            var slot = Instantiate(infoPrefab, clearRewardContents);
            slot.SetSlot(DataTableMgr.ItemTable.Get(dungeonData.RewardItemID).Icon, dungeonData.RewardCounts);
        }
    } // Scope by class UIDungeonEntryPanel
}// namespace Root