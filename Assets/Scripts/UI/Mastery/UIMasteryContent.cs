using NPOI.SS.Formula.UDF;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIMasteryContent : MonoBehaviour
    {
        private const float k_BaseHeight = 1080f;

        // 필드 (Fields)
        [SerializeField] private UIMasteryPanel m_UiMasteryPanel;
        [SerializeField] private GameObject m_LevelGroupPrefab;
        [SerializeField] private UIMasteryEdgeContent m_MasteryEdgeContent;
        [SerializeField] private ScrollRect m_ScrollRect;
        [SerializeField] private float m_LevelGroupPadding = 160f;

        private List<GameObject> m_Levels = new List<GameObject>();

        // 속성 (Properties)
        public int MaxLevel => m_Levels.Count;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            UpdateNodeFocus();
        }

        // Public 메서드
        public void IncreaseLevel()
        {
            GameObject levelGroupInstance = Instantiate(m_LevelGroupPrefab);
            levelGroupInstance.transform.SetParent(transform);
            levelGroupInstance.transform.position = Vector3.zero;
            levelGroupInstance.transform.localScale = Vector3.one;
            m_Levels.Add(levelGroupInstance);
            var rect = GetComponent<RectTransform>();

            float padding = GetResponsiveLevelGroupPadding();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + padding);
            m_ScrollRect.verticalNormalizedPosition = 0f; 
        }

        public void AddMasteryNode(UIMasteryNode node, int level)
        {
            if (level < 0 || level > m_Levels.Count)
            {
                Debug.LogWarning("[UIMasteryContent]: InsertMasteryNode 실패 Level 범위 초과");
                return;
            }
            if (node == null)
            {
                Debug.LogWarning("[UIMasteryContent]: InsertMasteryNode 실패 node가 null입니다.");
                return;
            }
            node.transform.SetParent(m_Levels[m_Levels.Count - level - 1].transform);
            node.transform.position = Vector3.zero;
            node.transform.localScale = Vector3.one;
            node.MasteryEdgeContent = m_MasteryEdgeContent;
            node.LevelGroup = level;
        }

        // Private 메서드
        private int FindActivedCurrentLevel()
        {
            var keys = m_UiMasteryPanel.NodeMap.Keys.ToList();
            keys.Sort();

            int result = 0;

            foreach (var key in keys)
            {
                var nodes = m_UiMasteryPanel.NodeMap[key];
                bool isActive = true;

                foreach (var node in nodes)
                {
                    if (!node.IsMaxLevel)
                    {
                        isActive = false;
                        break;
                    }
                }

                if (!isActive)
                {
                    break;
                }

                result++;
            }

            return result;
        }

        private void UpdateNodeFocus()
        {
            int currLevel = FindActivedCurrentLevel();
            m_ScrollRect.verticalNormalizedPosition = (float)currLevel / (float)(m_Levels.Count - 1);
        }

        private float GetResponsiveLevelGroupPadding()
        {
            float scale = (float)Screen.height / k_BaseHeight;
            return m_LevelGroupPadding * scale;
        }

        // Others

    } // Scope by class UIMasteryContent
} // namespace SkyDragonHunter