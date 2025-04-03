using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {

    public enum EnemySearchType
    {
        FullScan
    }

    public class EnemySearchProvider : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private EnemySearchType m_EnemySearchType;

        // 속성 (Properties)
        public GameObject Target { get; private set; }

        // 외부 종속성 필드 (External dependencies field)
        [SerializeField] private AttackTargetSelector m_AttackTargetSelector;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (m_AttackTargetSelector == null)
                return;

            SearchEnemy();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_AttackTargetSelector = GetComponent<AttackTargetSelector>();
            if (m_AttackTargetSelector == null)
            {
                Debug.Log("[EnemySearchProvider]: AttackTargetSelector 컴포넌트가 필요합니다.");
                return;
            }
        }

        private void SearchEnemy()
        {
            switch (m_EnemySearchType)
            {
                case EnemySearchType.FullScan:
                    FullScan();
                    break;
            }
        }

        private void FullScan()
        {
            foreach (string tag in m_AttackTargetSelector.AllowedTargetTags)
            {
                Target = GameObject.FindWithTag(tag);
            }
        }

        // Others

    } // Scope by class EnemySearchProvider
} // namespace SkyDragonHunter