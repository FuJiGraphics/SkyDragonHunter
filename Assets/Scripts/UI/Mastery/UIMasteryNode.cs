using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.Utility;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIMasteryNode : GraphNode<UIMasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node Infomations")]
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private TextMeshProUGUI m_StatTempText;

        private UIMasterySocket m_SocketInstance;

        // 속성 (Properties)
        public Sprite Icon => m_Icon.sprite;
        public int LevelGroup { get; set; }
        public bool IsMaxLevel => (m_SocketInstance != null) ? m_SocketInstance.IsMaxLevel : false;
        public bool IsActiveNode { get; private set; } = false;
        public int CurrentLevel => m_SocketInstance.CurrentLevel;
        public UIMasterySocket CurrentSocket => m_SocketInstance;
        public UIMasteryEdgeContent MasteryEdgeContent = null;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        public UnityEvent onLevelup;
        public UnityEvent onClickedEvent;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Button = GetComponent<Button>();
            m_Icon = GetComponent<Image>();
            m_Button.onClick.AddListener(() => { onClickedEvent?.Invoke(); } );
        }

        // Public 메서드
        public void SetMasteryNodeData(int masteryNodeId)
        {
            if (masteryNodeId < 0)
            {
                Debug.LogWarning("[MasteryNode]: SetMasteryNodeData 실패. 잘못된 인덱스 접근");
                return;
            }

            var nodeData = DataTableMgr.MasteryNodeTable.Get(masteryNodeId);
            if (nodeData == null)
            {
                Debug.LogWarning("[MasteryNode]: SetMasteryNodeData 실패. nodeData가 null이입니다.");
                return;
            }

            base.ID = nodeData.ID;
            var nextNodeIds = new List<int>(nodeData.NextNodeIds);
            foreach (var id in nextNodeIds)
            {
                base.AddEdge(id);
            }
            m_SocketInstance = new UIMasterySocket(nodeData.SocketID);
            m_Text.text = m_SocketInstance.SlotCountString;
            m_StatTempText.text = m_SocketInstance.SocketName;
            m_Icon.sprite = m_SocketInstance.Icon;
        }

        public void SocketLevelUp()
        {
            if (m_SocketInstance.LevelUp())
            {
                m_Text.text = m_SocketInstance.SlotCountString;
                onLevelup?.Invoke();
                SaveLoadMgr.CallSaveGameData();
            }
        }

        public override void OnVisited(UIMasteryNode[] edgeNodes)
        {
            if (edgeNodes == null)
            {
                SetActiveNode(true);
                return;
            }

            bool isAllowed = true;
            foreach (var edge in edgeNodes)
            {
                if (!edge.IsMaxLevel)
                {
                    isAllowed = false;
                    break;
                }
            }
            SetActiveNode(isAllowed);
            if (MasteryEdgeContent != null)
            {
                var newNodeData = DataTableMgr.MasteryNodeTable.Get(base.ID);
                MasteryEdgeContent.DrawUILineConnections(newNodeData.Level, edgeNodes);
            }
        }

        public void SetActiveNode(bool enabled)
        {
            IsActiveNode = enabled;
            if (enabled)
            {
                m_Button.interactable = true;
                m_Icon.color = Color.white;
            }
            else
            {
                m_Button.interactable = false;
                m_Icon.color = Color.gray;
            }
        }

        // Private 메서드

        // Others

    } // public class MasteryNode : GraphNode<MasteryNode>
} // namespace SkyDragonHunter