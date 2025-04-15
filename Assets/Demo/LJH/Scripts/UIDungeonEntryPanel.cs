using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class UIDungeonEntryPanel : MonoBehaviour
    {
        // Static Fields
        private const string StagePrefabName = "StagePrefab";

        // Fields
        [SerializeField] private Button[] m_DungeonTypeButtons;
        [SerializeField] private int[] m_DungeonStageCounts = {5,6,7};
        [SerializeField] private DungeonType m_SelectedDungeonType;
        [SerializeField] private int m_SelectedDungeonIndex;

        [SerializeField] private Transform m_ScrollViewContent;
        [SerializeField] private UIDungeonStages m_DungeonStagePrefab;
        [SerializeField] private List<GameObject> m_StageList;

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
                m_DungeonTypeButtons[i].onClick.AddListener(() => OnClickDungoenType(i));
            }
            m_EnterButton.onClick.AddListener(OnClickDungeonEnterButton);
        }

        private void OnClickDungeonEnterButton()
        {
            DungeonMgr.EnterDungeon(m_SelectedDungeonType, m_SelectedDungeonIndex);
        }

        private void OnClickDungoenType(int dungeonTypeIndex)
        {
            if(m_SelectedDungeonType != (DungeonType)dungeonTypeIndex)
            {
                ChangeDungeonType((DungeonType)dungeonTypeIndex);
            }
        }

        private void ChangeDungeonType(DungeonType dungeonType)
        {
            m_SelectedDungeonType = dungeonType;
            InstantiateStagePrefabs();
        }

        private void InstantiateStagePrefabs()
        {
            if(m_StageList == null)
                m_StageList = new List<GameObject>();
            foreach (var stagePrefab in m_StageList)
            {
                Destroy(stagePrefab);
            }
            m_StageList.Clear();

            for (int i = 0; i < m_DungeonStageCounts[(int)m_SelectedDungeonType]; ++i)
            {
                var stageButton = Instantiate(m_DungeonStagePrefab, m_ScrollViewContent);
                
                StringBuilder sb = new StringBuilder();
                sb.Append(StagePrefabName);
                sb.Append(i + 1);
                stageButton.name = sb.ToString();
                stageButton.SetLevel(i + 1);
            }
        }
    } // Scope by class UIDungeonEntryPanel

} // namespace Root