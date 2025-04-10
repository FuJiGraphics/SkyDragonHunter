using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UIMasteryContent : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private GameObject m_LevelGroupPrefab;

        private List<GameObject> m_Levels = new List<GameObject>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void IncreaseLevel()
        {
            GameObject levelGroupInstance = Instantiate(m_LevelGroupPrefab);
            levelGroupInstance.transform.SetParent(transform);
            levelGroupInstance.transform.position = Vector3.zero;
            levelGroupInstance.transform.localScale = Vector3.one;
            m_Levels.Add(levelGroupInstance);
        }

        public void AddMasteryNode(GameObject node, int level)
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
        }

        // Private 메서드
        // Others

    } // Scope by class UIMasteryContent
} // namespace SkyDragonHunter