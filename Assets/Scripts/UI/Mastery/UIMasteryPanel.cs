using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SkyDragonHunter.UI {

    public class UIMasteryPanel : Graph<UIMasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node UI Settings")]
        [SerializeField] private int m_UiMasteryTableStartId;
        [SerializeField] private int m_UiMasteryTableEndId;
        [SerializeField] private UIMasteryNode m_UiMasteryNodePrefab;
        [SerializeField] private UIMasteryContent m_UiContent;
        [SerializeField] private UIMasterySocket[] m_SocketPrefabs;

        private Dictionary<int, List<UIMasteryNode>> m_GenNodeMap 
            = new Dictionary<int, List<UIMasteryNode>>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            int maxLevel = -1;
            for (int id = m_UiMasteryTableStartId; id <= m_UiMasteryTableEndId; ++id)
            {
                var newNodeData = DataTableMgr.MasteryNodeTable.Get(id);

                // 마스터리 노드 생성
                var newMasteryNodeInstance = Instantiate(m_UiMasteryNodePrefab);
                newMasteryNodeInstance.onLevelup.AddListener(DirtyMastery);
                newMasteryNodeInstance.SetMasteryNodeData(id);
                if (maxLevel < newNodeData.Level)
                {
                    maxLevel = newNodeData.Level;
                }
                base.AddNode(newMasteryNodeInstance);

                if (!m_GenNodeMap.ContainsKey(newNodeData.Level))
                {
                    m_GenNodeMap.Add(newNodeData.Level, new List<UIMasteryNode>());
                }
                m_GenNodeMap[newNodeData.Level].Add(newMasteryNodeInstance);
            }
            SetMaxLevel(maxLevel);
            InsertAllNodeIntoLevels();
            DirtyMastery();
        }

        // Public 메서드
        public void DirtyMastery()
        {
            base.ResetVisitedFlags();
            base.TraverseBFS();
        }

        // Private 메서드
        private void SetMaxLevel(int maxLevel)
        {
            if (maxLevel <= 0)
                return;

            for (int i = 0; i < maxLevel; ++i)
            {
                m_UiContent.IncreaseLevel();
            }
        }

        private void InsertAllNodeIntoLevels()
        {
            foreach (var nodeList in m_GenNodeMap)
            {
                foreach (var node in nodeList.Value)
                {
                    m_UiContent.AddMasteryNode(node, nodeList.Key - 1);
                }
            }
        }

        // Others

    } // Scope by class UIMasteryPanel
} // namespace SkyDragonHunter