using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIMasteryEdgeContent : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UIMasteryContent m_UiMasteryContent;
        [SerializeField] private GameObject m_UiMasteryEdgeGroupPrefab;
        [SerializeField] private GameObject m_UiMasteryEdgePrefab;

        private List<GameObject> m_Levels = new List<GameObject>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            RectTransform thisRect = GetComponent<RectTransform>();
            RectTransform contentRect = m_UiMasteryContent.GetComponent<RectTransform>();

            thisRect.anchorMin = contentRect.anchorMin;
            thisRect.anchorMax = contentRect.anchorMax;
            thisRect.pivot = contentRect.pivot;

            thisRect.anchoredPosition = contentRect.anchoredPosition;
            thisRect.sizeDelta = contentRect.sizeDelta;

            thisRect.localScale = contentRect.localScale;

            for (int i = 0; i < m_UiMasteryContent.MaxLevel; ++i)
            {
                IncreaseLevel();
            }
        }

        // Public 메서드
        public void DrawUILineConnections(int level, UIMasteryNode[] edgeNodes)
        {
  
        }

        public void IncreaseLevel()
        {
            GameObject levelGroupInstance = Instantiate(m_UiMasteryEdgeGroupPrefab);
            levelGroupInstance.transform.SetParent(transform);
            levelGroupInstance.transform.position = Vector3.zero;
            levelGroupInstance.transform.localScale = Vector3.one;
            m_Levels.Add(levelGroupInstance);
        }

        // Private 메서드
        // Others

    } // Scope by class UIMasteryEdgeContent
} // namespace SkyDragonHunter