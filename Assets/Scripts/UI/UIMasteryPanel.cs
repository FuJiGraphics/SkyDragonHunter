using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIMasteryPanel : Graph<MasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node UI Settings")]
        [SerializeField] private MasteryNode m_UiMasteryNodePrefab;
        [SerializeField] private UIMasteryContent m_UiContent;
        [SerializeField] private UIMasterySocket[] m_SocketPrefabs;

        [Header("Mastery Node UI Buttons")]
        [SerializeField] private Button m_UiMasteryNodeExitButton;

        private List<MasteryNode> m_GenNodeList;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_GenNodeList = new List<MasteryNode>();
            int maxLevel = -1;
            for (int id = 301; id <= 313; ++id)
            {
                var newNodeData = DataTableMgr.MasteryNodeTable.Get(id);
                var newMasteryNodeInstance = Instantiate(m_UiMasteryNodePrefab);

                // 소켓 설정
                var socketData = DataTableMgr.MasterySocketTable.Get(newNodeData.SocketID);
                var socketInstance = Instantiate(m_SocketPrefabs[socketData.StatType]);
                
                // 마스터리 노드 생성
                newMasteryNodeInstance.SetMasteryNodeData(newNodeData, socketInstance);
                if (maxLevel < newNodeData.Level)
                {
                    maxLevel = newNodeData.Level;
                }
                base.AddNode(newMasteryNodeInstance);
                m_GenNodeList.Add(newMasteryNodeInstance);
            }
            SetMaxLevel(maxLevel);
            SetAllNodeIntoLevels();

            m_UiMasteryNodeExitButton.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        // Public 메서드

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

        private void SetAllNodeIntoLevels()
        {
            foreach (var node in m_GenNodeList)
            {
                m_UiContent.AddMasteryNode(node.gameObject, node.Level - 1);
            }
        }

        // Others

    } // Scope by class UIMasteryPanel
} // namespace SkyDragonHunter