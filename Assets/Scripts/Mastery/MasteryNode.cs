using SkyDragonHunter.Tables;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Gameplay {

    public class MasteryNode : GraphNode<MasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node Infomations")]
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private List<int> m_NextNodeIds;
        [SerializeField] private int m_SocketId = -1;
        [SerializeField] private int m_Level = 0;
        [SerializeField] private int m_Position = 0;
        [SerializeField] private bool isActive;

        private UIMasterySocket m_SocketInstance;

        // 속성 (Properties)
        public override bool IsVisited
        {
            get
            {
                Debug.Log($"{ID}");
                return base.IsVisited || m_SocketId == -1;
            }
            set
            {
                base.IsVisited = value;
            }
        }

        public int Level => m_Level;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Button = GetComponent<Button>();
            m_Icon = GetComponent<Image>();
            m_Text = GetComponentInChildren<TextMeshProUGUI>(true);
            m_Button.onClick.AddListener(SocketLevelUp);
        }

        // Public 메서드
        public void SetMasteryNodeData(MasteryNodeData nodeData, UIMasterySocket socketInstance)
        {
            if (nodeData == null)
            {
                Debug.LogWarning("[MasteryNode]: SetMasteryNodeData 실패. nodeData가 null이입니다.");
                return;
            }

            base.ID = nodeData.ID;
            m_NextNodeIds = new List<int>(nodeData.NextNodeIds);
            foreach (var id in m_NextNodeIds)
            {
                base.AddEdge(id);
            }
            m_SocketId = nodeData.SocketID;
            m_Level = nodeData.Level;
            m_Position = nodeData.Position;
            m_SocketInstance = socketInstance;
            m_SocketInstance.SetSocketData(nodeData.SocketID);
            m_Icon.sprite = m_SocketInstance.Icon;
            m_Text.text = m_SocketInstance.SlotCountString;
        }

        public void SocketLevelUp()
        {
            if (m_SocketInstance.LevelUp())
            {
                m_Icon.sprite = m_SocketInstance.Icon;
                m_Text.text = m_SocketInstance.SlotCountString;
            }
        }

        // Private 메서드
        // Others

    } // public class MasteryNode : GraphNode<MasteryNode>
} // namespace SkyDragonHunter