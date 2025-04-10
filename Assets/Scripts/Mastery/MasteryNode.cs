using SkyDragonHunter.Utility;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class MasteryNode : GraphNode<MasteryNode>
    {
        // �ʵ� (Fields)
        [Header("Mastery Node Infomations")]
        [SerializeField] private int m_NodeId = -1;
        [SerializeField] private int[] m_NextNodeIds;
        [SerializeField] private int m_StatType = 0;
        [SerializeField] private int m_Level = 0;
        [SerializeField] private bool isActive;

        // �Ӽ� (Properties)
        public override bool IsVisited
        {
            get
            {
                Debug.Log($"{m_NodeId}");
                return isActive;
            }
            set => isActive = value;
        }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        private void Start()
        {

        }

        private void Update()
        {

        }

        // Public �޼���
        // Private �޼���
        // Others

    } // public class MasteryNode : GraphNode<MasteryNode>
} // namespace SkyDragonHunter