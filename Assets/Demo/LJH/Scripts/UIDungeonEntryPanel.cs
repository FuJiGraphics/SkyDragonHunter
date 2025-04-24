using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIDungeonEntryPanel : MonoBehaviour
    {
        // Static Fields
        private const string StagePrefabName = "StagePrefab";

        // Fields
        [SerializeField] private Button[] m_DungeonTypeButtons;
        [SerializeField] private int[] m_DungeonStageCounts = { 5, 6, 7 };
        [SerializeField] private DungeonType m_SelectedDungeonType;
        [SerializeField] private int m_SelectedDungeonIndex;

        [SerializeField] private Transform m_ScrollViewContent;
        [SerializeField] private UIDungeonStages m_DungeonStagePrefab;
        [SerializeField] private List<UIDungeonStages> m_StageList;

        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Button m_EnterButton;

        // Unity Methods
        private void Start()
        {
            Init();
        }

        // Private Methods
        private void Init()
        {
            AddListeners();
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

            if(AccountMgr.Ticket > 0)
            {
                m_EnterButton.interactable = true;
            }
            else
            {
                m_EnterButton.interactable = false;
            }
        }
    } // Scope by class UIDungeonEntryPanel

} // namespace Root