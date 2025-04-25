using SkyDragonHunter.Database;
using System;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class CanonDummy
    {
        // 필드 (Fields)
        private int m_Level;

        // 속성 (Properties)
        public int ID { get; set; }
        public int Level 
        {
            get => m_Level;
            set
            {
                m_Level = value;
                m_OnLevelChangedEvents?.Invoke(m_Level);
            }
        }
        public CanonType Type { get; set; }
        public CanonGrade Grade { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        private event Action<int> m_OnLevelChangedEvents;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddLevelChangedEvent(Action<int> action)
            => m_OnLevelChangedEvents += action;

        public GameObject GetCanonInstance()
        {
            GameObject prefab = CanonTable.Get(Type, Grade);
            GameObject instance = null;
            if (prefab != null)
            {
                instance = GameObject.Instantiate(prefab);
            }
            return instance;
        }

        // Private 메서드
        // Others

    } // Scope by class Item
} // namespace SkyDragonHunter