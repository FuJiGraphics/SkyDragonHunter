using SkyDragonHunter.Utility;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class MasteryNode : GraphNode<MasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node Infomations")]
        [SerializeField] private int m_NodeId = -1;
        [SerializeField] private int[] m_NextNodeIds;
        [SerializeField] private int m_StatType = 0;
        [SerializeField] private int m_Level = 0;
        [SerializeField] private bool isActive;

        // 속성 (Properties)
        public override bool IsVisited
        {
            get
            {
                Debug.Log($"{m_NodeId}");
                return isActive;
            }
            set => isActive = value;
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {

        }

        private void Update()
        {

        }

        // Public 메서드
        // Private 메서드
        // Others

    } // public class MasteryNode : GraphNode<MasteryNode>
} // namespace SkyDragonHunter