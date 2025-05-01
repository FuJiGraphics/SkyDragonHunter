using SkyDragonHunter.Managers;
using SkyDragonHunter.Utility;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;

namespace SkyDragonHunter.UI {

    public class UIMasteryPanel : Graph<UIMasteryNode>
    {
        // 필드 (Fields)
        [Header("Mastery Node UI Settings")]
        [SerializeField] private UIMasteryNode m_UiMasteryNodePrefab;
        [SerializeField] private UIMasteryContent m_UiContent;
        [SerializeField] private UIMasterySocket[] m_SocketPrefabs;

        [Header("Mastery Node Infomations")]
        [SerializeField] private SpriteRenderer m_Icon;
        [SerializeField] private SpriteRenderer m_NextNodeInfoArrowIcon;
        [SerializeField] private TextMeshProUGUI m_UiPrevNodeInfo;
        [SerializeField] private TextMeshProUGUI m_UiNextNodeInfo;

        [Header("Airship UI Settings")]
        [SerializeField] private TextMeshProUGUI m_NicknameText;
        [SerializeField] private TextMeshProUGUI m_LevelText;
        [SerializeField] private TextMeshProUGUI m_DamageText;
        [SerializeField] private TextMeshProUGUI m_HealthText;

        private Dictionary<int, List<UIMasteryNode>> m_GenNodeMap;
        private UIMasteryNode m_CilckedNode;

        // 속성 (Properties)
        public Dictionary<int, List<UIMasteryNode>> NodeMap => m_GenNodeMap;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnDestroy()
        {
            var airship = GameMgr.FindObject<CharacterStatus>("Airship");
            airship?.RemoveChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            airship?.RemoveChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
            AccountMgr.RemoveLevelUpEvent(OnAccountLevelUp);
            AccountMgr.RemoveNicknameChangedEvent(OnChangedNickname);
        }

        // Public 메서드
        public void Init()
        {
            m_GenNodeMap = new Dictionary<int, List<UIMasteryNode>>();

            var allTableData = DataTableMgr.MasteryNodeTable.ToArray();
            int maxLevel = -1;
            foreach (var newNodeData in allTableData)
            {
                // 마스터리 노드 생성
                var newMasteryNodeInstance = Instantiate(m_UiMasteryNodePrefab);
                newMasteryNodeInstance.onLevelup.AddListener(DirtyMastery);
                newMasteryNodeInstance.onClickedEvent.AddListener(() => { ShowNodeInfo(newMasteryNodeInstance); });
                newMasteryNodeInstance.SetMasteryNodeData(newNodeData.ID);
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

            AccountMgr.AddLevelUpEvent(OnAccountLevelUp);
            AccountMgr.AddNicknameChangedEvent(OnChangedNickname);
            m_NicknameText.text = AccountMgr.Nickname;
            m_LevelText.text = "Lv: " + AccountMgr.CurrentLevel.ToString();
            var airship = GameMgr.FindObject<CharacterStatus>("Airship");
            airship.RemoveChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            airship.RemoveChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
            airship.AddChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            airship.AddChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
        }

        public void OnAccountLevelUp()
        {
            m_LevelText.text = "Lv: " + AccountMgr.CurrentLevel.ToString();
        }

        public void OnChangedNickname(string newNickname)
        {
            m_NicknameText.text = newNickname;
        }

        public void OnChangedAirshipMaxDamage(BigNum stat)
        {
            m_DamageText.text = stat.ToUnit();
        }

        public void OnChangedAirshipMaxHealth(BigNum stat)
        {
            m_HealthText.text = stat.ToUnit();
        }

        public void OnNodeLevelUp()
        {
            if (m_CilckedNode != null)
            {
                m_CilckedNode.SocketLevelUp();
                ShowNodeInfo(m_CilckedNode);
            }
        }

        public void DirtyMastery()
        {
            base.ResetVisitedFlags();
            base.TraverseBFS();
        }

        public void ShowNodeInfo(UIMasteryNode clickedNode)
        {
            m_CilckedNode = clickedNode;
            var currSocketInfo = clickedNode.CurrentSocket;
            if (clickedNode.CurrentLevel > 0)
            {
                m_UiPrevNodeInfo.text = currSocketInfo.Description;
                if (currSocketInfo.NextLevelSocket != null)
                {
                    m_UiNextNodeInfo.text = currSocketInfo.NextLevelSocket.Description;
                }
                else
                {
                    m_UiNextNodeInfo.text = "최대 레벨";
                }
            }
            else
            {
                m_UiPrevNodeInfo.text = "없음";
                m_UiNextNodeInfo.text = currSocketInfo.Description;
            }
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